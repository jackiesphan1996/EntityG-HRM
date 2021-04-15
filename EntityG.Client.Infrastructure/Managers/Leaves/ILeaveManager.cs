using EntityG.Contracts.Responses.Leaves;
using EntityG.Shared.Wrapper;
using System;
using System.Threading.Tasks;

namespace EntityG.Client.Infrastructure.Managers.Leaves
{
    public interface ILeaveManager : IManager
    {
        Task<PagingResult<LeaveDto>> GetAllAsync(int page, int pageSize, DateTime fromDate, DateTime toDate, bool? isApproved);
    }
}
