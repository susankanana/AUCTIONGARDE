using AuctionGardeMessageBus;
using AuthService.Models;
using AuthService.Models.Dtos;
using AuthService.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly ResponseDto _response;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserController(IUser usr, IConfiguration configuration, IMapper mapper)
        {
            _userService = usr;
            _response = new ResponseDto();
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ResponseDto>> RegisterUser(RegisterUserDto registerUserDto)
        {
            string role = string.IsNullOrEmpty(registerUserDto.Role) ? "Bidder" : registerUserDto.Role;
            var res = await _userService.RegisterUser(registerUserDto , role);
            if (string.IsNullOrEmpty(res))
            {
                //success
                _response.Result = "User Registered successfully";

                //add message to queue
                var message = new UserMessageDto()
                {
                    Name = registerUserDto.FName + registerUserDto.LName,
                    Email = registerUserDto.Email,
                };

                var mb = new MessageBus();
                await mb.PublishMessage(message, _configuration.GetValue<string>("ServiceBus:register"));

                return Created("", _response);
            }
            _response.ErrorMessage = res;
            _response.IsSuccess = false;

            return BadRequest(_response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseDto>> LoginUser(LoginRequestDto loginRequestDto)
        {
            var res = await _userService.LoginUser(loginRequestDto);
            if (res.User != null)
            {
                //success
                _response.Result = res;
                return Created("", _response);
            }
            _response.ErrorMessage = "Invalid Credentials";
            _response.IsSuccess = false;

            return BadRequest(_response);
        }
        [HttpPost("AssignRoles")]
        public async Task<ActionResult<ResponseDto>> AssignRole(AssignRoleDto assignRoleDto) //or AssignRoleDto will give you few columns to fill
        {
            var res = await _userService.AssignUserRoles(assignRoleDto.Email, assignRoleDto.Role);
            if (res)
            {
                _response.Result = res;
                return Ok(_response);
            }
            _response.ErrorMessage = "Error Ocurred";
            _response.Result = res;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDto>> GetUser(string Id)
        {
            var res = await _userService.GetUserById(Id);
            //var user = _mapper.Map<UserDto>(res); //to limit number of entries a user has
            if (res != null)
            {
                //this was success
                _response.Result = res;
                return Ok(_response);
            }

            _response.ErrorMessage = "User Not found ";
            _response.IsSuccess = false;
            return NotFound(_response);
        }


        [HttpGet]
        public async Task<ActionResult<ResponseDto>> getAllUsers()
        {
        var res = await _userService.getAllUsers();
        _response.Result = res;
         return Ok(_response);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<ResponseDto>> RemoveUser(string userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                //this was success
                _response.Result = "User Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            var res = await _userService.RemoveUser(user);
            _response.Result = res;
            return Ok(res);

        }
    }
}
