using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string email { get; set; }
    }
}
