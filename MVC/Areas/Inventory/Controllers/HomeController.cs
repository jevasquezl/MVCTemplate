using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Models.ErrorViewModels;
using MVC.Models.Especifications;
using MVC.Models.ViewModels;
using MVC.Utilities;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitWork _unitWork;

        [BindProperty]
        private ShoppingCartViewModel _shoppingCartViewModel {  get; set; }


        public HomeController(ILogger<HomeController> logger,IUnitWork unitWork)
        {
            _logger = logger;
            _unitWork = unitWork;
        }

        //public async Task<IActionResult> Index()
        //{
        //    IEnumerable<Product> productList = await _unitWork.ProductRepository.GetAll(); 
        //    return View(productList);
        //}
        public async Task<IActionResult> Index(int pageNumber = 1, string search="", string actualSearch="")
        {
            //Sesion
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            if (claim != null)
            {
                var shoppingList = await _unitWork.ShoppingCartRepository.GetAll(x => x.ApplicationUserId == claim.Value);
                var productNumber = shoppingList.Count();
                HttpContext.Session.SetInt32(SD.ssShoppinCart, productNumber);
            }

            if (!string.IsNullOrEmpty(search))
            {
                pageNumber = 1;
            }
            else
            {
                search = actualSearch;
            }
            ViewData["ActualSearch"] = search;

            if(pageNumber < 1) { pageNumber = 1; }
            Parameters parameters = new Parameters()
            {
                PageNumber = pageNumber,
                PageSize = 4
            };
            var result = _unitWork.ProductRepository.GetAllPages(parameters);

            if (!string.IsNullOrEmpty(search))
            {
                result = _unitWork.ProductRepository.GetAllPages(parameters, p=>p.Description.Contains(search));
            }
            ViewData["TotalPages"] = result.MetaData.TotalPages;
            ViewData["TotalRows"] = result.MetaData.TotalCount;
            ViewData["PageSize"] = result.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Preview"] = "disabled";
            ViewData["Next"] = "";
            if (pageNumber > 1)
            {
                ViewData["Preview"] = "";
            }
            if (result.MetaData.TotalPages <= pageNumber)
            {
                ViewData["Next"] = "disabled";
            }

            return View(result);
        }

        public async Task<IActionResult> Detail(int id)
        {
            _shoppingCartViewModel = new ShoppingCartViewModel();
            _shoppingCartViewModel.Company = await _unitWork.CompanytRepository.GetFirts();
            _shoppingCartViewModel.Product = await _unitWork.ProductRepository.GetFirts(x => x.Id == id,
                includeProperties: "Brand,Category");
            var storeProduct = await _unitWork.StoreProductRepository.GetFirts(x => x.ProductId ==  id && 
                                                                                    x.StoreId == _shoppingCartViewModel.Company.StoreSaleId);
            if (storeProduct == null)
            {
                _shoppingCartViewModel.Stock = 0;
            }
            else
            {
                _shoppingCartViewModel.Stock = storeProduct.Quantity;
            }

            _shoppingCartViewModel.ShoppingCart = new ShoppingCart()
            {
                Product = _shoppingCartViewModel.Product,
                ProductId = _shoppingCartViewModel.Product.Id,
            };
            return View(_shoppingCartViewModel);
                
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Detail(ShoppingCartViewModel shoppingcartVM)
        {
            var claimIdentity =(ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingcartVM.ShoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart shoppingBD = await _unitWork.ShoppingCartRepository.GetFirts(x => x.ApplicationUserId == claim.Value &&
            x.ProductId == shoppingcartVM.ShoppingCart.ProductId);

            if (shoppingBD == null)
            {
                await _unitWork.ShoppingCartRepository.Create(shoppingcartVM.ShoppingCart);
            }
            else
            {
                shoppingBD.Quantity += shoppingcartVM.ShoppingCart.Quantity;
                _unitWork.ShoppingCartRepository.Update(shoppingBD);
            }
            await _unitWork.Save();
            TempData[SD.OK] = "Registro adicionado";
            var shoppingList = await _unitWork.ShoppingCartRepository.GetAll(x => x.ApplicationUserId == claim.Value);
            var productNumber = shoppingList.Count();
            HttpContext.Session.SetInt32(SD.ssShoppinCart, productNumber);


            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
