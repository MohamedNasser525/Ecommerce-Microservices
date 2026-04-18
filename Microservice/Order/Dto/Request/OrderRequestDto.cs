namespace OrderService.Dto.Request
{
    public class OrderRequestDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderedOn { get; set; } = DateTime.UtcNow;
        public List<OrderDetailRequestDto> OrderDetails { get; set; } = new();
    }
}
