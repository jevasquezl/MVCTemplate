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
    public class InventoryDetailRepository : Repository<InventoryDetail>, IInventoryDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public InventoryDetailRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(InventoryDetail inventory)
        {
           var inventoryDB =  _db.InventoryDetail.FirstOrDefault(b => b.Id == inventory.Id);
            if (inventoryDB != null)
            {
                inventoryDB.LastStock = inventory.LastStock;
                inventoryDB.Quantity = inventory.Quantity;
                _db.SaveChanges();
            }
        }
    }
}
