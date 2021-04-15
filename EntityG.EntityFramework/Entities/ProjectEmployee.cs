using System;

namespace EntityG.EntityFramework.Entities
{
    public class ProjectEmployee : AuditableEntity
    {
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual Project Project { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
