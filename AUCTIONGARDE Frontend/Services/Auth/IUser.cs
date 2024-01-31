using AUCTIONGARDE_Frontend.Models.User;

namespace AUCTIONGARDE_Frontend.Services.Auth
{
    public interface IUser
    {
       Task<User> GetUserById(string id);
       
    }
}
