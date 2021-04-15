using System.ComponentModel.DataAnnotations;

namespace EntityG.Contracts.Requests.AssetTypes
{
    public class CreateAssetTypeDto
    {
        [Required]
        [Display(Name = "Asset Type Name")]
        public string Name { get; set; }
        [Display(Name = "Asset Type Description")]
        public string Description { get; set; }
    }
}
