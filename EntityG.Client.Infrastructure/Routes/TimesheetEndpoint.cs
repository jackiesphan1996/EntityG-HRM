namespace EntityG.Client.Infrastructure.Routes
{
    public static class EmployeeEndpoint
    {
        public static string GetAllWithPaging(int page, int pageSize, string search) => $"api/v1/employee?page={page}&pageSize={pageSize}&search={search}";
        public static string GetAll = "api/v1/employee/lookup";
        public static string GetById(int id) => $"api/v1/employee/{id}";
        public static string Create = "api/v1/employee";
        public static string Update = "api/v1/employee";

    }
}
