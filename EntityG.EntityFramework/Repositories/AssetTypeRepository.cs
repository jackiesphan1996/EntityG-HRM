using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EntityG.EntityFramework.Repositories
{
    public class AssetTypeRepository : BaseRepository<AssetType>, IAssetTypeRepository
    {
        public AssetTypeRepository(DbContext context) : base(context)
        {
        
        }
    }
}
