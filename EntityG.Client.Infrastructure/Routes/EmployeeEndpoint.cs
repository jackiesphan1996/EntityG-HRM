using System;

namespace EntityG.Client.Infrastructure.Routes
{
    public static class TimesheetEndpoint
    {
        public static string GetAllWithPaging(int page, int pageSize, string search) => $"api/v1/timesheet?page={page}&pageSize={pageSize}&search={search}";
        public static string GetAllTimesheet(int year, int month) => $"api/v1/timesheet/{year}/{month}";
        public static string GetAllTimesheet(DateTime fromDate, DateTime toDate) => $"api/v1/timesheet/range/{fromDate.ToString("MM-dd-yyyy")}/{toDate.ToString("MM-dd-yyyy")}";
        public static string GetById(int id) => $"api/v1/timesheet/{id}";
        public static string Create = "api/v1/timesheet";
        public static string Update = "api/v1/timesheet";
        public static string Delete(int id) => $"api/v1/timesheet/{id}";
    }
}
