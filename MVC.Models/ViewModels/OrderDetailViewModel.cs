using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public Company Company { get; set; }
        public Order Order { get; set; }
        public IEnumerable<OrderDetail> OrderDetailList { get; set; }
    }
}
