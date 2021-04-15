using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services.Interfaces
{
    public interface ILeaveTypeService
    {
        Task<IResult<List<LookupDto>>> GetAllAsync();
    }
}
