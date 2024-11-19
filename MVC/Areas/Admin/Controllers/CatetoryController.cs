using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Utilities;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitWork _unitWork;
        public CategoryController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Category category = new Category();
            if (id == null) {
                category.State = true;
                return View(category);
            }

            category = await _unitWork.CategoryRepository.Get(id.GetValueOrDefault());
            if (category.State != true) {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    await _unitWork.CategoryRepository.Create(category);
                    TempData[SD.OK] = "Creada ";
                }
                else
                {
                    _unitWork.CategoryRepository.Update(category);
                    TempData[SD.OK] = "Actualizada ";
                }
                await _unitWork.Save();

                return RedirectToAction(nameof(Index));
            }
            TempData[SD.ERROR] = "Error al actualizar";
            return View(category);
        }

        public async Task<IActionResult> CreateNew(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                category.State = true;
                return View(category);
            }

            category = await _unitWork.CategoryRepository.Get(id.GetValueOrDefault());
            if (category.State != true)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    await _unitWork.CategoryRepository.Create(category);
                }
                await _unitWork.Save();
            }
            TempData[SD.OK] = "Creada ";
            category.Id = 0;
            category.Name = "";
            category.Description = "";
            return View(category);
        }




        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var elements = await _unitWork.CategoryRepository.GetAll();
            return Json(new { data = elements });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var categoryDB = await _unitWork.CategoryRepository.Get(id);
            if (categoryDB == null)
            {
                return Json(new { success = false, message = "Error al eliminar" });
            }
            _unitWork.CategoryRepository.Remove(categoryDB);
            await _unitWork.Save();
            return Json(new { success = true, message = "Eliminada" });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, int id = 0)
        {
            bool value = false;
            var list = await _unitWork.CategoryRepository.GetAll();
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
