using EntityG.Contracts.Responses.Timesheets;
using EntityG.EntityFramework.Entities;

namespace EntityG.BusinessLogic.Mappers
{
    public static class TimesheetMapper
    {
        public static TimesheetDto Map(Timesheet timesheet)
        {
            return new TimesheetDto
            {
                Id = timesheet.Id,
                Activity = timesheet.Activity,
                Comment = timesheet.Comment,
                Date = timesheet.Date,
                EmployeeCode = timesheet.Employee.EmployeeIdNumber,
                FullName = timesheet.Employee.FullName,
                HourRate = timesheet.HourRate,
                Hours = timesheet.Hours,
                ProjectId = timesheet.ProjectId,
                ProjectManager = "",
                ProjectName = timesheet.Project.Name
            };
        }
    }
}
