using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string password { get; set; }
        [Required]
        public string email { get; set; }
    }
}
