using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Department;
using EntityG.Contracts.Responses.Shared;
using EntityG.Client.Infrastructure.Extensions;
using EntityG.Shared.Wrapper;
using EntityG.Contracts.Responses.Department;

namespace EntityG.Client.Infrastructure.Managers.Department
{
    public class DepartmentManager : IDepartmentManager
    {
        private readonly HttpClient _httpClient;

        public DepartmentManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagingResult<DepartmentDto>> GetAllAsync(int page, int pageSize, string search)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(Routes.DepartmentEndpoint.GetAllWithPaging(page, pageSize, search));

            return await response.ToPagingResult<DepartmentDto>();
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(Routes.DepartmentEndpoint.GetAll);

            return await response.ToResult<List<LookupDto>>();
        }

        public async Task<IResult> CreateAsync(CreateDepartmentDto request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(Routes.DepartmentEndpoint.Create, request);

            return await response.ToResult();
        }

        public async Task<IResult> UpdateAsync(UpdateDepartmentDto request)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(Routes.DepartmentEndpoint.Update, request);

            return await response.ToResult();
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(Routes.DepartmentEndpoint.Delete(id));

            return await response.ToResult<string>();
        }
    }
}
