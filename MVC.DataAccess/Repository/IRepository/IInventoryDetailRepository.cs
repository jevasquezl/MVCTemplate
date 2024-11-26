using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository.IRepository
{
    public interface IInventoryDetailRepository : IRepository<InventoryDetail>
    {
        void Update(InventoryDetail inventoryDetail);
    }
}
