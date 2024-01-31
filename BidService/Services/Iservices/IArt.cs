using BidService.Models.Dtos;

namespace BidService.Services.Iservices
{
    public interface IArt
    {
        Task<ArtDto> GetArtById(Guid Id, string Token);
        Task<string> UpdateArtHighestBid(Guid artId, int highestBid);
    }
}
