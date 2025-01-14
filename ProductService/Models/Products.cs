using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int InventoryCount { get; set; }
    }
}
