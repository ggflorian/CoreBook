using CoreBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBook.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        void Update(OrderDetails obj);
    }
}
