using Ecommerce.Order.API.Core.Context;
using Ecommerce.Order.API.Core.Infrastructure;
using Ecommerce.Order.API.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Order.API.Infrastructure.DAL
{
    public class OrderDAL : IOrderDAL
    {
        private readonly EcommerceDbContext _context;

        public OrderDAL(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(OrderModel order)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (order is null)
                    throw new ArgumentNullException(nameof(order));

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }

        public async Task<bool> InactiveOrder(OrderModel order)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (order is null)
                    throw new ArgumentNullException(nameof(order));

                order.Acitve = false;
                _context.Entry(order).State = EntityState.Modified;

                bool isSuccess = await _context.SaveChangesAsync() > 0;

                if (isSuccess)
                    transaction.Commit();
                else
                    transaction.Rollback();

                return isSuccess;
            }
            catch (Exception)
            {
                transaction.Rollback();

                return false;
            }
        }

        public async Task<OrderModel?> GetOrderById(int id) => await _context.Orders.Include(x => x.OrderDetails).Include("OrderDetails.Product").OrderBy(x => x.Id).Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<bool> OrderExists(int id) => await _context.Orders.Where(x => x.Id == id).AnyAsync();

        public async Task<bool> CreateOrderDetail(OrderDetailModel orderDetail)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (orderDetail is null)
                    throw new ArgumentNullException(nameof(orderDetail));

                await _context.OrderDetails.AddAsync(orderDetail);
                bool isSuccess = await _context.SaveChangesAsync() > 0;

                if (isSuccess)
                    transaction.Commit();
                else
                    transaction.Rollback();

                return isSuccess;
            }
            catch (Exception)
            {
                transaction.Rollback();

                return false;
            }
        }

        public async Task<bool> UpdateOrderDetail(OrderDetailModel orderDetail)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Entry(orderDetail).State = EntityState.Modified;

                bool isSuccess = await _context.SaveChangesAsync() > 0;

                if (isSuccess)
                    transaction.Commit();
                else
                    transaction.Rollback();

                return isSuccess;
            }
            catch (Exception)
            {
                transaction.Rollback();

                return false;
            }
        }

        public async Task<bool> DeleteOrderDetail(OrderDetailModel orderDetail)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.OrderDetails.Remove(orderDetail);
                bool isSuccess = await _context.SaveChangesAsync() > 0;

                if (isSuccess)
                    transaction.Commit();
                else
                    transaction.Rollback();

                return isSuccess;
            }
            catch (Exception)
            {
                transaction.Rollback();

                return false;
            }
        }

        public async Task<OrderDetailModel?> GetOrderDetailById(int id) => await _context.OrderDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
}
