namespace ProductService.Dto.Request
{
    public class ProductRequestDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Guid CategoryId { get; set; }
    }
}
