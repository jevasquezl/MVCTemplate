using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitWork _unitWork;
        public CompanyController(IUnitWork unitWork)
        {
            _unitWork = unitWork;

        }
        
        public async Task<IActionResult> Upsert()
        {
            CompanyViewModel companyVM = new CompanyViewModel()
            {
                Company = new Models.Company(),
                StoreList = _unitWork.InventoryRepository.GetAllDropDown("Store")
            };

            companyVM.Company = await _unitWork.CompanytRepository.GetFirts();
            if (companyVM.Company == null)
            {
                companyVM.Company = new Models.Company();
            }
            return View(companyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CompanyViewModel companyVM)
        {
            if (ModelState.IsValid)
            {
                TempData[SD.OK] = "Registrada";
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (companyVM.Company.Id == 0)
                {
                    companyVM.Company.CreatedId = claim.Value;
                    companyVM.Company.UpdatedId = claim.Value;
                    companyVM.Company.CreatedDate = DateTime.Now;
                    companyVM.Company.UpdatedDate = DateTime.Now;
                    await _unitWork.CompanytRepository.Create(companyVM.Company);
                }
                else
                {
                    companyVM.Company.UpdatedId = claim.Value;
                    companyVM.Company.UpdatedDate = DateTime.Now;
                    _unitWork.CompanytRepository.Update(companyVM.Company);
                }
                await _unitWork.Save();
                return RedirectToAction("Index", "Home", new {area="Inventory"});
            }
            TempData[SD.ERROR] = "Error al registrar company";
            return View(companyVM);
        }

    }
}
