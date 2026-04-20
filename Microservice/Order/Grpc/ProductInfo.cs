namespace OrderService.Grpc
{
    public class ProductInfo
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Quantity { get; init; }
    }
}
