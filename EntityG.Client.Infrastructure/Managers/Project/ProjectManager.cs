using EntityG.Contracts.Responses.Shared;
using EntityG.Client.Infrastructure.Extensions;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EntityG.Contracts.Responses.Projects;

namespace EntityG.Client.Infrastructure.Managers.Project
{
    public class ProjectManager : IProjectManager
    {
        private readonly HttpClient _httpClient;

        public ProjectManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProjectEndpoint.GetAll);

            return await response.ToResult<List<LookupDto>>();
        }

        public async Task<PagingResult<ProjectDto>> GetAllAsync(int page, int pageSize, string search)
        {
            var response = await _httpClient.GetAsync(Routes.ProjectEndpoint.GetAllWithPaging(page, pageSize, search));

            return await response.ToPagingResult<ProjectDto>();
        }
    }
}
