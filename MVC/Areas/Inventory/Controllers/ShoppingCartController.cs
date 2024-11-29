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
    public class ShoppingCartController : Controller
    {
        private readonly IUnitWork _unitWork;

        [BindProperty]
        public ShoppingCartViewModel shoppingcartVM { get; set; }
        public ShoppingCartController(IUnitWork unitWork)
        {
            _unitWork = unitWork;            
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingcartVM = new ShoppingCartViewModel();
            shoppingcartVM.Order = new Order();
            shoppingcartVM.ShoppingCartList = await _unitWork.ShoppingCartRepository.GetAll(x => x.ApplicationUserId == claim.Value, includPropertys: "Product");
            shoppingcartVM.Order.TotalOrder = 0;
            shoppingcartVM.Order.ApplicationUserId = claim.Value;

            foreach(var list in shoppingcartVM.ShoppingCartList)
            {
                list.Price = list.Product.Price;
                shoppingcartVM.Order.TotalOrder += list.Price * list.Quantity;
            }
            return View(shoppingcartVM);
        }

        public async Task<IActionResult> Plus(int shoppingid)
        {
            var shoppingCart = await _unitWork.ShoppingCartRepository.GetFirts(x => x.Id == shoppingid);
            shoppingCart.Quantity += 1;
            await _unitWork.Save();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Minus(int shoppingid)
        {
            var shoppingCart = await _unitWork.ShoppingCartRepository.GetFirts(x => x.Id == shoppingid);
            if(shoppingCart.Quantity == 1)
            {
                var shoppingList = await _unitWork.ShoppingCartRepository.GetAll(x => x.ApplicationUserId == shoppingCart.ApplicationUserId);
                var productsNumber = shoppingList.Count();
                _unitWork.ShoppingCartRepository.Remove(shoppingCart);
                await _unitWork.Save();
                HttpContext.Session.SetInt32(SD.ssShoppinCart, productsNumber -1);
            }
            else
            {
                shoppingCart.Quantity -= 1;
                await _unitWork.Save();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int shoppingid)
        {
            var shoppingCart = await _unitWork.ShoppingCartRepository.GetFirts(x => x.Id == shoppingid);
            var shoppingList = await _unitWork.ShoppingCartRepository.GetAll(x => x.ApplicationUserId == shoppingCart.ApplicationUserId);
            var productsNumber = shoppingList.Count();
            _unitWork.ShoppingCartRepository.Remove(shoppingCart);
            await _unitWork.Save();
            HttpContext.Session.SetInt32(SD.ssShoppinCart, productsNumber - 1);
            return RedirectToAction("Index");
        }

    }
}
