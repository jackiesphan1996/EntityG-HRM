using System;

namespace EntityG.Client.Infrastructure.Routes
{
    public class LeaveEndPoint
    {
        public static string GetAll(int page, int pageSize, DateTime fromDate, DateTime toDate, bool? isApproved) 
            => $"api/v1/leave?page={page}&pageSize={pageSize}&fromDate={fromDate.ToString("MM-dd-yyyy")}&toDate={toDate.ToString("MM-dd-yyyy")}&isApproved={isApproved}";
    }
}
