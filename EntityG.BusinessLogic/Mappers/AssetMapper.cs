using EntityG.Contracts.Requests.Assets;
using EntityG.Contracts.Responses.Assets;
using EntityG.EntityFramework.Entities;

namespace EntityG.BusinessLogic.Mappers
{
    public static class AssetMapper
    {
        public static AssetDto Map(Asset item)
        {
            return new AssetDto
            {
                Id = item.Id,
                AssetName = item.AssetName,
                Description = item.Description,
                AssetTypeId = item.AssetTypeId,
                AssetType = item.AssetType.Name,
                IsActive = item.IsActive,
                PurchaseDate = item.PurchaseDate,
                PurchasePrice = item.PurchasePrice,
                UsedById = item.UsedById,
                UsedBy = item.UsedBy?.EmployeeIdNumber
            };
        }

        public static Asset ToEntity(CreateAssetDto item)
        {
            return new Asset
            {
                AssetName = item.AssetName,
                Description = item.Description,
                AssetTypeId = item.AssetTypeId,
                IsActive = item.IsActive,
                PurchaseDate = item.PurchaseDate,
                PurchasePrice = item.PurchasePrice,
                UsedById = item.UsedById
            };
        }
    }
}
