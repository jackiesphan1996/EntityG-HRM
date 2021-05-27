using EntityG.Contracts.Requests.Department;
using EntityG.Contracts.Responses.Department;
using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<PagingResult<DepartmentDto>> GetAllAsync(int page, int pageSize, string search);
        Task<IResult<List<LookupDto>>> GetAllAsync();
        Task<int> CreateAsync(CreateDepartmentDto request);
        Task<int> UpdateAsync(UpdateDepartmentDto request);
        Task<int> DeleteAsync(int id);
    }
}
