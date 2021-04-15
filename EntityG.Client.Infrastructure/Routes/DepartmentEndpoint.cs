namespace EntityG.Client.Infrastructure.Routes
{
    public static class DepartmentEndpoint
    {
        public static string GetAllWithPaging(int page, int pageSize, string search) => $"api/v1/department?page={page}&pageSize={pageSize}&search={search}";
        public static string GetAll => "api/v1/department/lookup";
        public static string Create = "api/v1/department";
        public static string Update = "api/v1/department";
        public static string Delete(int id) => $"api/v1/department/{id}";
    }
}
