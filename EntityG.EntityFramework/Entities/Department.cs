using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityG.EntityFramework.Entities
{
    public class Department : AuditableEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
