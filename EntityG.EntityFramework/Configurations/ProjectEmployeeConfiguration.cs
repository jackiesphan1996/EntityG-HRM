using EntityG.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityG.EntityFramework.Configurations
{
    public class ProjectEmployeeConfiguration : IEntityTypeConfiguration<ProjectEmployee>
    {
        public void Configure(EntityTypeBuilder<ProjectEmployee> builder)
        {
            builder.ToTable(nameof(ProjectEmployee));
            builder.HasOne(x => x.Employee).WithMany(x => x.ProjectEmployees).HasForeignKey(x => x.EmployeeId);
            builder.HasOne(x => x.Project).WithMany(x => x.ProjectEmployees).HasForeignKey(x => x.ProjectId);
        }
    }
}
