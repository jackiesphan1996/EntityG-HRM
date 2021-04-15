using System.Collections.Generic;

namespace EntityG.EntityFramework.Entities
{
    public class Project : AuditableEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Timesheet> Timesheets { get; set; }
        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    }
}
