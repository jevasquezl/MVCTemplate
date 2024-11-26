using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository.IRepository
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        void Update(Inventory inventory);

        IEnumerable<SelectListItem> GetAllDropDown(string obj);
    }
}
