using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Utilities;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Storage)]
    public class ProductController : Controller
    {
        private readonly IUnitWork _unitWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitWork unitWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitWork = unitWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductViewModel productVM = new()
            {
                Product = new(),
                CategoryList = _unitWork.ProductRepository.GetAllDropDown("Category"),
                BrandsList = _unitWork.ProductRepository.GetAllDropDown("Brand"),
                ParentList = _unitWork.ProductRepository.GetAllDropDown("Product")
            };
            if(id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = await _unitWork.ProductRepository.Get(id.GetValueOrDefault());
                if(productVM.Product == null)
                {
                    return NotFound();
                }
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductViewModel productViewModel)
        {
            if(ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if(productViewModel.Product.Id == 0)
                {
                    string upload = webRootPath + SD.ImagenRoute;
                    string fileName = Guid.NewGuid().ToString();
                    string ext = Path.GetExtension(files[0].FileName);
                    using(var fileStream = new FileStream(Path.Combine(upload,fileName+ext),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productViewModel.Product.ImageUrl = fileName + ext;
                    await _unitWork.ProductRepository.Create(productViewModel.Product);
                }
                else
                {
                    var objProduct = await _unitWork.ProductRepository.GetFirts(p => p.Id == productViewModel.Product.Id, isTracking : false);
                    if(files.Count>0) //If it is new image
                    {
                        string upload = webRootPath + SD.ImagenRoute;
                        string fileName = Guid.NewGuid().ToString();
                        string ext = Path.GetExtension(files[0].FileName);
                        //Delete last image
                        var lastFile = Path.Combine(upload, objProduct.ImageUrl);
                        if(System.IO.File.Exists(lastFile))
                        {
                            System.IO.File.Delete(lastFile);
                        }
                        using(var fileStream = new FileStream(Path.Combine(upload, fileName + ext),FileMode.Create))
                        {
                            files[0].CopyTo(fileStream) ;
                        }
                        productViewModel.Product.ImageUrl= fileName + ext;
                    }
                    else
                    {
                        productViewModel.Product.ImageUrl = objProduct.ImageUrl;
                    }
                    _unitWork.ProductRepository.Update(productViewModel.Product);
                }
                TempData[SD.OK] = "Actualizado";
                await _unitWork.Save();
                return View("Index");
            }
            productViewModel.CategoryList = _unitWork.ProductRepository.GetAllDropDown("Category");
            productViewModel.BrandsList = _unitWork.ProductRepository.GetAllDropDown("Brand");
            productViewModel.ParentList = _unitWork.ProductRepository.GetAllDropDown("Product");
            return View(productViewModel);
        }

        public async Task<IActionResult> CreateNew(int? id)
        {

            ProductViewModel productVM = new ProductViewModel
            {
                Product = new(),
                CategoryList = _unitWork.ProductRepository.GetAllDropDown("Category"),
                BrandsList = _unitWork.ProductRepository.GetAllDropDown("Brand"),
                ParentList = _unitWork.ProductRepository.GetAllDropDown("Product")
            };
            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = await _unitWork.ProductRepository.Get(id.GetValueOrDefault());
                if (productVM.Product == null)
                {
                    return NotFound();
                }
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productViewModel.Product.Id == 0)
                {
                    string upload = webRootPath + SD.ImagenRoute;
                    string fileName = Guid.NewGuid().ToString();
                    string ext = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productViewModel.Product.ImageUrl = fileName + ext;
                    await _unitWork.ProductRepository.Create(productViewModel.Product);
                }
                else
                {
                    var objProduct = await _unitWork.ProductRepository.GetFirts(p => p.Id == productViewModel.Product.Id, isTracking: false);
                    if (files.Count > 0) //If it is new image
                    {
                        string upload = webRootPath + SD.ImagenRoute;
                        string fileName = Guid.NewGuid().ToString();
                        string ext = Path.GetExtension(files[0].FileName);
                        //Delete last image
                        var lastFile = Path.Combine(upload, objProduct.ImageUrl);
                        if (System.IO.File.Exists(lastFile))
                        {
                            System.IO.File.Delete(lastFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productViewModel.Product.ImageUrl = fileName + ext;
                    }
                    else
                    {
                        productViewModel.Product.ImageUrl = objProduct.ImageUrl;
                    }
                    _unitWork.ProductRepository.Update(productViewModel.Product);
                }
                TempData[SD.OK] = "Registrado";
                await _unitWork.Save();
                productViewModel.CategoryList = _unitWork.ProductRepository.GetAllDropDown("Category");
                productViewModel.BrandsList = _unitWork.ProductRepository.GetAllDropDown("Brand");
                productViewModel.ParentList = _unitWork.ProductRepository.GetAllDropDown("Product");
                return View(productViewModel);
            }
            productViewModel.CategoryList = _unitWork.ProductRepository.GetAllDropDown("Category");
            productViewModel.BrandsList = _unitWork.ProductRepository.GetAllDropDown("Brand");
            productViewModel.ParentList = _unitWork.ProductRepository.GetAllDropDown("Product");
            return View(productViewModel);
        }




        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var elements = await _unitWork.ProductRepository.GetAll(includeProperties:"Category,Brand");
            return Json(new { data = elements });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var producDB = await _unitWork.ProductRepository.Get(id);
            if (producDB == null)
            {
                return Json(new { success = false, message = "Error al eliminar" });
            }
            string upload = _webHostEnvironment.WebRootPath + SD.ImagenRoute;
            var lastfile = Path.Combine( upload, producDB.ImageUrl);
            if(System.IO.File.Exists(lastfile))
            {
                System.IO.File.Delete(lastfile);
            }
            _unitWork.ProductRepository.Remove(producDB);
            await _unitWork.Save();
            return Json(new { success = true, message = "Eliminada" });
        }

        [ActionName("ValidateSerie")]
        public async Task<IActionResult> ValidateSerie(string serie, int id = 0)
        {
            bool value = false;
            var list = await _unitWork.ProductRepository.GetAll();
            if( id == 0)
            {
                value = list.Any(b => b.Serie.ToLower().Equals(serie, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                value = list.Any(b => b.Serie.ToLower().Equals(serie, StringComparison.CurrentCultureIgnoreCase) && b.Id != id);
            }
            if(value)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }
        #endregion
    }
}
