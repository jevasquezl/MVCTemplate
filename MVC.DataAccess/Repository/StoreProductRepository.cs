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
    public class StoreProductRepository : Repository<StoreProduct>, IStoreProductRepository
    {
        private readonly ApplicationDbContext _db;

        public StoreProductRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(StoreProduct storeProduct)
        {
           var storeProductDB =  _db.StoreProduct.FirstOrDefault(b => b.Id == storeProduct.Id);
            if (storeProductDB != null)
            {
                storeProductDB.Quantity = storeProduct.Quantity;
                _db.SaveChanges();
            }
        }
    }
}
