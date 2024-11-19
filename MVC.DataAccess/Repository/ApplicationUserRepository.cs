using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ApplicationUser> GetEntity(string id)
        {
            var userDB = await _db.ApplicationUser.FindAsync(id);

            if (userDB != null)
            {
                return userDB;
            }
            return null;
        }

        public void Update(ApplicationUser user)
        {
            var userDB = _db.ApplicationUser.FirstOrDefault(b => b.Id == user.Id);
            if (userDB != null)
            {
                userDB.Email = user.Email;
                userDB.Names = user.Names;

                _db.SaveChanges();
            }
        }

    }
}
