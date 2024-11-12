using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _db;

        public BrandRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(Brand brand)
        {
           var brandDB =  _db.Brand.FirstOrDefault(b => b.Id == brand.Id);
            if (brandDB != null)
            {
                brandDB.Name = brand.Name;
                brandDB.Description = brand.Description;
                brandDB.State = brand.State;
                _db.SaveChanges();
            }
        }
    }
}
