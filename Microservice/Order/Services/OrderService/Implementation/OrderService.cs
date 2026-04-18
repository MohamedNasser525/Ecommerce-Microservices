using OrderService.Dto;
using OrderService.Dto.Request;
using OrderService.Dto.Update;
using OrderService.Repository.OrderRepo;

namespace OrderService.Services.OrderService.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;

        public OrderService(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<OrderDto> AddOrder(OrderRequestDto orderDto)
        {
            var order = new Models.Order
            {
                Id = orderDto.Id,
                CustomerId = orderDto.CustomerId,
                OrderedOn = orderDto.OrderedOn,
                OrderDetails = orderDto.OrderDetails.Select(detail => new Models.OrderDetail
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            };

            var result = await _orderRepo.AddOrder(order);
            return MapToDto(result);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepo.GetAllOrders();
            return orders.Select(MapToDto);
        }

        public async Task<OrderDto> GetOrderById(int orderId)
        {
            var order = await _orderRepo.GetOrderById(orderId);
            return MapToDto(order);
        }

        public async Task<OrderDto> UpdateOrder(OrderUpdateDto orderDto)
        {
            var updatedOrder = await _orderRepo.UpdateOrder(orderDto);
            return MapToDto(updatedOrder);
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            return await _orderRepo.DeleteOrder(orderId);
        }

        private static OrderDto MapToDto(Models.Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderedOn = order.OrderedOn,
                CreatedAt = order.CreatedAt,
                OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDto
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            };
        }
    }
}
