using AUCTIONGARDE_Frontend.Models.User;
using AUCTIONGARDE_Frontend.Models.User.Dtos;

namespace AUCTIONGARDE_Frontend.Services.Auth
{
    public interface IAuthRegister
    {
        Task<ResponseDto> Register(RegisterUser registerRequestDto);
    }
}
