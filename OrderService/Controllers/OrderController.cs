using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Models.Dtos;
using OrderService.Services.IServices;
using System.Security.Claims;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly IArt _artService;
        private readonly IBid _bidService;
        private readonly IOrder _orderService;
        private readonly IUser _userService;

        public OrderController(IOrder order, IArt artService, IBid bidService, IUser userService)
        {
            _orderService = order;
            _artService = artService;
            _bidService = bidService;
            _userService = userService;
            _response = new ResponseDto();
            

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> MakeOrder(AddOrderDto newOrder)
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized. Log in first.";
                return StatusCode(500, _response);
            }
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            
            var bid = await _bidService.GetBidById(newOrder.BidId, token);
            var art = await _artService.GetArtById(bid.ArtId, token);

            if (art == null || bid == null)
            {
                _response.ErrorMessage = "Art or bid not found.";
                return StatusCode(404, _response);
            }
            var user = await _userService.GetUserById(bid.BidderId.ToString());
            if (user == null)
            {
                _response.ErrorMessage = "User not found.";
                return NotFound(_response);
            }
            var order = new Order()
            {
                UserId = new Guid(userId),
                BidId = newOrder.BidId,
                ArtId = bid.ArtId,
                OrderTotal = bid.BidAmount,
                OrderName = art.Name,
                OrderDescription = art.Description,
                OrderOwner = $"{user.FName} {user.LName}"
            };
               
        
            var response = await _orderService.MakeOrder(order);
            _response.Result = response;
            return Ok(_response);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetOrderByUserId()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized. Log in first.";
                return StatusCode(500, _response);
            }
            var order = await _orderService.GetOrderByUserId(new Guid(userId));

            if (order == null)
            {
                _response.ErrorMessage = "You have not placed an order yet .";
                return NotFound(_response);
            }

            _response.Result = order;
            return Ok(_response);
        }

        [HttpPost("Pay")]
        [Authorize]

        public async Task<ActionResult<ResponseDto>> MakePayments(StripeRequestDto dto)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (token == null)
            {
                _response.ErrorMessage = "You are not authorized. Log in first";
                return StatusCode(500, _response);
            }

            var res = await _orderService.MakePayments(dto, token);

            _response.Result = res;
            return Ok(_response);
        }

        [HttpPost("validate/{orderId}")]
        [Authorize]

        public async Task<ActionResult<ResponseDto>> ValidatePayment(Guid orderId)
        {
           
            var res = await _orderService.ValidatePayments(orderId);

            if (res)
            {
                _response.Result = res;
                return Ok(_response);
            }

            _response.ErrorMessage = "Payment Failed!";
            return BadRequest(_response);
        }

    }
}
