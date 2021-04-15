using System.Collections.Generic;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Department;
using EntityG.Contracts.Responses.Department;
using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;

namespace EntityG.Client.Infrastructure.Managers.Department
{
    public interface IDepartmentManager : IManager
    {
        Task<IResult<List<LookupDto>>> GetAllAsync();
        Task<PagingResult<DepartmentDto>> GetAllAsync(int page, int pageSize, string search);
        Task<IResult> CreateAsync(CreateDepartmentDto request);
        Task<IResult> UpdateAsync(UpdateDepartmentDto request);
        Task<IResult> DeleteAsync(int id);
    }
}
