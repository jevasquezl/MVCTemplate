using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitWork _unitWork;

        [BindProperty]
        private OrderDetailViewModel _orderDetailViewModel {  get; set; }

        public OrderController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            _orderDetailViewModel = new OrderDetailViewModel()
            {
                Order = await _unitWork.OrderRepository.GetFirts(x => x.Id == id, includeProperties: "ApplicationUser"),
                OrderDetailList = await _unitWork.OrderDetailRepository.GetAll(x => x.OrderId == id, includeProperties: "Product")
            };
            return View(_orderDetailViewModel);
        }

        public async Task<IActionResult> Process(int id)
        {
            var order = await _unitWork.OrderRepository.GetFirts(x => x.Id == id);
            order.StateOrder = SD.StateOrderInProcess;
            await _unitWork.Save();
            TempData[SD.OK] = "Order change to inProcess";
            return RedirectToAction("Detail", new { id = id});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> SendOrder(OrderDetailViewModel orderDetailViewModel)
        {
            var order = await _unitWork.OrderRepository.GetFirts(x => x.Id == orderDetailViewModel.Order.Id);
            order.StateOrder = SD.StateOrderSend;
            order.Carrier = orderDetailViewModel.Order.Carrier;
            order.DeliveryNumber = orderDetailViewModel.Order.DeliveryNumber;
            order.DeliveryOrder = DateTime.Now;
            await _unitWork.Save();
            TempData[SD.OK] = "Order change to Sended";
            return RedirectToAction("Detail", new { id = orderDetailViewModel.Order.Id });
        }

        #region

        [HttpGet]
        public async Task<IActionResult> GetOrderList(string state)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<Order> all;

            if(User.IsInRole(SD.Role_Admin))  // get roll
            {
                all = await _unitWork.OrderRepository.GetAll(includeProperties: "ApplicationUser");

            }
            else
            {
                all = await _unitWork.OrderRepository.GetAll(x => x.ApplicationUserId == claim.Value,  includeProperties: "ApplicationUser");

            }
            //validate state
            switch(state)
            {
                case "approved":
                    all = all.Where(x => x.StateOrder == SD.StateOrderApproved);
                    break;
                case "completed":
                    all = all.Where(x => x.StateOrder == SD.StateOrderSend); 
                    break;
                default: 
                    break;
            }

            return Json(new {data = all});
        }

        #endregion

    }
}
