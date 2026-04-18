namespace OrderService.Dto.Request
{
    public class OrderDetailRequestDto
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
