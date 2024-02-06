using System.ComponentModel.DataAnnotations;

namespace BidService.Models
{
    public class Bid
    {
        [Key]
        public Guid BidId { get; set; }
        public int BidAmount { get; set; }
        public int HighestBid { get; set; }
        public Guid BidderId { get; set; }
        public Guid ArtId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
