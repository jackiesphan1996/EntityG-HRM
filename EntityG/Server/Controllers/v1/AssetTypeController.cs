using EntityG.BusinessLogic.Services.Interfaces;
using EntityG.Contracts.Requests.AssetTypes;
using EntityG.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EntityG.Server.Controllers.v1
{
    public class AssetTypeController : BaseApiController<AssetTypeController>
    {
        private readonly IAssetTypeService _assetTypeService;

        public AssetTypeController(IAssetTypeService assetTypeService)
        {
            _assetTypeService = assetTypeService ?? throw new ArgumentNullException(nameof(assetTypeService));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _assetTypeService.GetAllAsync());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAssetType([FromBody] CreateAssetTypeDto request)
        {
            await _assetTypeService.CreateAsync(request);

            return Ok(await Result.SuccessAsync("Created successfully."));
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAssetType([FromBody] UpdateAssetTypeDto request)
        {
            await _assetTypeService.UpdateAsync(request);

            return Ok(await Result.SuccessAsync("Updated successfully."));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAssetType([FromRoute] int id)
        {
            await _assetTypeService.DeleteAsync(id);

            return Ok(await Result.SuccessAsync("Deleted successfully."));
        }
    }
}
