using EntityG.Contracts.Requests.Assets;
using EntityG.Contracts.Responses.Assets;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services.Interfaces
{
    public interface IAssetService
    {
        Task<IResult<List<AssetDto>>> GetAllAsync(string search);
        Task<int> CreateAsync(CreateAssetDto request);
        Task<int> UpdateAsync(UpdateAssetDto request);
        Task<int> DeleteAsync(int id);
    }
}
