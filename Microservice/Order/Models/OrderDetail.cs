using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class OrderDetail
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }
}
