using Microsoft.EntityFrameworkCore;
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
    public class KardexRepository : Repository<Kardex>, IKardexRepository
    {
        private readonly ApplicationDbContext _db;
        public KardexRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task CreateKardex(int storeProductId, string type, string detail, int lastStock, int quantity, string userId)
        {
            var storeProduc = await _db.StoreProduct.Include(b => b.Product).FirstOrDefaultAsync(b => b.Id == storeProductId);

            if(type == "Input")
            {
                Kardex kardex = new Kardex();
                kardex.StoreProductId = storeProductId;
                kardex.Type = type;
                kardex.Detail = detail;
                kardex.LastStock = lastStock;
                kardex.Quantity = quantity;
                kardex.CostUnit = storeProduc.Product.Cost;
                kardex.Stock = lastStock + quantity;
                kardex.Total = kardex.Stock * kardex.CostUnit;
                kardex.ApplicationUserId = userId;
                kardex.RegisterDate = DateTime.Now;

                await _db.Kardex.AddAsync(kardex);
                await _db.SaveChangesAsync();
            }
            if (type == "Output")
            {
                Kardex kardex = new Kardex();
                kardex.StoreProductId = storeProductId;
                kardex.Type = type;
                kardex.Detail = detail;
                kardex.LastStock = lastStock;
                kardex.Quantity = quantity;
                kardex.CostUnit = storeProduc.Product.Cost;
                kardex.Stock = lastStock - quantity;
                kardex.Total = kardex.Stock * kardex.CostUnit;
                kardex.ApplicationUserId = userId;
                kardex.RegisterDate = DateTime.Now;

                await _db.Kardex.AddAsync(kardex);
                await _db.SaveChangesAsync();
            }

        }
    }
}




        //public void Update(Kardex kardex)
        //{
        //   var KardexDB =  _db.Kardex.FirstOrDefault(b => b.Id == kardex.Id);
        //    if (KardexDB != null)
        //    {
        //        KardexDB.Type = kardex.Type;
        //        KardexDB.Detail = kardex.Detail;
        //        KardexDB.lastStock = kardex.lastStock;
        //        KardexDB.Quantity = Kardex.Quantity;
        //        KardexDB.UnitCost = kardex.UnitCost;
        //        KardexDB.RegisterDate = kardex.RegisterDate;
        //        _db.SaveChanges();
        //    }
        //}
