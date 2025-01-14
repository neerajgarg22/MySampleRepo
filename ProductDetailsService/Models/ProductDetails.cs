using System.ComponentModel.DataAnnotations;

namespace ProductDetailsService.Models
{
    public class ProductDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public string Design { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
