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

        Task Save();
    }
}
