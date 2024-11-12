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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(Category category)
        {
           var categoryDB =  _db.Category.FirstOrDefault(b => b.Id == category.Id);
            if (categoryDB != null)
            {
                categoryDB.Name = category.Name;
                categoryDB.Description = category.Description;
                categoryDB.State = category.State;
                _db.SaveChanges();
            }
        }
    }
}
