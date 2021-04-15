using System.Collections.Generic;
using EntityG.Shared.Wrapper;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.AssetTypes;
using EntityG.Contracts.Responses.AssetTypes;

namespace EntityG.Client.Infrastructure.Managers.AssetType
{
    public interface IAssetTypeManager : IManager
    {
        Task<IResult<List<AssetTypeDto>>> GetAllAsync();
        Task<IResult> CreateAsync(CreateAssetTypeDto request);
        Task<IResult> UpdateAsync(UpdateAssetTypeDto request);
        Task<IResult> DeleteAsync(int id);
    }
}
