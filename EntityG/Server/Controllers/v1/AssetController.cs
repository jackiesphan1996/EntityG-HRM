using EntityG.BusinessLogic.Services.Interfaces;
using EntityG.Contracts.Requests.Assets;
using EntityG.Shared.Wrapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EntityG.Server.Controllers.v1
{
    public class AssetController : BaseApiController<AssetController>
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService ?? throw new ArgumentNullException(nameof(assetService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string search = "")
        {
            return Ok(await _assetService.GetAllAsync(search));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAssetType([FromRoute] int id)
        {
            await _assetService.DeleteAsync(id);

            return Ok(await Result.SuccessAsync("Deleted successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetDto request)
        {
            await _assetService.CreateAsync(request);

            return Ok(await Result.SuccessAsync("Created successfully."));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsset([FromBody] UpdateAssetDto request)
        {
            await _assetService.UpdateAsync(request);

            return Ok(await Result.SuccessAsync("Updated successfully."));
        }
    }
}
