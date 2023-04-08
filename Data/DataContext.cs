using Microsoft.EntityFrameworkCore;
using WebShopApi.Models;

namespace WebShopApi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Account> Users { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {

        }
    }
}
