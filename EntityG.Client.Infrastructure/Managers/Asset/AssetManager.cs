using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Assets;
using EntityG.Contracts.Responses;
using EntityG.Client.Infrastructure.Extensions;
using EntityG.Shared.Wrapper;
using EntityG.Contracts.Responses.Assets;

namespace EntityG.Client.Infrastructure.Managers.Asset
{
    public class AssetManager : IAssetManager
    {
        private readonly HttpClient _httpClient;

        public AssetManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<AssetDto>>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(Routes.AssetEndPoint.GetAll);
            return await response.ToResult<List<AssetDto>>();
        }

        public async Task<IResult> CreateAsync(CreateAssetDto request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(Routes.AssetEndPoint.Create, request);
            return await response.ToResult<string>();
        }

        public async Task<IResult> UpdateAsync(UpdateAssetDto request)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(Routes.AssetEndPoint.Update, request);
            return await response.ToResult<string>();
        }

        public Task<IResult> DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
