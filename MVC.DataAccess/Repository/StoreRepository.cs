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
    internal class StoreRepository : Repository<Store>, IStoreRepository
    {
        private readonly ApplicationDbContext _db;

        internal StoreRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(Store store)
        {
           var storeDB =  _db.Store.FirstOrDefault(b => b.Id == store.Id);
            if (storeDB != null)
            {
                storeDB.Name = store.Name;
                storeDB.Description = store.Description;
                storeDB.State = store.State;
                _db.SaveChanges();
            }
        }
    }
}
