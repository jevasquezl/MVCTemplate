using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVC.DataAccess.Data;
using MVC.DataAccess.Repository;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController(IUnitWork unitWork, ApplicationDbContext context) : Controller
    {

        // GET: UserController
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(string? id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var user = await unitWork.ApplicationUserRepository.GetEntity(id);
            if (user.Id != "")
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id.IsNullOrEmpty())
                {
                    await unitWork.ApplicationUserRepository.Create(user);
                    TempData[SD.OK] = "Creada ";
                }
                else
                {
                    unitWork.ApplicationUserRepository.Update(user);
                    TempData[SD.OK] = "Actualizada ";
                }
                await unitWork.Save();

                return RedirectToAction(nameof(Index));
            }
            TempData[SD.ERROR] = "Error al actualizar";
            return View(user);
        }

        public async Task<IActionResult> CreateNew(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            ApplicationUser user = new();

            if (user.Id.IsNullOrEmpty())
            {
                return View(user);
            }

            user = await unitWork.ApplicationUserRepository.GetEntity(id);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id.IsNullOrEmpty())
                {
                    await unitWork.ApplicationUserRepository.Create(user);
                }
                await unitWork.Save();
            }
            TempData[SD.OK] = "Creada ";
            return View(user);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userList = await unitWork.ApplicationUserRepository.GetAll();
            IEnumerable<IdentityUserRole<string>> userRoles = await context.UserRoles.ToListAsync();
            var roles = await context.Roles.ToListAsync();

            foreach (var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                if (roleId != null)
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                }
            }
            return Json(new { data = userList });
        }

        [HttpPost]
        public async Task<IActionResult> LockandUnlock([FromBody] string id)
        {
            var user = await unitWork.ApplicationUserRepository.GetFirts(U => U.Id == id);
            if (user == null)
            {
                return Json(new { success = false, message = "Error de usuario" });
            }
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now.AddYears(100))
            {
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            await unitWork.Save();
            return Json(new { success = true, message = "Operacion exitosa" });

        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var storeDB = await unitWork.ApplicationUserRepository.Get(id);
            if (storeDB == null)
            {
                return Json(new { success = false, message = "Error al eliminar" });
            }
            unitWork.ApplicationUserRepository.Remove(storeDB);
            await unitWork.Save();
            return Json(new { success = true, message = "Eliminada" });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string email, string id = "")
        {
            bool value = false;
            var list = await unitWork.ApplicationUserRepository.GetAll();
            if (id == "")
            {
                value = list.Any(b => b.Email.ToLower().Equals(email, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                value = list.Any(b => b.Email.ToLower().Equals(email, StringComparison.CurrentCultureIgnoreCase) && b.Id != id);
            }
            if (value)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }


        #endregion
    }
}
