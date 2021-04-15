using System;
using System.ComponentModel.DataAnnotations;

namespace EntityG.Contracts.Requests.Assets
{
    public class CreateAssetDto
    {
        [Required]
        [Display(Name = "Asset Name")]
        public string AssetName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }
        [Required]
        [Display(Name = "Asset Type")]
        public int AssetTypeId { get; set; }
        [Required]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }
        [Required]
        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }
        [Display(Name = "UsedBy")]
        public int? UsedById { get; set; }
    }
}
