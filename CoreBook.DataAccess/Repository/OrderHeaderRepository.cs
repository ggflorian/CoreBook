using CoreBook.DataAccess.Data;
using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.Update(obj);
        }
    }
}
