using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using WebEvent.API.Model.DTO;
using WebEvent.API.Model.Entity;
using WebEvent.API.Services.Abstract;
using WebEvent.API.Services.Logger.Abstract;

namespace WebEvent.API.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authorizationService;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;


        private readonly IMapper _mapper;
        public AuthController(IAuthenticationService authorizationService, IMapper mapper, IUserService userService, ILoggerManager logger)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto dto)
        {
            bool isUserValid = await _authorizationService.ValidateUser(dto.Password, dto.Email);

            //user validation
            if (!isUserValid)
            {
                _logger.LogInfo($"Wrong password. User email = {dto.Email}, wrong password = {dto.Password}");
                return Unauthorized("Wrong password");
            }

            //create JWT token
            string token = await _authorizationService.CreateToken();

            //create refresh token
            RefreshToken refreshToken = _authorizationService.CreateRefreshToken();

            //set refresh token to responce header and to user database
            //await SetRefreshToken(refreshToken,dto.Email);

            //send to client jwt token
            return Ok(token);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            User currentUser = await _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);

            return Ok(JsonSerializer.Serialize(_mapper.Map<UserDto>(currentUser)));
        }

        private async Task SetRefreshToken(RefreshToken refreshToken, string email)
        {
            //set refresh token to database
            //await _userService.SetRefreshTokenToUser(refreshToken, email);

            //create cookie optons
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(7)
            };

            //set refresh token to response headers
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

        }
    }
}
