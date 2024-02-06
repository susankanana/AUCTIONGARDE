using System.ComponentModel.DataAnnotations;

namespace AUCTIONGARDE_Frontend.Models.Order
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid ArtId { get; set; }
        public Guid BidId { get; set; }
        public int OrderTotal { get; set; }
        public string OrderName { get; set; } = string.Empty;
        public string OrderDescription { get; set; } = string.Empty;
        public string OrderOwner { get; set; } = string.Empty;
        public string? StripeSessionId { get; set; }

        public string Status { get; set; } = "Pending"; // is payment ongoing or was it successful

        public string PaymentIntent { get; set; } = string.Empty;
    }
}
