using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Models.DTOs
{
    public class UserDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
