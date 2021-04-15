using System.Collections.Generic;

namespace EntityG.Contracts.Responses.Identity
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}