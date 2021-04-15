using EntityG.Contracts.Responses.Identity;
using System.Collections.Generic;

namespace EntityG.Contracts.Requests.Identity
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }
        public IList<UserRoleModel> UserRoles { get; set; }
    }
}