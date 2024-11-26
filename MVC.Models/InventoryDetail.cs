using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class InventoryDetail
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int InventoryId { get; set; }
        
        [ForeignKey("InventoryId")]
        public Inventory Inventory {  get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        
        [Required]
        public int LastStock {  get; set; }
        
        [Required]
        public int Quantity { get; set; }

    }
}
