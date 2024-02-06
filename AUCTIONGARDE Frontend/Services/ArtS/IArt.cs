using AUCTIONGARDE_Frontend.Models.Arts;
using AUCTIONGARDE_Frontend.Models.Arts.Dtos;
using AUCTIONGARDE_Frontend.Models.User.Dtos;

namespace AUCTIONGARDE_Frontend.Services.ArtS
{
    public interface IArt
    {
        Task<List<Art>> GetArtsByUserId();
        Task<List<Art>> GetAllArtsStatusTrue();
        Task<ResponseDto> AddArt(AddArtDto art);
        Task<ResponseDto> EditArt(Art art);
        Task<List<Art>> GetAllArts();
        Task<bool> DeleteArt(Guid artId);
    }
}
