using System;
using System.ComponentModel.DataAnnotations;

namespace EntityG.EntityFramework.Entities
{
    public class Asset : AuditableEntity
    {
        [Required]
        public string AssetName { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public AssetType AssetType { get; set; }
        [Required]
        public int AssetTypeId { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public decimal PurchasePrice { get; set; }
        public Employee UsedBy { get; set; }
        public int? UsedById { get; set; }
    }
}
