using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Utilities;
using Rotativa.AspNetCore;
using Stripe;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles = SD.Role_Admin + "," +  SD.Role_Storage)]
    public class InventoryController : Controller
    {
        private readonly IUnitWork _unitWork;
       
        [BindProperty]
        public InventoryViewModel inventoryVM { get; set; }
        public InventoryController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }

        // GET: InventoryController
        public ActionResult Index()
        {
            return View();
        }

        #region
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var elements = await _unitWork.StoreProductRepository.GetAll(includeProperties: "Store,Product");
            return Json(new { data = elements });
        }

        public async Task<IActionResult> Add(int id)
        {
            inventoryVM = new InventoryViewModel();
            var detail = await _unitWork.InventoryDetailRepository.Get(id);
            inventoryVM.Inventory = await _unitWork.InventoryRepository.Get(detail.InventoryId);
            detail.Quantity += 1;
            await _unitWork.Save();
            return RedirectToAction("InventoryDetail", new { id = inventoryVM.Inventory.Id });
        }

        public async Task<IActionResult> subtract(int id)
        {
            inventoryVM = new InventoryViewModel();
            var detail = await _unitWork.InventoryDetailRepository.Get(id);
            inventoryVM.Inventory = await _unitWork.InventoryRepository.Get(detail.InventoryId);
            if(detail.Quantity == 1)
            {
                _unitWork.InventoryDetailRepository.Remove(detail);
                await _unitWork.Save();
            }
            else
            { 
                detail.Quantity -= 1;
                await _unitWork.Save();
            }
            return RedirectToAction("InventoryDetail", new { id = inventoryVM.Inventory.Id });

        }

        public async Task<IActionResult> AddStock(int id)
        {
            var inventory = await _unitWork.InventoryRepository.Get(id);
            var detailList = await _unitWork.InventoryDetailRepository.GetAll(d => d.InventoryId == id);
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            foreach (var item in detailList)
            {
                var storeProduct = new StoreProduct();
                storeProduct = await _unitWork.StoreProductRepository.GetFirts(e => e.ProductId == item.ProductId &&
                                                                                   e.StoreId == inventory.StoreId);
                if(storeProduct != null)  //El producto existe
                {
                    await _unitWork.KardexRepository.CreateKardex(storeProduct.Id, "Input",
                        "Transaction input", storeProduct.Quantity, item.Quantity, claim.Value);

                    storeProduct.Quantity += item.Quantity;
                    await _unitWork.Save();
                }
                else
                {
                    storeProduct = new StoreProduct();
                    storeProduct.StoreId = inventory.StoreId;
                    storeProduct.ProductId = item.ProductId;
                    storeProduct.Quantity = item.Quantity;
                    await _unitWork.StoreProductRepository.Create(storeProduct);
                    await _unitWork.Save();

                    await _unitWork.KardexRepository.CreateKardex(storeProduct.Id, "Input",
                        "Initial inventory", 0, item.Quantity, claim.Value);
                }
            }
            //Update inventory
            inventory.State = true;
            inventory.EndDate = DateTime.Now;
            await _unitWork.Save();
            TempData[SD.OK] = "Stock Creado con exito";
            return RedirectToAction("Index");
        }

        public IActionResult CreateKardex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateKardex(string startdate, string lastdate, int productId)
        {
            if (lastdate == null) lastdate = DateTime.Now.AddMonths(2).ToString();
            return RedirectToAction("KardexResult", new { startdate, lastdate, productId });
        }

        public async Task<IActionResult> KardexResult(string startdate, string lastdate, int productId)
        {
            
            KardexViewModel kardexVM = new KardexViewModel();
            kardexVM.Product = new MVC.Models.Product();
            kardexVM.Product = await _unitWork.ProductRepository.Get(productId);
            kardexVM.StartDate = DateTime.Parse(startdate);
            kardexVM.LastDate = DateTime.Parse(lastdate).AddHours(23).AddMinutes(59);
            kardexVM.KardexList = await _unitWork.KardexRepository.GetAll(
                k => k.StoreProduct.ProductId == productId &&
                (k.RegisterDate >= kardexVM.StartDate && k.RegisterDate <= kardexVM.LastDate),
            includeProperties: "StoreProduct,StoreProduct.Product,StoreProduct.Store",
                orderBy: o => o.OrderBy(o=>o.RegisterDate)
                );

            return View(kardexVM);
        }
        public async Task<IActionResult> CreateNew(int? id)
        {

            inventoryVM = new InventoryViewModel()
            {
                Inventory = new(),
                StoreList = _unitWork.InventoryRepository.GetAllDropDown("Store"),
            };

            inventoryVM.Inventory.State = false;
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            inventoryVM.Inventory.ApplicationUserId = claim.Value;
            inventoryVM.Inventory.StartDate = DateTime.Now;
            inventoryVM.Inventory.EndDate = DateTime.Now;


            return View(inventoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(InventoryViewModel inventoryViewModel)
        {
            if(ModelState.IsValid)
            {
                inventoryViewModel.Inventory.StartDate = DateTime.Now;
                inventoryViewModel.Inventory.EndDate = DateTime.Now;
                await _unitWork.InventoryRepository.Create(inventoryViewModel.Inventory);
                await _unitWork.Save();
                return RedirectToAction("InventoryDetail",
                    new { id = inventoryViewModel.Inventory.Id });
            }

            inventoryViewModel.StoreList = _unitWork.InventoryRepository.GetAllDropDown("Store");

            return View(inventoryViewModel);
        }

        public async Task<IActionResult> InventoryDetail(int id)
        {
            inventoryVM = new InventoryViewModel();
            inventoryVM.Inventory = await _unitWork.InventoryRepository.GetFirts(i => i.Id == id, includeProperties: "Store");
            inventoryVM.InventoryDetails = await _unitWork.InventoryDetailRepository.GetAll
                (x => x.InventoryId == id, includeProperties:"Product,Product.Brand");

            return View(inventoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> InventoryDetail(int inventoryId, int productId, int quantity)
        {
            inventoryVM = new InventoryViewModel();
            inventoryVM.Inventory = await _unitWork.InventoryRepository.GetFirts(i => i.Id == inventoryId);
            
            var storeProduct = await _unitWork.StoreProductRepository.GetFirts(x => x.ProductId == productId &&
            x.StoreId == inventoryVM.Inventory.StoreId);
            
            var detail = _unitWork.InventoryDetailRepository.GetFirts(y => y.InventoryId == inventoryId &&
            y.ProductId == productId);

            if (detail.Result == null)
            {
                inventoryVM.InventoryDetail = new InventoryDetail();
                inventoryVM.InventoryDetail.ProductId = productId;
                inventoryVM.InventoryDetail.InventoryId = inventoryId;
                if (storeProduct != null) 
                {
                    inventoryVM.InventoryDetail.LastStock = storeProduct.Quantity;
                }
                else
                {
                    inventoryVM.InventoryDetail.LastStock = 0;
                }
                inventoryVM.InventoryDetail.Quantity = quantity;
                await _unitWork.InventoryDetailRepository.Create(inventoryVM.InventoryDetail);
                await _unitWork.Save();
            }
            else
            {
                detail.Result.Quantity += quantity;
                await _unitWork.Save();
            }
            return RedirectToAction("InventoryDetail", new { id= inventoryId });
        }


        [HttpGet]
        public async Task<IActionResult> SearchProduct(string term)
        {
            if(!string.IsNullOrEmpty(term))
            {
                var ProductList = await _unitWork.ProductRepository.GetAll(p => p.State == true);
                var data = ProductList.Where(x => x.Serie.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.Description.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();

                return Ok(data);

            }

            return Ok();
        }

        public async Task<IActionResult> PrintKardex(string startdate, string lastdate, int productId)
        {

            KardexViewModel kardexVM = new KardexViewModel();
            kardexVM.Product = new MVC.Models.Product();
            kardexVM.Product = await _unitWork.ProductRepository.Get(productId);
            kardexVM.StartDate = DateTime.Parse(startdate);
            kardexVM.LastDate = DateTime.Parse(lastdate).AddHours(23).AddMinutes(59);
            kardexVM.KardexList = await _unitWork.KardexRepository.GetAll(
                k => k.StoreProduct.ProductId == productId &&
                (k.RegisterDate >= kardexVM.StartDate && k.RegisterDate <= kardexVM.LastDate),
            includeProperties: "StoreProduct,StoreProduct.Product,StoreProduct.Store",
                orderBy: o => o.OrderBy(o => o.RegisterDate)
                );

            return new ViewAsPdf("PrintKardex", kardexVM)
            {
                FileName = "Kardex.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page--pffset 0 --footer-center [page] --footer-font-size 12"
            };
        }
        #endregion
    }
}
