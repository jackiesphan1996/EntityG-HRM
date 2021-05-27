using EntityG.BusinessLogic.Caching.Proxies.Interfaces;
using EntityG.BusinessLogic.Interfaces.Services;
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
        private readonly IAssetTypeProxy _assetTypeProxy;

        public AssetTypeController(IAssetTypeService assetTypeService, IAssetTypeProxy assetTypeProxy)
        {
            _assetTypeService = assetTypeService ?? throw new ArgumentNullException(nameof(assetTypeService));
            _assetTypeProxy = assetTypeProxy ?? throw new ArgumentNullException(nameof(assetTypeProxy));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _assetTypeProxy.GetAllAsync());
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
