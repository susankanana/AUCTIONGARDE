using BidService.Models;

namespace BidService.Services.Iservices
{
    public interface IBid
    {
        Task<List<Bid>> GetBidsByBidderId(Guid bidderId);
        Task<List<Bid>> GetMostRecentBidsByBidderId(Guid bidderId);
        Task<List<Bid>> GetBidsByArtId(Guid artId);
        Task<Bid> GetBidById(Guid Id);
        Task<List<Bid>> GetAllBids();
        Task<List<Bid>> GetExpiredBids();
        Task<List<Bid>> GetWonBids();
        Task<List<Bid>> GetWonBidsOfSellersArt(Guid sellerId);
        Task<Guid> GetSellerIdForArt(Guid artId);
        Task<string> AddBid(Bid bid);
        Task<string> DeleteBid(Bid bid);
        Task<string> UpdateBid(Bid bid);
        Task<string> DeleteAllBids(Guid artId);
        Task<string> UpdateBidStatus(List<string> artIds);
    }
}
