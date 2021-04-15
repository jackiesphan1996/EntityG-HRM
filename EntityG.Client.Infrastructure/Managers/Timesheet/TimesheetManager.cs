using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Timesheets;
using EntityG.Client.Infrastructure.Extensions;
using EntityG.Shared.Wrapper;
using EntityG.Contracts.Responses.Employees;
using EntityG.Contracts.Responses.Timesheets;

namespace EntityG.Client.Infrastructure.Managers.Timesheet
{
    public class TimesheetManager : ITimesheetManager
    {
        private readonly HttpClient _httpClient;

        public TimesheetManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagingResult<TimesheetDto>> GetAllAsync(int page, int pageSize, string search)
        {
            var response = await _httpClient.GetAsync(Routes.TimesheetEndpoint.GetAllWithPaging(page, pageSize, search));

            return await response.ToPagingResult<TimesheetDto>();
        }

        public Task<IResult<EmployeeDto>> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IResult> CreateAsync(CreateTimesheetRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TimesheetEndpoint.Create, request);

            return await response.ToResult();
        }

        public async Task<IResult> UpdateAsync(UpdateTimesheetRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.TimesheetEndpoint.Update, request);

            return await response.ToResult();
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.TimesheetEndpoint.Delete(id));

            return await response.ToResult();
        }

        public async Task<IResult<List<TimesheetDto>>> GetAllAsync(int year, int month)
        {
            var response = await _httpClient.GetAsync(Routes.TimesheetEndpoint.GetAllTimesheet(year, month));

            return await response.ToResult<List<TimesheetDto>>();
        }

        public async Task<IResult<List<TimesheetDto>>> GetAllAsync(DateTime fromDate, DateTime toDate)
        {
            var response = await _httpClient.GetAsync(Routes.TimesheetEndpoint.GetAllTimesheet(fromDate, toDate));

            return await response.ToResult<List<TimesheetDto>>();
        }
    }
}
