using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(Order order)
        {
           _db.Update(order);
        }

        public void UpdatePayStripeId(int id, string sessionId, string transactionId)
        {
            var orderBD = _db.Order.FirstOrDefault(x => x.Id == id);
            if (orderBD != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                { 
                    orderBD.SessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(transactionId)) 
                { 
                    orderBD.TransactionId = transactionId;
                    orderBD.DatePay = DateTime.Now;
                }
            }
        }

        public void UpdateState(int id, string orderState, string PayState)
        {
            var orderBD = _db.Order.FirstOrDefault(x => x.Id == id);
            if (orderBD != null) 
            { 
                orderBD.StateOrder = orderState;
                orderBD.StatePay = PayState;
            }
        }
    }
}
