using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Utilities;
using System.ComponentModel;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitWork _unitWork;
        public BrandController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Brand brand = new Brand();
            if (id == null) {
                brand.State = true;
                return View(brand);
            }

            brand = await _unitWork.BrandRepository.Get(id.GetValueOrDefault());
            if (brand.State != true) {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (brand.Id == 0)
                {
                    await _unitWork.BrandRepository.Create(brand);
                    TempData[SD.OK] = "Creada ";
                }
                else
                {
                    _unitWork.BrandRepository.Update(brand);
                    TempData[SD.OK] = "Actualizada ";
                }
                await _unitWork.Save();

                return RedirectToAction(nameof(Index));
            }
            TempData[SD.ERROR] = "Error al actualizar";
            return View(brand);
        }

        public async Task<IActionResult> CreateNew(int? id)
        {
            Brand brand = new Brand();
            if (id == null)
            {
                brand.State = true;
                return View(brand);
            }

            brand = await _unitWork.BrandRepository.Get(id.GetValueOrDefault());
            if (brand.State != true)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (brand.Id == 0)
                {
                    await _unitWork.BrandRepository.Create(brand);
                }
                await _unitWork.Save();
            }
            TempData[SD.OK] = "Creada ";
            brand.Id = 0;
            brand.Name = "";
            brand.Description = "";
            return View(brand);
        }




        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var elements = await _unitWork.BrandRepository.GetAll();
            return Json(new { data = elements });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var brandDB = await _unitWork.BrandRepository.Get(id);
            if (brandDB == null)
            {
                return Json(new { success = false, message = "Error al eliminar" });
            }
            _unitWork.BrandRepository.Remove(brandDB);
            await _unitWork.Save();
            return Json(new { success = true, message = "Eliminada" });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, int id = 0)
        {
            bool value = false;
            var list = await _unitWork.BrandRepository.GetAll();
            if( id == 0)
            {
                value = list.Any(b => b.Name.ToLower().Equals(name, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                value = list.Any(b => b.Name.ToLower().Equals(name, StringComparison.CurrentCultureIgnoreCase) && b.Id != id);
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
