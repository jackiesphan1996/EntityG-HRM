namespace EntityG.Client.Infrastructure.Routes
{
    public static class AssetEndPoint
    {
        public static string GetAll = "api/v1/asset";
        public static string Create = "api/v1/asset";
        public static string Update = "api/v1/asset";
        public static string Delete(int id) => $"api/v1/asset/{id}";
    }
}
