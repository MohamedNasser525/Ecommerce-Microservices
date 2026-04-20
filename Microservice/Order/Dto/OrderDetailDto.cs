namespace OrderService.Dto
{
    public class OrderDetailDto
    {
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
