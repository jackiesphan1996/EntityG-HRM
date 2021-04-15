using System.Collections.Generic;

namespace Entity.IntegrationTest.Models
{
    public class AccountCreatePrarams
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public List<string> Roles { get; set; }

    }
}
