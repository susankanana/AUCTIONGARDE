namespace AUCTIONGARDE_Frontend.Models.User.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserResponseDto User { get; set; } = default!;
    }
}
