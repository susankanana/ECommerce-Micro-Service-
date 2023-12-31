using AuthService.Models.Dtos;
using AuthService.Services.IServices;
using AutoMapper;
using EcommerceMessageBus;
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
        public UserController(IUser usr, IConfiguration configuration , IMapper mapper)
        {
            _userService = usr;
            _response = new ResponseDto();
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ResponseDto>> RegisterUser(RegisterUserDto registerUserDto)
        {
            var res = await _userService.RegisterUser(registerUserDto);
            if(string.IsNullOrEmpty(res))
            {
                //success
                _response.Result = "User Registered successfully";

                //add message to queue
                var message = new UserMessageDto()
                {
                    Name = registerUserDto.Name,
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
        public async Task<ActionResult<ResponseDto>> AssignRole(RegisterUserDto registerUserDto) //or AssignRoleDto will give you few columns to fill
        {
            var res = await _userService.AssignUserRoles(registerUserDto.Email, registerUserDto.Role);
            if (res)
            {
                _response.Result = res;
                return Ok(_response);
            }
            _response.ErrorMessage = "Error Ocurred";
            _response.Result = res;
            _response.IsSuccess= false;
            return BadRequest(_response);
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDto>> GetUser(string Id)
        {
            var res = await _userService.GetUserById(Id);
            var user = _mapper.Map<UserDto>(res); //to limit number of entries a user has
            if (res != null)
            {
                //this was success
                _response.Result = user;
                return Ok(_response);
            }

            _response.ErrorMessage = "User Not found ";
            _response.IsSuccess = false;
            return NotFound(_response);
        }

    }
}
