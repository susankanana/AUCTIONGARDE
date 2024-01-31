namespace OrderService.Models.Dtos
{
    public class BidDto
    {
        public Guid BidId { get; set; }
        public int BidAmount { get; set; }
        public int HighestBid { get; set; }
        public Guid BidderId { get; set; }
        public Guid ArtId { get; set; }
        public string State { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
