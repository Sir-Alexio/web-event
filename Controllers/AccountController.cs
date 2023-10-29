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
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;

        public AccountController(IUserService userService, ILoggerManager logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Route("modify-user")]
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserDto dto)
        {
            //Update User
            bool isUserUpdated = await _userService.UpdateUser(dto);

            if (!isUserUpdated)
            {
                _logger.LogInfo($"Can not update user. User with email: {dto.Email} now found in database.");
                return BadRequest("No user found");
            }

            return Ok(JsonSerializer.Serialize(dto));
        }

        [Route("registration")]
        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            bool isUserRegistrated = await _userService.Registrate(userDto);

            if (!isUserRegistrated)
            {
                _logger.LogInfo($"Can not registrate user. User with email: {userDto.Email} is already exist.");
                return BadRequest("This email is alredy exist");
            }

            return Ok();
        }

        [Route("change-password")]
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword)
        {
            //Get email from claims
            string email = User.FindFirst(ClaimTypes.Email).Value;

            bool isPasswordChanged = await _userService.ChangePassword(changePassword, email);

            if (!isPasswordChanged)
            {
                _logger.Equals("Can not change password. Old passwort is incorrect.");
                return Unauthorized("Password was incorrect");
            }

            return Ok();
        }
    }
}
