using EntityG.Contracts.Responses.Shared;
using EntityG.Client.Infrastructure.Extensions;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Employees;
using EntityG.Contracts.Responses.Employees;

namespace EntityG.Client.Infrastructure.Managers.Employee
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly HttpClient _httpClient;

        public EmployeeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(Routes.EmployeeEndpoint.GetAll);
            return await response.ToResult<List<LookupDto>>();
        }

        public async Task<PagingResult<EmployeeDto>> GetAllAsync(int page, int pageSize, string search)
        {
            var response = await _httpClient.GetAsync(Routes.EmployeeEndpoint.GetAllWithPaging(page, pageSize, search));

            return await response.ToPagingResult<EmployeeDto>();
        }

        public async Task<IResult<EmployeeDto>> GetByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(Routes.EmployeeEndpoint.GetById(id));

            return await response.ToResult<EmployeeDto>();
        }

        public async Task<IResult> CreateAsync(CreateEmployeeDto request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.EmployeeEndpoint.Create, request);

            return await response.ToResult();
        }

        public async Task<IResult> UpdateAsync(UpdateEmployeeDto request)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.EmployeeEndpoint.Update, request);

            return await response.ToResult();
        }
    }
}
