using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int InventoryCount { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public string Design { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
