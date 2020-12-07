using CoreBook.DataAccess.Data;
using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(o => o.ID == product.ID);
            if (objFromDb != null)
            {
                if (product.ImageUrl != null)
                    objFromDb.ImageUrl = product.ImageUrl;

                objFromDb.CategoryID = product.CategoryID;
                objFromDb.CoverTypeID = product.CoverTypeID;
                objFromDb.Title = product.Title;
                objFromDb.Description = product.Description;
                objFromDb.ISBN = product.ISBN;
                objFromDb.Authors = product.Authors;
                objFromDb.ListPrice = product.ListPrice;
                objFromDb.Price = product.Price;
                objFromDb.Price50 = product.Price50;
                objFromDb.Price100 = product.Price100;

                // _db.SaveChanges(); // added save in UnitOfWork (used in CategoryController-Upsert to be consistent)
            }
        }
    }
}
