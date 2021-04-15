using System.Collections.Generic;
using System.Threading.Tasks;
using EntityG.Contracts.Responses.Projects;
using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;

namespace EntityG.Client.Infrastructure.Managers.Project
{
    public interface IProjectManager : IManager
    {
        Task<IResult<List<LookupDto>>> GetAllAsync();
        Task<PagingResult<ProjectDto>> GetAllAsync(int page, int pageSize, string search);
    }
}
