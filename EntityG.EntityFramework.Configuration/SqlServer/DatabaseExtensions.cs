using EntityG.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EntityG.EntityFramework.SqlServer.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection RegisterSqlServerDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
               .AddEntityFrameworkSqlServer()
               .AddDbContextPool<ApplicationDbContext>(options => options
                   .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                       sqlOptions =>
                       {
                           sqlOptions.EnableRetryOnFailure(
                               maxRetryCount: 5,
                               maxRetryDelay: TimeSpan.FromSeconds(15),
                               errorNumbersToAdd: null);
                           sqlOptions.MigrationsAssembly("EntityG.EntityFramework.SqlServer");
                       }), 5000);

            return services;
        }
       
    }
}
