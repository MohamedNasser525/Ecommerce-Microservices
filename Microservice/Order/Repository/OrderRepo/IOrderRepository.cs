using OrderService.Dto.Update;

namespace OrderService.Repository.OrderRepo
{
    public interface IOrderRepository
    {
        Task<Models.Order> AddOrder(Models.Order order);
        Task<IEnumerable<Models.Order>> GetAllOrders();
        Task<Models.Order> GetOrderById(int orderId);
        Task<Models.Order> UpdateOrder(OrderUpdateDto order);
        Task<bool> DeleteOrder(int orderId);
    }
}
