using System;

namespace EntityG.Contracts.Responses.Assets
{
    public class AssetDto
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int AssetTypeId { get; set; }
        public string AssetType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public int? UsedById { get; set; }
        public string UsedBy { get; set; }
    }
}
