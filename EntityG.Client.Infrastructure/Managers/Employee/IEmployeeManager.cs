using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Employees;
using EntityG.Contracts.Responses.Employees;

namespace EntityG.Client.Infrastructure.Managers.Employee
{
    public interface IEmployeeManager : IManager
    {
        Task<IResult<List<LookupDto>>> GetAllAsync();
        Task<PagingResult<EmployeeDto>> GetAllAsync(int page, int pageSize, string search);
        Task<IResult<EmployeeDto>> GetByIdAsync(int id);
        Task<IResult> CreateAsync(CreateEmployeeDto request);
        Task<IResult> UpdateAsync(UpdateEmployeeDto request);
    }
}
