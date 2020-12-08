using CoreBook.DataAccess.Data;
using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBook.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
