using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EntityG.EntityFramework.Configuration.MySql
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection RegisterMySqlDbContext(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            throw new NotSupportedException();
        }
    }
}