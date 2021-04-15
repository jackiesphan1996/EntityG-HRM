using EntityG.Contracts.Requests.AssetTypes;
using EntityG.Contracts.Responses.AssetTypes;
using EntityG.EntityFramework.Entities;

namespace EntityG.BusinessLogic.Mappers
{
    public static class AssetTypeMapper
    {
        public static AssetTypeDto Map(AssetType item)
        {
            return new AssetTypeDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CreatedOn = item.CreatedOn
            };
        }

        public static AssetType Map(CreateAssetTypeDto request)
        {
            return new AssetType
            {
                Name = request.Name,
                Description = request.Description
            };
        }
    }
}
