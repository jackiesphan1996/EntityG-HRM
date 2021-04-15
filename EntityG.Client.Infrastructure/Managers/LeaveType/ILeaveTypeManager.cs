using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.Client.Infrastructure.Managers.LeaveType
{
    public interface ILeaveTypeManager : IManager
    {
        Task<IResult<List<LookupDto>>> GetAllAsync();
    }
}
