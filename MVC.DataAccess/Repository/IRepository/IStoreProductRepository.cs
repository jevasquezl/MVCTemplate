
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.DataAccess.Repository.IRepository
{
    public interface IStoreProductRepository: IRepository<StoreProduct>
    {
        void Update(StoreProduct storeProduct);
    }
}
