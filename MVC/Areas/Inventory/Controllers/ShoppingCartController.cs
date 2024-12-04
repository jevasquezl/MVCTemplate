using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Models.ErrorViewModels;
using MVC.Models.Especifications;
using MVC.Models.ViewModels;
using MVC.Utilities;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Stripe.Checkout;

namespace MVC.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitWork _unitWork;
        private string _webUrl;

        [BindProperty]
        public ShoppingCartViewModel shoppingcartVM { get; set; }
        public ShoppingCartController(IUnitWork unitWork, IConfiguration configuration)
        {
            _unitWork = unitWork;
            _webUrl = configuration.GetValue<string>("DomainUrls:WEB_URL");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingcartVM = new ShoppingCartViewModel();
            shoppingcartVM.Order = new Order();
            shoppingcartVM.ShoppingCartList = await _unitWork.ShoppingCartRepository.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product");
            shoppingcartVM.Order.TotalOrder = 0;
            shoppingcartVM.Order.ApplicationUserId = claim.Value;

            foreach (var list in shoppingcartVM.ShoppingCartList)
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
            if (shoppingCart.Quantity == 1)
            {
                var shoppingList = await _unitWork.ShoppingCartRepository.GetAll(x => x.ApplicationUserId == shoppingCart.ApplicationUserId);
                var productsNumber = shoppingList.Count();
                _unitWork.ShoppingCartRepository.Remove(shoppingCart);
                await _unitWork.Save();
                HttpContext.Session.SetInt32(SD.ssShoppinCart, productsNumber - 1);
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

        public async Task<IActionResult> Process()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingcartVM = new ShoppingCartViewModel()
            {
                Order = new Order(),
                ShoppingCartList = await _unitWork.ShoppingCartRepository.GetAll(
                x => x.ApplicationUserId == claim.Value, includeProperties: "Product"),
                Company = await _unitWork.CompanytRepository.GetFirts()
            };
            shoppingcartVM.Order.TotalOrder = 0;
            shoppingcartVM.Order.ApplicationUser = await _unitWork.ApplicationUserRepository.GetFirts(x => x.Id == claim.Value);

            foreach (var list in shoppingcartVM.ShoppingCartList)
            {
                list.Price = list.Product.Price;
                shoppingcartVM.Order.TotalOrder += list.Product.Price * list.Quantity;
            }
            shoppingcartVM.Order.CustomerName = shoppingcartVM.Order.ApplicationUser.Names;
            shoppingcartVM.Order.Telephone = shoppingcartVM.Order.ApplicationUser.PhoneNumber;
//Aqui faltan datos de direccion en application user
                
            foreach (var list in shoppingcartVM.ShoppingCartList)
            {
                var product = await _unitWork.StoreProductRepository.GetFirts(
                    x => x.ProductId == list.ProductId &&
                         x.StoreId == shoppingcartVM.Company.StoreSaleId);
                if (list.Quantity > product.Quantity)
                {
                    TempData[SD.ERROR] = "Cantidad de producto" + list.Product.Description + " Excede al stock actual";
                    return RedirectToAction("Index");
                }
            }

            return View(shoppingcartVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(ShoppingCart shoppingCart)
        {

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingcartVM.ShoppingCartList = await _unitWork.ShoppingCartRepository.GetAll(
                x => x.ApplicationUserId == claim.Value,
                includeProperties: "Product");
            shoppingcartVM.Company = await _unitWork.CompanytRepository.GetFirts();
            shoppingcartVM.Order.TotalOrder = 0;
            shoppingcartVM.Order.ApplicationUserId = claim.Value;
            shoppingcartVM.Order.DateOrder = DateTime.Now;

            foreach (var list in shoppingcartVM.ShoppingCartList)
            {
                list.Price = list.Product.Price;
                shoppingcartVM.Order.TotalOrder += list.Price * list.Quantity;
            }

            foreach (var list in shoppingcartVM.ShoppingCartList)
            {
                var product = await _unitWork.StoreProductRepository.GetFirts(
                x => x.ProductId == list.ProductId &&
                x.StoreId == shoppingcartVM.Company.StoreSaleId);
                if (list.Quantity > product.Quantity)
                {
                    TempData[SD.ERROR] = "Cantidad de producto" + list.Product.Description + " Excede al stock actual (" + product.Quantity + ")";
                    return RedirectToAction("Index");
                }
            }
            shoppingcartVM.Order.StateOrder = SD.StateOrderPending;
            shoppingcartVM.Order.StatePay = SD.StatePayedPending;
                
            await _unitWork.OrderRepository.Create(shoppingcartVM.Order);
            await _unitWork.Save();

            //Save detail
            foreach(var detail in shoppingcartVM.ShoppingCartList)
            {
                OrderDetail orderdet = new OrderDetail()
                {
                    ProductId = detail.ProductId,
                    OrderId = shoppingcartVM.Order.Id,
                    Price = detail.Price,
                    Quantity = detail.Quantity
                };
                await _unitWork.OrderDetailRepository.Create(orderdet);
                await _unitWork.Save();
            }
            //Stripe
            var users = await _unitWork.ApplicationUserRepository.GetFirts(x=>x.Id == claim.Value);
            var options = new SessionCreateOptions()
            {
                SuccessUrl = _webUrl + $"inventory/ShoppingCart/ConfirmationOrder?id={shoppingcartVM.Order.Id}",
                CancelUrl = _webUrl + $"inventory/ShoppingCart/CancelOrder?id={shoppingcartVM.Order.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = users.Email
            };
            foreach(var list in shoppingcartVM.ShoppingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(list.Price * 100),  //$20 se manda 200
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = list.Product.Description
                        }
                    },
                    Quantity = list.Quantity
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitWork.OrderRepository.UpdatePayStripeId(shoppingcartVM.Order.Id, session.Id, session.PaymentIntentId);
            await _unitWork.Save();
            Response.Headers.Add("Location", session.Url);  //Redirection to Stripe


            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> ConfirmationOrder(int id)
        {
            var order = await _unitWork.OrderRepository.GetFirts(x => x.Id == id, includeProperties: "ApplicationUser");
            var service = new SessionService();
            Session session = service.Get(order.SessionId);
            var shoppingCart = await _unitWork.ShoppingCartRepository.GetAll(x => x.ApplicationUserId == order.ApplicationUserId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitWork.OrderRepository.UpdatePayStripeId(id, session.Id, session.PaymentIntentId);
                _unitWork.OrderRepository.UpdateState(id, SD.StateOrderApproved, SD.StatePayedApproved);
                await _unitWork.Save();
                //Disminuir Stock de Sale Store
                var company = await _unitWork.CompanytRepository.GetFirts();
                foreach(var list in shoppingCart)
                {
                    var storeProduct = new StoreProduct();
                    storeProduct = await _unitWork.StoreProductRepository.GetFirts(
                        x => x.ProductId == list.ProductId &&
                             x.StoreId == company.StoreSaleId
                        );
                    await _unitWork.KardexRepository.CreateKardex(storeProduct.Id, "Output","Sale -Order#"+id,
                                    storeProduct.Quantity,list.Quantity,order.ApplicationUserId);

                    storeProduct.Quantity -= list.Quantity;
                    await _unitWork.Save();
                }

            }
            //Eliminar el shoppincart
            List<ShoppingCart> shoppingCartList = shoppingCart.ToList();
            _unitWork.ShoppingCartRepository.RemoveRange(shoppingCartList);
            await _unitWork.Save();
            HttpContext.Session.SetInt32(SD.ssShoppinCart, 0);

            return View(id);
        }

        public async Task<IActionResult> CancelOrder(int id)
        {
            _unitWork.OrderRepository.UpdateState(id, SD.StateOrderCancel, SD.StatePayedCancel);
            await _unitWork.Save();

            return View(id);
        }
    }
}


