using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityG.EntityFramework.Entities
{
    public class AssetType : AuditableEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
    }
}
