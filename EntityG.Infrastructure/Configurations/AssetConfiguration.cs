using EntityG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityG.Infrastructure.Configurations
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable(nameof(Asset));
            builder.HasOne(x => x.AssetType).WithMany(x => x.Assets).HasForeignKey(x => x.AssetTypeId);
        }
    }
}