using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models.ViewModels
{
    public class KardexViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Kardex> KardexList { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime LastDate { get; set; }
 
    }
}
