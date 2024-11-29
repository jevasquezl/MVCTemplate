using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository.IRepository
{
    public interface IUnitWork: IDisposable
    {
        IStoreRepository StoreRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IBrandRepository BrandRepository { get; }
        IProductRepository ProductRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        IStoreProductRepository StoreProductRepository { get; }
        IInventoryRepository InventoryRepository { get; }
        IInventoryDetailRepository InventoryDetailRepository { get; }
        IKardexRepository KardexRepository { get; }
        ICompanytRepository CompanytRepository { get; }

        IShoppingCartRepository ShoppingCartRepository { get; }

        IOrderRepository OrderRepository { get; }

        IOrderDetailRepository OrderDetailRepository { get; }
        Task Save();
    }
}
