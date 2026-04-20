namespace OrderService.Dto.Update
{
    public class OrderDetailUpdateDto
    {
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
