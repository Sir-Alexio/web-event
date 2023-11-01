using WebEvent.API.Model.Entity;

namespace WebEvent.API.Repository.Abstract
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        public Task<User?> GetUserByEmail(string email);
        public Task<User?> GetUSerByToken(string token);
        public Task<User?> GetUserWithEventsAndParameters(string email);
    }
}
