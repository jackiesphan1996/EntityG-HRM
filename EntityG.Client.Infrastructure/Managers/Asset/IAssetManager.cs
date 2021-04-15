using EntityG.Contracts.Requests.Assets;
using EntityG.Contracts.Responses.Assets;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.Client.Infrastructure.Managers.Asset
{
    public interface IAssetManager : IManager
    {
        Task<IResult<List<AssetDto>>> GetAllAsync();
        Task<IResult> CreateAsync(CreateAssetDto request);
        Task<IResult> UpdateAsync(UpdateAssetDto request);
        Task<IResult> DeleteAsync(int id);
    }
}
