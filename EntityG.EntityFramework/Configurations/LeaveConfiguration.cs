using EntityG.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityG.EntityFramework.Configurations
{
    public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.ToTable(nameof(Leave));
            builder.HasOne(x => x.LeaveType).WithMany(x => x.Leaves).HasForeignKey(x => x.LeaveTypeId);
            builder.HasOne(x => x.Employee).WithMany(x => x.Leaves).HasForeignKey(x => x.EmployeeId);
        }
    }
}
