namespace EntityG.Client.Infrastructure.Routes
{
    public static class ProjectEndpoint
    {
        public static string GetAllWithPaging(int page, int pageSize, string search) => $"api/v1/project?page={page}&pageSize={pageSize}&search={search}";
        public static string GetAll = "api/v1/project/all";
        public static string GetById(int id) => $"api/v1/project/{id}";
        public static string Create = "api/v1/project";
        public static string Update = "api/v1/project";
    }
}
