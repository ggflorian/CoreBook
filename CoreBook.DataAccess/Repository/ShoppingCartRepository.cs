using CoreBook.DataAccess.Data;
using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBook.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart obj)
        {
            _db.Update(obj);
        }
    }
}
