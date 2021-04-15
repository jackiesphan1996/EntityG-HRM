using System.Collections.Generic;

namespace EntityG.Contracts.Responses.Identity
{
    public class GetAllUsersReponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}