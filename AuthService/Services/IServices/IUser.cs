using AuthService.Models;
using AuthService.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Services.IServices
{
    public interface IUser
    {
        Task<string> RegisterUser(RegisterUserDto userDto);
        Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto);
        Task<bool> AssignUserRoles(string Email, string RoleName);
        Task<ApplicationUser> GetUserById(string Id);
        Task<List<ApplicationUser>> getAllUsers();
    }
}
