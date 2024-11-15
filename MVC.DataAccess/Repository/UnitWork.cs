using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class UnitWork : IUnitWork
    {
        private readonly ApplicationDbContext _context;
        public IStoreRepository StoreRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IBrandRepository BrandRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }

        public IApplicationUserRepository ApplicationUserRepository { get; private set; }

        public UnitWork(ApplicationDbContext context)
        {
            _context = context;
            StoreRepository = new StoreRepository(context);
            CategoryRepository = new CategoryRepository(context);
            BrandRepository = new BrandRepository(context);
            ProductRepository = new ProductRepository(context);
            ApplicationUserRepository = new ApplicationUserRepository(context);

        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
