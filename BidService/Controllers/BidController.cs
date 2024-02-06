using AutoMapper;
using BidService.Models;
using BidService.Models.Dtos;
using BidService.Services.Iservices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BidService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBid _bidService;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        private readonly IArt _artService;
        public BidController(IBid bidService, IMapper mapper, IArt artService)
        {

            _bidService = bidService;
            _mapper = mapper;
            _response = new ResponseDto();
            _artService = artService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> PlaceBid(AddBidDto newBid)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            //does the art exists
            var art = await _artService.GetArtById(newBid.ArtId, token);
            if (art == null)
            {
                _response.ErrorMessage = "The art you are trying to bid on does not exist !";
                return NotFound(_response);
            }
            if (DateTime.Now > art.ExpiryTime)
            {
                return BadRequest("Bidding is closed for this art.");
            }

            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized";
                return StatusCode(403, _response);
            }

            var bid = _mapper.Map<Bid>(newBid);
            bid.BidderId = new Guid(userId);
            bid.ExpiryTime = art.ExpiryTime;
            bid.Status = "True";
            if(bid.BidAmount >= art.StartPrice)
            {
                var res = await _bidService.AddBid(bid);
                _response.Result = res;
                return Created("", _response);
            }
            _response.ErrorMessage = "Bid amount must be higher than start price!";
            return BadRequest(_response);

        }
        [HttpPost("update/bidIds")]
        public async Task<ActionResult<ResponseDto>> UpdateBidStatus(List<string> artIds){
            Console.WriteLine("I reached here");
            var res = await _bidService.UpdateBidStatus(artIds);
            _response.Result = res;
            return Ok(res);
        }


        [HttpGet("Bidder")]
        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> GetBidsByBidderId()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized";
                return StatusCode(403, _response);
            }

            var res = await _bidService.GetBidsByBidderId(new Guid(userId));
            _response.Result = res;
            return Ok(_response);

        }
        [HttpGet("Bidder/MostRecent")]
        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> GetMostRecentBidsByBidderId()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized";
                return StatusCode(403, _response);
            }

            var res = await _bidService.GetMostRecentBidsByBidderId(new Guid(userId));
            _response.Result = res;
            return Ok(_response);

        }

        [HttpGet]
        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> GetAllBids()
        {

            var res = await _bidService.GetAllBids();
            _response.Result = res;
            return Ok(_response);

        }

        [HttpGet("Expired")]
        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> GetExpiredBids()
        {

            var res = await _bidService.GetExpiredBids();

            _response.Result = res;
            return Ok(_response);

        }

        [HttpGet("Won")]
        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> GetWonBids()
        {

            var res = await _bidService.GetWonBids();

            _response.Result = res;
            return Ok(_response);

        }

        [HttpGet("Won/Seller{sellerId}")]
        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> GetWonBidsOfSellersArt(Guid sellerId)
        {

            var res = await _bidService.GetWonBidsOfSellersArt(sellerId);

            _response.Result = res;
            return Ok(_response);

        }


        [HttpGet("{artId}")]
        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> GetBidsByArtId(Guid artId)
        {

            var res = await _bidService.GetBidsByArtId(artId);
            _response.Result = res;
            return Ok(_response);

        }

        [HttpGet("BidId/{Id}")]
        [Authorize]

        public async Task<ActionResult<ResponseDto>> GetBid(Guid Id)
        {

            var bid = await _bidService.GetBidById(Id);
            if (bid == null)
            {
                _response.ErrorMessage = "Bid Not Found";
                return NotFound(_response);
            }
            _response.Result = bid;
            return Ok(_response);

        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> DeleteBid([FromBody] Bid bid)
        {

            var res = await _bidService.DeleteBid(bid);
            _response.Result = res;
            return Ok(_response);

        }

        [HttpDelete("{artId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> DeleteAllBids(Guid artId)
        {
            
            var res = await _bidService.DeleteAllBids(artId);
            _response.Result = res;
            return Ok(res);

        }
        [HttpPut]

        [Authorize(Roles = "Admin, Bidder, Seller")]
        public async Task<ActionResult<ResponseDto>> Updatebid([FromBody] Bid bid)
        {
            var res = await _bidService.UpdateBid(bid);
            _response.Result = res;
            return Ok(_response);

        }
    }
}
