using AUCTIONGARDE_Frontend.Models.Arts;

namespace AUCTIONGARDE_Frontend.Services.ArtS
{
    public interface IArt
    {
        Task<List<Art>> GetArtsByUserId();
        Task<List<Art>> GetAllArts();
    }
}
