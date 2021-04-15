using EntityG.Contracts.Requests.AssetTypes;
using EntityG.Client.Infrastructure.Extensions;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EntityG.Contracts.Responses.AssetTypes;

namespace EntityG.Client.Infrastructure.Managers.AssetType
{
    public class AssetTypeManager : IAssetTypeManager
    {
        private readonly HttpClient _httpClient;

        public AssetTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<List<AssetTypeDto>>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(Routes.AssetTypeEndPoint.GetAll);
            return await response.ToResult<List<AssetTypeDto>>();
        }

        public async Task<IResult> CreateAsync(CreateAssetTypeDto request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(Routes.AssetTypeEndPoint.Create, request);
            return await response.ToResult<string>();
        }
        public async Task<IResult> UpdateAsync(UpdateAssetTypeDto request)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(Routes.AssetTypeEndPoint.Update, request);
            return await response.ToResult<string>();
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(Routes.AssetTypeEndPoint.Delete(id));
            return await response.ToResult<string>();
        }
    }
}
