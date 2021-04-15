using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EntityG.Contracts.Responses.Identity
{
    public class RoleResponse
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public bool IsLoading { get; set; }

        [JsonIgnore]
        public PermissionResponse PermissionResponse { get; set; }
    }
}