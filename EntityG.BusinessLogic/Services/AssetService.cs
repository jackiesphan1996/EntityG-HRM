using EntityG.BusinessLogic.Exceptions;
using EntityG.BusinessLogic.Interfaces.Services;
using EntityG.BusinessLogic.Mappers;
using EntityG.Contracts.Requests.Assets;
using EntityG.Contracts.Responses.Assets;
using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces;
using EntityG.EntityFramework.Interfaces.Repositories;
using EntityG.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssetService> _logger;

        public AssetService(IAssetRepository assetRepository, IUnitOfWork unitOfWork, ILogger<AssetService> logger)
        {
            _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IResult<List<AssetDto>>> GetAllAsync(string search)
        { 
            List<Asset> assets = await _assetRepository.GetAllAsync(filter: x => x.AssetName.Contains(search), 
                                                                    includes: y => y.Include(x => x.AssetType).Include(x => x.UsedBy));

            return await Result<List<AssetDto>>.SuccessAsync(assets.Select(AssetMapper.Map).ToList());
        }

        public async Task<int> CreateAsync(CreateAssetDto request)
        {
            Asset asset = AssetMapper.ToEntity(request);
            _assetRepository.Add(asset);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(UpdateAssetDto request)
        {
            var editAsset = await _assetRepository.GetByIdAsync(request.Id);

            if (editAsset == null)
            {
                throw new ValidationException($"ID : {request.Id} does not exist");
            }

            editAsset.AssetName = request.AssetName;
            editAsset.PurchasePrice = request.PurchasePrice;
            editAsset.PurchaseDate = request.PurchaseDate;
            editAsset.AssetTypeId = request.AssetTypeId;
            editAsset.UsedById = request.UsedById;
            editAsset.IsActive = request.IsActive;
            editAsset.Description = request.Description;
            _assetRepository.Update(editAsset);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var removedData = await _assetRepository.GetByIdAsync(id);

            if (removedData == null)
            {
                throw new ValidationException($"ID : {id} does not exist.");
            }

            _assetRepository.Remove(removedData);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
