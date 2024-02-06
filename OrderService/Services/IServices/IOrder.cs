using OrderService.Models;
using OrderService.Models.Dtos;

namespace OrderService.Services.IServices
{
    public interface IOrder
    {
        Task<string> MakeOrder(Order order);
        Task<Order> GetOrderById(Guid orderId);
        Task<List<Order>> GetOrderByUserId(Guid userId);
        Task<StripeRequestDto> MakePayments(StripeRequestDto stripeRequestDto, string token);
        Task<bool> ValidatePayments(Guid OrderId);
    }
}
