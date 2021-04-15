using EntityG.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityG.EntityFramework.Configurations
{
    public class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
    {
        public void Configure(EntityTypeBuilder<Timesheet> builder)
        {
            builder.ToTable(nameof(Timesheet));
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Employee).WithMany(x => x.Timesheets).HasForeignKey(x => x.EmployeeId);
            builder.HasOne(x => x.Project).WithMany(x => x.Timesheets).HasForeignKey(x => x.ProjectId);
        }
    }
}
