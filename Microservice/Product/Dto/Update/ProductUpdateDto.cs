namespace ProductService.Dto.Update
{
    public class ProductUpdateDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
