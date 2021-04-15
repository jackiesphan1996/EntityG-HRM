using System;

namespace EntityG.EntityFramework.Entities
{
    public class Timesheet : AuditableEntity
    {
        public int ProjectId { get; set; }
        public decimal Hours { get; set; }
        public HourRate HourRate { get; set; }
        public Activity Activity { get; set; }
        public string Comment { get; set; }
        public virtual Project Project { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public enum HourRate
    {
         NormalWorkingDays,
         OverTimeInWorkingDays,
         WorkAtWeekends,
         WorkAtPublicHolidays
    }

    public enum Activity
    {
        Code,
        Test,
        Estimate,
        Design,
        Manage,
        Meeting,
        Other
    }
}
