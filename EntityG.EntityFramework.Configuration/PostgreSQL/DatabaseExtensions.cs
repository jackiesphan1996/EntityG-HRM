using EntityG.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EntityG.EntityFramework.Configuration.PostgreSQL
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection RegisterNpgSqlDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
            => services
                .AddDbContext<ApplicationDbContext>(options => options
                    .UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(15),
                                errorCodesToAdd: null
                              );
                            sqlOptions.MigrationsAssembly("EntityG.EntityFramework.PostgreSQL");
                        }), ServiceLifetime.Scoped);
    }
}
