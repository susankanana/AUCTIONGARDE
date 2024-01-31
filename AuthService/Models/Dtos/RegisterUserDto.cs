using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string FName { get; set; } = string.Empty;
        [Required]
        public string LName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Role { get; set; } = "User";
    }
}
