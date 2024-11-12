using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(20, ErrorMessage = "Maximo 100")]
        public string Serie { get; set; } = "";
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(100, ErrorMessage = "Maximo 100")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Requerido")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public double Cost { get; set; }

        public string ImageUrl { get; set; } = ""; 
        
        [Required(ErrorMessage = "Requerido")]
        public bool State { get; set; }


        public int? CategoryId { get; set; }
       
        [ForeignKey("CategoryId")]
        public  Category? Category { get; set; }


        public int? BrandId { get; set; }
      
        [ForeignKey("BrandId")]
        public  Brand? Brand { get; set; }

        public int? ParentId { get; set; }
        public virtual Product? Parent {  get; set; }
    }
}
