using BidService.Models;

namespace BidService.Services.Iservices
{
    public interface IBid
    {
        Task<List<Bid>> GetBidsByBidderId(Guid bidderId);
        Task<List<Bid>> GetBidsByArtId(Guid artId);
        Task<Bid> GetBidById(Guid Id);
        Task<List<Bid>> GetAllBids();
        Task<List<Bid>> GetExpiredBids();
        Task<List<Bid>> GetWonBids();
        Task<string> AddBid(Bid bid);
        Task<string> DeleteBid(Bid bid);
        Task<string> UpdateBid(Bid bid);
        Task<string> UpdateBidStatus(List<string> artIds);
    }
}
