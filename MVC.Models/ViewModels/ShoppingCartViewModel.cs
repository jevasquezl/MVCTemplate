using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public Company Company { get; set; }
        public Product Product { get; set; }
        public int Stock {  get; set; }
        public ShoppingCart ShoppingCart { get; set; }

    }
}
