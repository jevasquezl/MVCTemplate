using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public IEnumerable<SelectListItem>? GetAllDropDown(string obj)
        {
           if (obj == "Category")
            {
                return _db.Category.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Brand")
            {
                return _db.Brand.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Product")
            {
                return _db.Product.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Serie,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product product)
        {
           var productDB =  _db.Product.FirstOrDefault(b => b.Id == product.Id);
            if (productDB != null)
            {
                if(productDB.ImageUrl != null)
                {
                    productDB.ImageUrl = product.ImageUrl;
                }
                productDB.Serie = product.Serie;
                productDB.Description = product.Description;
                productDB.State = product.State;
                productDB.Price = product.Price;
                productDB.Cost = product.Cost;
                productDB.CategoryId = product.CategoryId;
                productDB.BrandId = product.BrandId;
                productDB.ParentId = product.ParentId;
                _db.SaveChanges();
            }
        }
    }
}
