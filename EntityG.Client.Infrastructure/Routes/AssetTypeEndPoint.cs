namespace EntityG.Client.Infrastructure.Routes
{
    public static class AssetTypeEndPoint
    {
        public static string GetAll = "api/v1/assettype";
        public static string Create = "api/v1/assettype";
        public static string Update = "api/v1/assettype";
        public static string Delete(int id) =>  $"api/v1/assettype/{id}";
    }
}
