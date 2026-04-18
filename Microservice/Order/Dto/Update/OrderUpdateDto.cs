namespace OrderService.Dto.Update
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderedOn { get; set; }
        public List<OrderDetailUpdateDto>? OrderDetails { get; set; }
    }
}
