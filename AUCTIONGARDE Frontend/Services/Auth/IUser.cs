using AUCTIONGARDE_Frontend.Models.User;

namespace AUCTIONGARDE_Frontend.Services.Auth
{
    public interface IUser
    {
       Task<User> GetUserById(string id);
       Task<List<User>> GetAllUsers();
       Task<bool> RemoveUser(string userId);
    }
}
