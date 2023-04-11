using Microsoft.EntityFrameworkCore;
using WebShopApi.Data;
using WebShopApi.Models.Repository;

namespace WebShopApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAccountsRepository, AccountRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddCors();
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));


            services.AddSwaggerGen();

            return services;
        }
    }
}
