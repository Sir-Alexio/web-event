using WebEvent.API.Model.DTO;
using WebEvent.API.Model.Entity;

namespace WebEvent.API.Services.Abstract
{
    public interface IUserService
    {
        public Task<bool> UpdateUser(UserDto dto);
        public Task<bool> Registrate(UserDto userDto);
        public Task<bool> ChangePassword(ChangePasswordModel changePassword, string email);
        public Task<User> GetUser(string email);
        public Task<User> GetUserByToken(string token);
        public Task<bool> VerificationComplite(UserDto dto);


    }
}