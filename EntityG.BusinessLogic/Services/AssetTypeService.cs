using EntityG.BusinessLogic.Exceptions;
using EntityG.BusinessLogic.Interfaces.Services;
using EntityG.BusinessLogic.Mappers;
using EntityG.Contracts.Requests.AssetTypes;
using EntityG.Contracts.Responses.AssetTypes;
using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces;
using EntityG.EntityFramework.Interfaces.Repositories;
using EntityG.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services
{
    public class AssetTypeService : IAssetTypeService
    {
        private readonly IAssetTypeRepository _assetTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssetTypeService(IAssetTypeRepository assetTypeRepository, IUnitOfWork unitOfWork)
        {
            _assetTypeRepository = assetTypeRepository ?? throw new ArgumentNullException(nameof(assetTypeRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IResult<List<AssetTypeDto>>> GetAllAsync()
        {
            List<AssetType> assets = await _assetTypeRepository.GetAllAsync(orderBy: order => order.OrderByDescending(x => x.CreatedOn));

            return await Result<List<AssetTypeDto>>.SuccessAsync(assets.Select(AssetTypeMapper.Map).ToList());
        }

        public async Task<int> CreateAsync(CreateAssetTypeDto request)
        {
            AssetType assetType = AssetTypeMapper.Map(request) ?? throw new ArgumentNullException(nameof(request));
            if (await _assetTypeRepository.AnyAsync(x => x.Name.Equals(request.Name)))
            {
                throw new ValidationException($"{request.Name} already exist.");
            }
            _assetTypeRepository.Add(assetType);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(UpdateAssetTypeDto request)
        {
            var editAssetType = await _assetTypeRepository.GetByIdAsync(request.Id);
            var existAssetType = await _assetTypeRepository.FirstOrDefaultAsync(filter:x => x.Name.Equals(request.Name));
            if (editAssetType == null)
            {
                throw new ValidationException($"ID : {request.Id} does not exist");
            }

            if (existAssetType != null && existAssetType != null & editAssetType.Id != existAssetType.Id)
            {
                throw new ValidationException($"Name : {request.Name} already exist");
            }

            editAssetType.Name = request.Name;
            editAssetType.Description = request.Description;
            _assetTypeRepository.Update(editAssetType);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var removedData = await _assetTypeRepository.FirstOrDefaultAsync(
                filter: filter => filter.Id == id, 
                includes: include => include.Include(x => x.Assets));

            if (removedData == null)
            {
                throw new ValidationException($"ID : {id} does not exist.");
            }

            if (removedData.Assets.Any())
            {
                throw new ValidationException($"Can not deleted because some assets being used.");
            }
            _assetTypeRepository.Remove(removedData);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
