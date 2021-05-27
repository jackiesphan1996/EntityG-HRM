using EntityG.Contracts.Requests.AssetTypes;
using EntityG.Contracts.Responses.AssetTypes;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Interfaces.Services
{
    public interface IAssetTypeService
    {
        Task<IResult<List<AssetTypeDto>>> GetAllAsync();
        Task<int> CreateAsync(CreateAssetTypeDto request);
        Task<int> UpdateAsync(UpdateAssetTypeDto request);
        Task<int> DeleteAsync(int id);
    }
}
