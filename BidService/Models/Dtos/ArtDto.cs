namespace BidService.Models.Dtos
{
    public class ArtDto
    {
        public Guid ArtId { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime ExpiryTime { get; set; }
        public string Status { get; set; } = string.Empty;

        public int StartPrice { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string ArtImage { get; set; }
        public Guid SellerId { get; set; }

        public string Category { get; set; }
    }
}
