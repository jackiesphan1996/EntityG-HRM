using Microsoft.Extensions.Configuration;

namespace Entity.IntegrationTest.Helpers
{
    public static class ConfigurationHelper
    {
        public static IConfiguration Configuration => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public static string ServerUrl => Configuration["ServerUrl"];
    }
}
