using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        private InventoryViewModel inventoryViewModel {  get; set; }

        public InventoryRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(Inventory inventory)
        {
           var inventoryDB =  _db.Inventory.FirstOrDefault(b => b.Id == inventory.Id);
            if (inventoryDB != null)
            {
                inventoryDB.StoreId = inventory.StoreId;
                inventoryDB.EndDate = inventory.EndDate;
                inventoryDB.State = inventory.State;
                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> GetAllDropDown(string obj)
        {
            if (obj == "Store")
            {
                return _db.Store.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }

    }
}
