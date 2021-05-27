using EntityG.Contracts.Responses.AssetTypes;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Caching.Proxies.Interfaces
{
    public interface IAssetTypeProxy
    {
        Task<IResult<List<AssetTypeDto>>> GetAllAsync();
    }
}
