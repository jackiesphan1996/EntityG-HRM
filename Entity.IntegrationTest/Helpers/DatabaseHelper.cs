using EntityG.EntityFramework.Contexts;
using EntityG.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Entity.IntegrationTest.Helpers
{
    public static class DatabaseHelper
    {
        public static ApplicationDbContext CreateDbContext()
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(ConfigurationHelper.Configuration.GetConnectionString("DefaultConnection"));

            return new ApplicationDbContext(optionBuilder.Options);
        }
    }

    public class IntegrationTestUserService : ICurrentUserService
    {
        public string UserId => System.Environment.MachineName;
    }
}
