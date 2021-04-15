using EntityG.EntityFramework.Entities;
using System;

namespace EntityG.Contracts.Requests.Timesheets
{
    public class CreateTimesheetRequest
    {
        public int ProjectId { get; set; }
        public decimal Hours { get; set; } = 8;
        public HourRate HourRate { get; set; }
        public Activity Activity { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int EmployeeId { get; set; }
    }
}
