using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models.ViewModels
{
    public class InventoryViewModel
    {
        public Inventory Inventory { get; set; }
        public InventoryDetail InventoryDetail { get; set; }
        public IEnumerable<InventoryDetail> InventoryDetails { get; set; }
        public IEnumerable<SelectListItem> StoreList { get; set; } = new List<SelectListItem>();
 
    }
}
