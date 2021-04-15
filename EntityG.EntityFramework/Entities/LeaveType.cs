using System.Collections.Generic;

namespace EntityG.EntityFramework.Entities
{
    public class LeaveType : AuditableEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Leave> Leaves { get; set; }
    }
}
