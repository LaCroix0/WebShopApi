using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }
        public int SerialNumber { get; set; }
        public string Producer { get; set; }
        public string PublicId { get; set; }
        public int AccountId { get; set; }
        public Account Account{ get; set; }

    }
}
