using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Caching.Interfaces.Proxies
{
    public interface IEmployeeProxy
    {
        Task<IResult<List<LookupDto>>> GetAllAsync();
    }
}
