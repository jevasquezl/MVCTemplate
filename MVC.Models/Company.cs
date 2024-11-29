using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }

        public int StoreSaleId { get; set; }

        [ForeignKey("StoreSaleId")]
        public Store Store { get; set; }

        public string CreatedId { get; set; }

        [ForeignKey("CreatedId")]

        public ApplicationUser Created { get; set; }

        public string UpdatedId { get; set; }

        [ForeignKey("UpdatedId")]

        public ApplicationUser Updated { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
