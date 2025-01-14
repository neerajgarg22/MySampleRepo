using System.ComponentModel.DataAnnotations;

namespace CartService.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public OrderDetails OrderDetails { get; set; }
        public decimal TotalPrice => Quantity * Price;
    }
}
