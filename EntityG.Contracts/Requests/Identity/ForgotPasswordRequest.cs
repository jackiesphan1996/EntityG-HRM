using System.ComponentModel.DataAnnotations;

namespace EntityG.Contracts.Requests.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}