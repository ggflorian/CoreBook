using CoreBook.DataAccess.Data;
using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var objFromDb = _db.Categories.FirstOrDefault(o => o.ID == category.ID);
            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;
                // _db.SaveChanges(); // added save in UnitOfWork (used in CategoryController-Upsert to be consistent)
            }
        }
    }
}
