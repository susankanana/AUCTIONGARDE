using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.Services.IServices;
using OrderService.Data;
using OrderService.Models.Dtos;
using Stripe.Checkout;
using Stripe;
using AuctionGardeMessageBus;

namespace OrderService.Services
{
    public class OrdersService : IOrder
    {
        private readonly ApplicationDbContext _context;
        private readonly IArt _artService;
        private readonly IUser _userService;
        private readonly IMessageBus _messageBus;

        public OrdersService(ApplicationDbContext context, IArt artService, IUser userService, IMessageBus messageBus)
        {
            _context = context;
            _artService = artService;
            _userService = userService;
            _messageBus = messageBus;
        }
        public async Task<Order> GetOrderByUserId(Guid userId)
        {
            return await _context.Orders.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<string> MakeOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return "Order made successfully";
        }

        public async Task<StripeRequestDto> MakePayments(StripeRequestDto stripeRequestDto, string token)
        {
            var order = await _context.Orders.Where(x => x.OrderId == stripeRequestDto.OrderId).FirstOrDefaultAsync();
            var art = await _artService.GetArtById(order.ArtId , token);
            var options = new SessionCreateOptions()
            {
                SuccessUrl = stripeRequestDto.ApprovedUrl,
                CancelUrl = stripeRequestDto.CancelUrl,
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>()
            };



            var item = new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmount = (long)order.OrderTotal * 100,
                    Currency = "kes",

                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = art.Name,
                        Description = art.Description,
                        Images = new List<string> { art.ArtImage}
                    }
                },
                Quantity = 1


            };

            options.LineItems.Add(item);

            //service receives a good documentation of what we are paying for
            var service = new SessionService();
            Session session = service.Create(options); //an instance of a session that will come with more info that we need to open stripe.

            // info given back to us will be a URL and ID

            stripeRequestDto.StripeSessionUrl = session.Url;
            stripeRequestDto.StripeSessionId = session.Id;

            //update Database =>status/ SessionId 

            order.StripeSessionId = session.Id;
            order.Status = "Ongoing";
            await _context.SaveChangesAsync();

            return stripeRequestDto;
        }

        public async Task<bool> ValidatePayments(Guid OrderId)
        {
            var order = await _context.Orders.Where(x => x.OrderId == OrderId).FirstOrDefaultAsync();

            var service = new SessionService();
            Session session = service.Get(order.StripeSessionId); //select session we want to validate

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

            if (paymentIntent.Status == "succeeded")
            {
                //the payment was success

                order.Status = "Paid";
                order.PaymentIntent = paymentIntent.Id;  //can track the payment
                await _context.SaveChangesAsync();

                var user = await _userService.GetUserById(order.UserId.ToString());

                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    return false;
                }
                else
                {
                    var reward = new RewardsDto()
                    {
                        OrderId = order.OrderId,
                        OrderTotal = order.OrderTotal,
                        Name = user.FName + user.LName,
                        Email = user.Email

                    };

                    //send this reward to our topic
                    await _messageBus.PublishMessage(reward, "orderadded");
                }
                //Send an email to user
                return true;
            }
            return false;
        }
    }
}
