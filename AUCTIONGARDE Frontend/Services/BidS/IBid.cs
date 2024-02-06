using AUCTIONGARDE_Frontend.Models.Bid;
using AUCTIONGARDE_Frontend.Models.Bid.Dtos;
using AUCTIONGARDE_Frontend.Models.User.Dtos;

namespace AUCTIONGARDE_Frontend.Services.BidS
{
    public interface IBid
    {
        Task<List<Bid>> GetBidsByUserId();
        Task<List<Bid>> GetBidsByArtId(Guid artId);
        Task<List<Bid>> GetMostRecentBidsByBidderId();
        Task<List<Bid>> GetAllBids();
        Task<List<Bid>> GetWonBids();
        Task<List<Bid>> GetWonBidsOfSellersArt(Guid sellerId);
        Task<ResponseDto> PlaceBid(AddBidDto newBid);
        Task<ResponseDto> UpdateBid(Bid bid);
        Task<bool> DeleteBid(Bid bid);
        Task<bool> DeleteAllBidsRelatedToArt(Guid artId);
    }
}
