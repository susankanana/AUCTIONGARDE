using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AUCTIONGARDE_Frontend.Models.Arts
{
    public class Art
    {
        [Key]
        public Guid ArtId { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime ExpiryTime { get; set; }
        public string Status { get; set; } = string.Empty;

        public int StartPrice { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string ArtImage { get; set; }
        public Guid SellerId { get; set; }

        public int HighestBid { get; set; } = 0;
        public string Category { get; set; }

    }
}
