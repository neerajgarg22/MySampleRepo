using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CartService.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; } 
        public ICollection<OrderItem> Items { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }       
        public DateTime OrderDate  => DateTime.Now;
        public string ShippingCity { get; set; }
    }
}
