using EasyCaching.Core;
using EntityG.BusinessLogic.Caching.Constants;
using EntityG.BusinessLogic.Caching.Proxies.Interfaces;
using EntityG.BusinessLogic.Interfaces.Services;
using EntityG.Contracts.Responses.AssetTypes;
using EntityG.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Caching.Proxies.Implements
{
    public class AssetTypeProxy : IAssetTypeProxy
    {
        private readonly IAssetTypeService _assetTypeService;
        private readonly IEasyCachingProviderFactory _factory;
        private readonly IEasyCachingProvider _provider;

        public AssetTypeProxy(IAssetTypeService assetTypeService, IEasyCachingProviderFactory factory)
        {
            _assetTypeService = assetTypeService ?? throw new ArgumentNullException(nameof(assetTypeService));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _provider = factory.GetCachingProvider(nameof(EntityG));
        }

        public async Task<IResult<List<AssetTypeDto>>> GetAllAsync()
        {
            var cachingData = _provider.Get<IResult<List<AssetTypeDto>>>(CachingKey.ASSET_TYPES);

            if (cachingData.HasValue)
            {
                return cachingData.Value;
            }

            IResult<List<AssetTypeDto>> assetTypes = await _assetTypeService.GetAllAsync();
            _provider.Set(CachingKey.ASSET_TYPES, assetTypes, TimeSpan.FromMinutes(5));

            return assetTypes;
        }
    }
}
