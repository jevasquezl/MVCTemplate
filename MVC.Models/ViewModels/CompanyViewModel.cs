using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models.ViewModels
{
    public class CompanyViewModel
    {
        public Company Company { get; set; }
        public IEnumerable<SelectListItem> StoreList { get; set; }

    }
}
