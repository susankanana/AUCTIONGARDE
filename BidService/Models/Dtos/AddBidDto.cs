namespace BidService.Models.Dtos
{
    public class AddBidDto
    {
        public int BidAmount { get; set; }
        public int HighestBid { get; set; }
        public Guid ArtId { get; set; }
        public string State { get; set; }
    }
}
