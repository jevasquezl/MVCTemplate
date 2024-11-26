using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class UnitWork(ApplicationDbContext context) : IUnitWork
    {
        private readonly ApplicationDbContext _context = context;
        public IStoreRepository StoreRepository { get; private set; } = new StoreRepository(context);
        public ICategoryRepository CategoryRepository { get; private set; } = new CategoryRepository(context);
        public IBrandRepository BrandRepository { get; private set; } = new BrandRepository(context);
        public IProductRepository ProductRepository { get; private set; } = new ProductRepository(context);
        public IApplicationUserRepository ApplicationUserRepository { get; private set; } = new ApplicationUserRepository(context);
        public IStoreProductRepository StoreProductRepository { get; private set; } = new StoreProductRepository(context);
        public IInventoryRepository InventoryRepository { get; private set; } = new InventoryRepository(context);
        public IInventoryDetailRepository InventoryDetailRepository { get; private set; } = new InventoryDetailRepository(context);
        public IKardexRepository KardexRepository { get; private set; } = new KardexRepository(context);
        
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
