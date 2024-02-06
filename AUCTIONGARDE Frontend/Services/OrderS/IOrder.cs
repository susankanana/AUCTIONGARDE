using AUCTIONGARDE_Frontend.Models.Order;
using AUCTIONGARDE_Frontend.Models.Order.Dtos;
using AUCTIONGARDE_Frontend.Models.User.Dtos;

namespace AUCTIONGARDE_Frontend.Services.OrderS
{
    public interface IOrder
    {
        Task<ResponseDto> MakeOrder(AddOrderDto newOrder);
        
        Task<Order> GetOrderById(Guid orderId);
        Task<List<Order>> GetOrdersByUserId(Guid userId);
        Task<StripeRequestDto> MakePayments(StripeRequestDto stripe);
        Task<bool> ValidatePayment(Guid orderId);
    }
}
