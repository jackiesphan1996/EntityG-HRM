using EntityG.Contracts.Requests.Timesheets;
using EntityG.Contracts.Responses.Employees;
using EntityG.Contracts.Responses.Timesheets;
using EntityG.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.Client.Infrastructure.Managers.Timesheet
{
    public interface ITimesheetManager : IManager
    {
        Task<PagingResult<TimesheetDto>> GetAllAsync(int page, int pageSize, string search);
        Task<IResult<List<TimesheetDto>>> GetAllAsync(int year, int month);
        Task<IResult<List<TimesheetDto>>> GetAllAsync(DateTime fromDate, DateTime toDate);
        Task<IResult<EmployeeDto>> GetByIdAsync(int id);
        Task<IResult> CreateAsync(CreateTimesheetRequest request);
        Task<IResult> UpdateAsync(UpdateTimesheetRequest request);
        Task<IResult> DeleteAsync(int id);
    }
}
