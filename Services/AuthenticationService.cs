using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebEvent.API.Model.Entity;
using WebEvent.API.Model.Enums;
using WebEvent.API.Model.ErrorModel;
using WebEvent.API.Repository.Abstract;
using WebEvent.API.Services.Logger.Abstract;
using WebEvent.API.Services.Abstract;

namespace WebEvent.API.Services
{
    public class AuthenticationService: Abstract.IAuthenticationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;

        private static User? _user;
        public AuthenticationService(IRepositoryManager repository, IConfiguration configuration, ILoggerManager logger)
        {
            _repository = repository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> ValidateUser(string password, string email)
        {
            _user = await _repository.Users.GetUserByEmail(email);

            if (_user == null)
            {
                _logger.LogError($"No user found with email: {email} in authentification service");
                throw new CustomException(message: "No user found") { StatusCode = StatusCode.DoesNotExist };
            }


            if (!VerifyPasswordHash(password, _user.Password, _user.PasswordSalt))
            {
                return false;
            }

            return true;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));

            SymmetricSecurityKey secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _user.Email)
            };

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("JwtSettings");

            JwtSecurityToken tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings.GetSection("validIssuer").Value,
            audience: jwtSettings.GetSection("validAudience").Value,
            claims: claims,
            expires:
            DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),

            signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
        public RefreshToken CreateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

    }
}
