using System;

namespace EntityG.Contracts.Responses.AssetTypes
{
    public class AssetTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
