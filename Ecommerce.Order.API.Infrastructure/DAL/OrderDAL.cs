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

        public async Task<IEnumerable<OrderModel>> GetAllOrders() => await _context.Orders.Include(x => x.Products).OrderBy(x => x.Id).ToListAsync();

        public async Task<OrderModel?> GetOrderById(int id) => await _context.Orders.Include(x => x.Products).OrderBy(x => x.Id).Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<bool> OrderExists(int id) => await _context.Orders.Where(x => x.Id == id).AnyAsync();

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
    }
}
