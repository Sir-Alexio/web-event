using WebEvent.API.Model.Entity;

namespace WebEvent.API.Services.Abstract
{
    public interface IAuthenticationService
    {
        public Task<bool> ValidateUser(string password, string email);
        public Task<string> CreateToken();
        public RefreshToken CreateRefreshToken();
    }
}
