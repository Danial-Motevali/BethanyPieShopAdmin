using BethPieShopAdmin.Models;

namespace BethPieShopAdmin.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderDetailsAsync(int? orderId);
        Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync();
    }
}
