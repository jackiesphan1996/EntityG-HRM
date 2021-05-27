using EntityG.Contracts.Requests.Timesheets;
using EntityG.Contracts.Responses.Timesheets;
using EntityG.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Interfaces.Services
{
    public interface ITimesheetService
    {
        Task<PagingResult<TimesheetDto>> GetAllAsync(int employeeId, int pageIndex, int pageSize, string search);
        Task<IResult<List<TimesheetDto>>> GetAllAsync(int employeeId, int year, int month);
        Task<IResult<List<TimesheetDto>>> GetAllAsync(int employeeId, DateTime fromDate, DateTime toDate);
        Task<int> CreateAsync(CreateTimesheetRequest request);
        Task<int> UpdateAsync(UpdateTimesheetRequest request);
        Task<int> DeleteAsync(int id);
    }
}
