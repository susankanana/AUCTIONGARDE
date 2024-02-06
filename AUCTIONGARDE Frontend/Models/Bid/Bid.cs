using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AUCTIONGARDE_Frontend.Models.Bid
{
    public class Bid
    {
        [Key]
        public Guid bidId {  get; set; }
        public int bidAmount { get; set; }
        public int highestBid { get; set; }

        [ForeignKey("bidderId")]
        public Guid bidderId { get; set; }
        [ForeignKey("artId")]
        public Guid artId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime expiryTime { get; set; }


    }
}
