using EntityG.Contracts.Requests.Projects;
using EntityG.Contracts.Responses.Projects;
using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Interfaces.Services
{
    public interface IProjectService
    {
        Task<PagingResult<ProjectDto>> GetAllAsync(int page, int pageSize, string search);
        Task<IResult<List<LookupDto>>> GetAllAsync();
        Task<int> CreateAsync(CreateProjectRequest request);
    }
}
