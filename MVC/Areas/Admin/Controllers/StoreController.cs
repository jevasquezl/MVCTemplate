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
    public class StoreController : Controller
    {
        private readonly IUnitWork _unitWork;
        public StoreController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Store store = new Store();
            if (id == null) {
                store.State = true;
                return View(store);
            }

            store = await _unitWork.StoreRepository.Get(id.GetValueOrDefault());
            if (store.State != true) {
                return NotFound();
            }
            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Store store)
        {
            if (ModelState.IsValid)
            {
                if (store.Id == 0)
                {
                    await _unitWork.StoreRepository.Create(store);
                    TempData[SD.OK] = "Creada ";
                }
                else
                {
                    _unitWork.StoreRepository.Update(store);
                    TempData[SD.OK] = "Actualizada ";
                }
                await _unitWork.Save();

                return RedirectToAction(nameof(Index));
            }
            TempData[SD.ERROR] = "Error al actualizar";
            return View(store);
        }

        public async Task<IActionResult> CreateNew(int? id)
        {
            Store store = new Store();
            if (id == null)
            {
                store.State = true;
                return View(store);
            }

            store = await _unitWork.StoreRepository.Get(id.GetValueOrDefault());
            if (store.State != true)
            {
                return NotFound();
            }
            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(Store store)
        {
            if (ModelState.IsValid)
            {
                if (store.Id == 0)
                {
                    await _unitWork.StoreRepository.Create(store);
                }
                await _unitWork.Save();
            }
            TempData[SD.OK] = "Creada ";
            store.Id = 0;
            store.Name = "";
            store.Description = "";
            return View(store);
        }




        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var elements = await _unitWork.StoreRepository.GetAll();
            return Json(new { data = elements });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var storeDB = await _unitWork.StoreRepository.Get(id);
            if (storeDB == null)
            {
                return Json(new { success = false, message = "Error al eliminar" });
            }
            _unitWork.StoreRepository.Remove(storeDB);
            await _unitWork.Save();
            return Json(new { success = true, message = "Eliminada" });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, int id = 0)
        {
            bool value = false;
            var list = await _unitWork.StoreRepository.GetAll();
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
