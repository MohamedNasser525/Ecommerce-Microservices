using OrderService.Dto;
using OrderService.Dto.Request;
using OrderService.Dto.Update;
using OrderService.Grpc;
using OrderService.Repository.OrderRepo;

namespace OrderService.Services.OrderService.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductGrpcClient _productGrpcClient;

        public OrderService(IOrderRepository orderRepo, IProductGrpcClient productGrpcClient)
        {
            _orderRepo = orderRepo;
            _productGrpcClient = productGrpcClient;
        }

        public async Task<OrderDto> AddOrder(OrderRequestDto orderDto)
        {
            var normalizedOrderDetails = await ValidateAndNormalizeOrderDetailsAsync(orderDto.OrderDetails);

            var order = new Models.Order
            {
                Id = orderDto.Id,
                CustomerId = orderDto.CustomerId,
                OrderedOn = orderDto.OrderedOn,
                OrderDetails = normalizedOrderDetails.Select(detail => new Models.OrderDetail
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
            if (orderDto.OrderDetails != null)
            {
                orderDto.OrderDetails = await ValidateAndNormalizeOrderDetailsAsync(orderDto.OrderDetails);
            }

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

        private async Task<List<OrderDetailRequestDto>> ValidateAndNormalizeOrderDetailsAsync(IEnumerable<OrderDetailRequestDto> orderDetails)
        {
            var normalizedDetails = new List<OrderDetailRequestDto>();

            foreach (var detail in orderDetails)
            {
                var product = await _productGrpcClient.GetProductByIdAsync(detail.ProductId);
                if (detail.Quantity > product.Quantity)
                {
                    throw new Exception($"Requested quantity for product '{product.Name}' exceeds available stock.");
                }

                normalizedDetails.Add(new OrderDetailRequestDto
                {
                    ProductId = product.Id,
                    Quantity = detail.Quantity,
                    UnitPrice = product.Price
                });
            }

            return normalizedDetails;
        }

        private async Task<List<OrderDetailUpdateDto>> ValidateAndNormalizeOrderDetailsAsync(IEnumerable<OrderDetailUpdateDto> orderDetails)
        {
            var normalizedDetails = new List<OrderDetailUpdateDto>();

            foreach (var detail in orderDetails)
            {
                var product = await _productGrpcClient.GetProductByIdAsync(detail.ProductId);
                if (detail.Quantity > product.Quantity)
                {
                    throw new Exception($"Requested quantity for product '{product.Name}' exceeds available stock.");
                }

                normalizedDetails.Add(new OrderDetailUpdateDto
                {
                    ProductId = product.Id,
                    Quantity = detail.Quantity,
                    UnitPrice = product.Price
                });
            }

            return normalizedDetails;
        }
    }
}
