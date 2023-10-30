using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, ILoggerManager logger, IEmailService emailService, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _emailService = emailService;
            _mapper = mapper;
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
            userDto.VerificationToken = Guid.NewGuid().ToString();
            userDto.IsVerified = false;

            bool isUserRegistrated = await _userService.Registrate(userDto);

            if (!isUserRegistrated)
            {
                _logger.LogInfo($"Can not registrate user. User with email: {userDto.Email} is already exist.");
                return BadRequest("This email is alredy exist");
            }

            // Redirect to the "SendEmail" action in the "Account" controller.
            return RedirectToAction("SendEmail", "Account", new { verificationToken = userDto.VerificationToken });
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

        [Route("send-email")]
        [HttpGet]
        public async Task<IActionResult> SendEmail(string verificationToken)
        {

            // Generate the verification link using Url.Action.
            string verificationLink = Url.Action("VerifyEmail", "Account", new { token = verificationToken }, Request.Scheme);

            // Send the verification email to the user.
            await _emailService.SendVerificationEmailAsync(verificationLink);

            return Ok("User registered. Check your email for verification.");
        }


        [Route("verify-email")]
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            User? user = await _userService.GetUserByToken(token);

            if (user == null)
            {
                return Unauthorized("Incorrect verification token");
            }

            user.IsVerified = true;

            await _userService.VerificationComplite(_mapper.Map<UserDto>(user));

            return Ok(); // Return a response indicating the result of the verification process.
        }

    }
}
