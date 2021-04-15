using EntityG.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntityG.EntityFramework.Configuration.InMemory
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection RegisterInMemoryDbContext(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(nameof(ApplicationDbContext)));

            return services;
        }
    }
}