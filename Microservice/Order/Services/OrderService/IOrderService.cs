using OrderService.Dto;
using OrderService.Dto.Request;
using OrderService.Dto.Update;

namespace OrderService.Services.OrderService
{
    public interface IOrderService
    {
        Task<OrderDto> AddOrder(OrderRequestDto orderDto);
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<OrderDto> GetOrderById(int orderId);
        Task<OrderDto> UpdateOrder(OrderUpdateDto orderDto);
        Task<bool> DeleteOrder(int orderId);
    }
}
