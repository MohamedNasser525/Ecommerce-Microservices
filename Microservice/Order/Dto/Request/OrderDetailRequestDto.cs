namespace OrderService.Dto.Request
{
    public class OrderDetailRequestDto
    {
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
