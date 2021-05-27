using EntityG.Contracts.Responses.Leaves;
using EntityG.Shared.Wrapper;
using System;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Interfaces.Services
{
    public interface ILeaveService
    {
        Task<PagingResult<LeaveDto>> GetAllLeaves(int page, int pageSize, int employeeId, DateTime fromDate, DateTime toDate, bool? isApproved);
    }
}
