using EntityG.Contracts.Requests.Employees;
using EntityG.Contracts.Responses.Employees;
using EntityG.Contracts.Responses.Shared;
using EntityG.EntityFramework.Entities;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IResult<List<LookupDto>>> GetAllAsync();
        Task<PagingResult<EmployeeDto>> GetAllAsync(int pageIndex, int pageSize, string search);
        Task<IResult<EmployeeDto>> GetByIdAsync(int id);
        Task<Employee> GetByUserId(string id);
        Task<int> CreateAsync(CreateEmployeeDto request);
        Task<int> UpdateAsync(UpdateEmployeeDto request);
    }
}
