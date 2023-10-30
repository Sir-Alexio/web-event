using Microsoft.EntityFrameworkCore;
using WebEvent.API.Context;
using WebEvent.API.Model.Entity;
using WebEvent.API.Repository.Abstract;
using WebEvent.API.Repository.Base;

namespace WebEvent.API.Repository
{
    public class UserRepository : RepositoryBase<User>,IUserRepository
    {
        public UserRepository(ApplicationContext db) : base(db)
        {
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            User? user = await base.GetByCondition(s => s.Email == email, false).Result.FirstOrDefaultAsync();
            return user;
        }

        public async Task<User?> GetUSerByToken(string token)
        {
            User? user = await base.GetByCondition(s => s.VerificationToken == token, false).Result.FirstOrDefaultAsync();
            return user;
        }
    }
}
