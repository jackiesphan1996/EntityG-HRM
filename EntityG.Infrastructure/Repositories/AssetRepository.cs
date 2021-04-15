using EntityG.Domain.Entities;
using EntityG.Infrastructure.Contexts;

namespace EntityG.Infrastructure.Repositories
{
    public interface IAssetRepository : IRepositoryAsync<Asset>
    {

    }
    public class AssetRepository : RepositoryAsync<Asset>, IAssetRepository
    {
        public AssetRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
