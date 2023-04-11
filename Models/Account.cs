namespace WebShopApi.Models
{

    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

        public int GetAge(DateTime dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var age = today.Year - dateOfBirth.Year;

            return age;
        }
    }
}
