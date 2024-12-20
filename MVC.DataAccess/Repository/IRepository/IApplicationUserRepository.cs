﻿using MVC.Models;
using MVC.Models.Especifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository :IRepository<ApplicationUser>
    {
       Task<ApplicationUser> GetEntity(string id);

        void Update(ApplicationUser user);
    }
}
