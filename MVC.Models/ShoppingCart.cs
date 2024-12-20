﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
 
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Quantity { get; set; }

        [NotMapped]
        public double Price { get; set; }

    }
}
