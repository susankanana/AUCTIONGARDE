using System.ComponentModel.DataAnnotations;

namespace AUCTIONGARDE_Frontend.Models.User
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string FName { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

    }
}
