using ArtService.Models;

namespace ArtService.Services.IServices
{
    public interface IArt
    {
        Task<List<Art>> GetArtsBySellerId(Guid sellerId);
        Task<Art> GetArtById(Guid Id);
        Task<string> AddArt(Art art);
        Task<string> DeleteArt(Art art);
        Task<string> SaveChanges();
        Task<string>UpdateArt(Art art);
        Task<List<Art>> GetAllArtsStatusTrue(string status);
        Task<List<Art>> GetAllArts();
        Task<string> UpdateArtHighestBid(Guid artId, int highestBid);
    }
}
