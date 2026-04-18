using MongoDB.Driver;
using OrderService.Dto.Update;
using OrderService.Helper;

namespace OrderService.Repository.OrderRepo.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Models.Order> _orderCollection;

        public OrderRepository(MongoContext mongoContext)
        {
            _orderCollection = mongoContext.Orders;
        }

        public async Task<Models.Order> AddOrder(Models.Order order)
        {
            await _orderCollection.InsertOneAsync(order);
            return order;
        }

        public async Task<IEnumerable<Models.Order>> GetAllOrders()
        {
            return await _orderCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Models.Order> GetOrderById(int orderId)
        {
            var order = await _orderCollection.Find(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order == null)
            {
                throw new Exception("The Order Not Found");
            }

            return order;
        }

        public async Task<Models.Order> UpdateOrder(OrderUpdateDto order)
        {
            var existedOrder = await _orderCollection.Find(o => o.Id == order.Id).FirstOrDefaultAsync();
            if (existedOrder == null)
            {
                throw new Exception("The Order Not Found");
            }

            if (order.CustomerId.HasValue)
                existedOrder.CustomerId = order.CustomerId.Value;

            if (order.OrderedOn.HasValue)
                existedOrder.OrderedOn = order.OrderedOn.Value;

            if (order.OrderDetails != null)
            {
                existedOrder.OrderDetails = order.OrderDetails.Select(detail => new Models.OrderDetail
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList();
            }

            existedOrder.UpdatedAt = DateTime.UtcNow;
            await _orderCollection.ReplaceOneAsync(o => o.Id == order.Id, existedOrder);
            return existedOrder;
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var result = await _orderCollection.DeleteOneAsync(o => o.Id == orderId);
            return result.DeletedCount > 0;
        }
    }
}
