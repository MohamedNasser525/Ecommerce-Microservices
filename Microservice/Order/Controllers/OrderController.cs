using Microsoft.AspNetCore.Mvc;
using OrderService.Dto.Request;
using OrderService.Dto.Update;
using OrderService.Services.OrderService;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("it work Order Service");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetId(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderById(orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto order)
        {
            var result = await _orderService.AddOrder(order);
            return CreatedAtAction(nameof(GetId), new { orderId = result.Id }, result);
        }

        [HttpPut("{orderId:int}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderUpdateDto order)
        {
            if (order.Id != 0 && order.Id != orderId)
            {
                return BadRequest(new { message = "Order id in body must match route id." });
            }

            order.Id = orderId;

            try
            {
                var result = await _orderService.UpdateOrder(order);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{orderId:int}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var deleted = await _orderService.DeleteOrder(orderId);
            if (!deleted)
            {
                return NotFound(new { message = "Order not found." });
            }

            return Ok("delete succeeded");
        }
    }
}
