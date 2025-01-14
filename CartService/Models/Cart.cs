using System.ComponentModel.DataAnnotations;

namespace CartService.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public ICollection<CartItem> Items { get; set; }
        [Required]
        public decimal TotalAmount { get; set; } 
    }
}
