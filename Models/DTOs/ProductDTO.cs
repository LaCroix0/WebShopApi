using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Models.DTO
{
    public class ProductDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [MaxLength(200)]
        public string Category { get; set; }
        [Required]
        public int SerialNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string Producer { get; set; }
    }
}
