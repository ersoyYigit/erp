using System.ComponentModel.DataAnnotations;

namespace ArdaManager.Application.Requests.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}