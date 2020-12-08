using CoreBook.DataAccess.Data;
using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            _db.Update(company);
        }
    }
}
