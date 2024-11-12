using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(100, ErrorMessage = "Maximo 100")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(100, ErrorMessage = "Maximo 100")]
        public string Description { get; set; } = "";
        [Required(ErrorMessage = "Requerido")]
        public bool State { get; set; }
    }
}
