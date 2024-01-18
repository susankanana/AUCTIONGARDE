using System.ComponentModel.DataAnnotations;

namespace AUCTIONGARDE_Frontend.Models.User
{
    public class RegisterUser
    {
        [Required]
        public string FName { get; set; } = string.Empty;
        [Required]
        public string LName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; }=string.Empty;
        [Required]
        public string PhoneNumber { get; set; }= string.Empty;
        public string? Role { get; set; } = "User";
    }
}
