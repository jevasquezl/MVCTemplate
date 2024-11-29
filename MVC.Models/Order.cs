using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser AplicationUser { get; set; }

        public DateTime DateOrder { get; set; }
        public DateTime DeliveryOrder { get; set; }

        public string DeliveryNumber { get; set; }

        public string Carrier {  get; set; }

        public double TotalOrder { get; set; }

        public string StateOrder { get; set; }

        public string StatePay {  get; set; }

        public DateTime DatePay { get; set; }

        public DateTime DatePayLimit { get; set; }

        public string TransactionId { get; set; }

        public string Telephone {  get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string CustomerName { get; set; }

    }
}
