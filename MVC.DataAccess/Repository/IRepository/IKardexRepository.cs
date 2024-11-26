
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.DataAccess.Repository.IRepository
{
    public interface IKardexRepository : IRepository<Kardex>
    {
        Task CreateKardex(int storeProductId, string type, string detail, int lastStock, int quantity, string userId);

    }
}
