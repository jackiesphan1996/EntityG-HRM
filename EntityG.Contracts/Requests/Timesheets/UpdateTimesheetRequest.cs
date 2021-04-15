using System;
using EntityG.EntityFramework.Entities;

namespace EntityG.Contracts.Requests.Timesheets
{
    public class UpdateTimesheetRequest
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectManager { get; set; }
        public string Inquiry { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public decimal Hours { get; set; } = 8;
        public HourRate HourRate { get; set; }
        public Activity Activity { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

        public string HourRateStr
        {
            get
            {
                switch (HourRate)
                {
                    case HourRate.NormalWorkingDays:
                        return "1x";
                    case HourRate.OverTimeInWorkingDays:
                        return "1.5x";
                    case HourRate.WorkAtPublicHolidays:
                        return "3x";
                    case HourRate.WorkAtWeekends:
                        return "2x";
                    default:
                        return "";
                }
            }
        }
    }
}
