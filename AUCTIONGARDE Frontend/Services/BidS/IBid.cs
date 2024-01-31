using AUCTIONGARDE_Frontend.Models.Bid;

namespace AUCTIONGARDE_Frontend.Services.BidS
{
    public interface IBid
    {
        Task<List<Bid>> GetBidsByUserId();
        Task<List<Bid>> GetBidsByArtId(Guid artId);
        Task<List<Bid>> GetAllBids();
    }
}
