using BethPieShopAdmin.Data;
using BethPieShopAdmin.Models;
using BethPieShopAdmin.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BethPieShopAdmin.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _bethanysPieShopDbContext;

        public OrderRepository(ApplicationDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync()
        {
            return await _bethanysPieShopDbContext.orders.Include(o => o.OrderDetails).ThenInclude(od => od.Pie).OrderBy(o => o.OrderId).AsNoTracking().ToListAsync();
        }

        public async Task<Order?> GetOrderDetailsAsync(int? orderId)
        {
            if (orderId != null)
            {
                var order = await _bethanysPieShopDbContext.orders.Include(o => o.OrderDetails).ThenInclude(od => od.Pie).OrderBy(o => o.OrderId).Where(o => o.OrderId == orderId.Value).AsNoTracking().FirstOrDefaultAsync();

                return order;
            }
            return null;
        }
    }
}
