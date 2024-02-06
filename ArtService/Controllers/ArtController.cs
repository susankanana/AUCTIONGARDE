using ArtService.Models;
using ArtService.Models.Dtos;
using ArtService.Services;
using ArtService.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtController : ControllerBase
    {
        private readonly IArt _artService;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        private readonly IBid _bidService;
        public ArtController(IArt artService, IMapper mapper, IBid bidService)
        {
            _artService = artService;
            _mapper = mapper;
            _response = new ResponseDto();
            _bidService = bidService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller")]
        public async Task<ActionResult<ResponseDto>> AddArt(AddArtDto newArt)
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized";
                return StatusCode(403, _response);
            }
            var art = _mapper.Map<Art>(newArt);
            art.SellerId = new Guid(userId);
            art.Status = "True";
            var res = await _artService.AddArt(art);
            _response.Result = res;
            return Created("", _response);
        }

        [HttpPut("State")]
        public async Task<ActionResult<ResponseDto>> UpdateArtState()
        {
            var arts = await _artService.GetAllArtsStatusTrue("True");

            foreach (var art in arts)
            {
                if (art.ExpiryTime < DateTime.Now)
                {
                    art.Status = "False";  
                    await _artService.SaveChanges();
                }
            }

            _response.Result = "Art statuses updated successfully";
            return Ok(_response);
        }

        [HttpGet("Seller")]
        [Authorize(Roles = "Admin, Seller")]

        public async Task<ActionResult<ResponseDto>> GetArtsByUserId()
        {

            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized";
                return StatusCode(403, _response);
            }

            var art = await _artService.GetArtsBySellerId(new Guid(userId));
            _response.Result = art;
            return Ok(_response);

        }

        [HttpGet("{Id}")]
        [Authorize]

        public async Task<ActionResult<ResponseDto>> GetArt(Guid Id)
        {

            var art = await _artService.GetArtById(Id);
            if (art == null)
            {
                _response.ErrorMessage = "Art Not Found";
                return NotFound(_response);
            }
            _response.Result = art;
            return Ok(_response);

        }

        [HttpGet("True")]
        

        public async Task<ActionResult<ResponseDto>> GetAllArtsStatusTrue()
        {

            var arts = await _artService.GetAllArtsStatusTrue("True");
            var expiredArts = arts.Where(art=> art.ExpiryTime < DateTime.Now).ToList();
            if(expiredArts.Count > 0)
            {
                List<string> artIds = new List<string>();   
                foreach(var art in expiredArts)
                {
                    art.Status = "False";
                    artIds.Add(art.ArtId.ToString());
                }

                await _bidService.UpdateBidStatus(artIds);
                await _artService.SaveChanges();
            }

            
            _response.Result = arts;
            return Ok(_response);

        }

        [HttpGet]

        public async Task<ActionResult<ResponseDto>> GetAllArts()
        {
            var arts = await _artService.GetAllArts();
            _response.Result = arts;
            return Ok(_response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Seller")]
        public async Task<ActionResult<ResponseDto>> UpdateArt(Art art)
        {
            
            var res = await _artService.UpdateArt(art);
            _response.Result = res;
            return Ok(_response);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin, Seller")]
        public async Task<ActionResult<ResponseDto>> DeleteArt(Guid Id)
        {
            var art = await _artService.GetArtById(Id);
            if (art == null)
            {
                _response.Result = "Art Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            var res = await _artService.DeleteArt(art);
            _response.Result = res;
            return Ok(res);

        }

        [HttpGet("UpdateHighestBid/{artId}/{highestBid}")]
        public async Task<ActionResult<ResponseDto>> UpdateArtHighestBid(Guid artId, int highestBid)
        {
            Console.WriteLine("I reached here");
            var res = await _artService.UpdateArtHighestBid(artId,highestBid);
            _response.Result = res;
            return Ok(res);
        }
    }
}
