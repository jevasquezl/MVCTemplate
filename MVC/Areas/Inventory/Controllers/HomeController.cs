using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Models.ErrorViewModels;
using MVC.Models.Especifications;
using System.Diagnostics;

namespace MVC.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitWork _unitWork;
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
        public IActionResult Index(int pageNumber = 1, string search="", string actualSearch="")
        {
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
