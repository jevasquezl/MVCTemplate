using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class Kardex
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StoreProductId { get; set; }

        [ForeignKey("StoreProductId")]
        public StoreProduct StoreProduct { get; set; }
        
 
        [Required]
        [MaxLength(100)]
        public string Type { get; set; }
        [Required]
        public string Detail { get; set; }
        [Required]
        public int LastStock { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double CostUnit { get; set; }
        [Required]
        public double Total {  get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime RegisterDate { get; set; }
       
    }
}
