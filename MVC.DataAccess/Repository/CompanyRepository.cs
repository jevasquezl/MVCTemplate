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
    public class CompanyRepository : Repository<Company>, ICompanytRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(Company company)
        {
           var companyDB =  _db.Company.FirstOrDefault(b => b.Id == company.Id);
            if (companyDB != null)
            {
                companyDB.Name = company.Name;
                companyDB.Description = company.Description;
                companyDB.Country = company.Country;
                companyDB.Address = company.Address;
                companyDB.StoreSaleId = company.StoreSaleId;
                companyDB.UpdatedId = company.UpdatedId;
                companyDB.UpdatedDate = company.UpdatedDate;

                _db.SaveChanges();
            }
        }
    }
}
