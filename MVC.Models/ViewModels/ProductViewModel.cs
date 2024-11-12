using Microsoft.AspNetCore.Mvc.Rendering;
namespace MVC.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product? Product { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem>? BrandsList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem>? ParentList { get; set; } = new List<SelectListItem>();
    }
}
