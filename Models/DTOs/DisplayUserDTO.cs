using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Models.DTOs
{
    public class DisplayUserDTO
    {

        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}
