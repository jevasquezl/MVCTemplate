using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitWork _unitWork;
        private readonly ApplicationDbContext _context;
        public UserController(IUnitWork unitWork, ApplicationDbContext context)
        {
            _unitWork = unitWork;
            _context = context;
        }

        // GET: UserController
        public IActionResult Index()
        {
            return View();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var userList = await _unitWork.ApplicationUserRepository.GetAll();
            IEnumerable<IdentityUserRole<string>> userRoles = await _context.UserRoles.ToListAsync();
            var roles = await _context.Roles.ToListAsync();

            foreach (var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                if (roleId != null)
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                }
            }
            return Json(new { data = userList });
        }

        #endregion
    }
}
