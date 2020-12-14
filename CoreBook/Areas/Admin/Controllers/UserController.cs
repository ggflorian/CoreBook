using CoreBook.DataAccess.Data;
using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;
using CoreBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext  db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }


        // WEB METHODS - API CALLS
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _db.ApplicationUsers.Include(u => u.Company).ToList();
            var roles = _db.Roles.ToList();
            var user_role = _db.UserRoles.ToList();

            foreach (var user in users)
            {
                var roleId = user_role.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;

                if (user.Company == null)
                    user.Company = new Company() { Name = "" };
            }

            return Json(new { data = users });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
                return Json(new { scc = false, msj = "Error while Locking/Unlocking!" });

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                // user is currently locked, will unlock him
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else objFromDb.LockoutEnd = DateTime.Now.AddYears(160);

            _db.SaveChanges();

            return Json(new { scc = true, msj = "Operation Successful!" });
        }

        #endregion
    }
}
