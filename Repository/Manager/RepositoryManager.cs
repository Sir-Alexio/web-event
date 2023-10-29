using WebEvent.API.Context;
using WebEvent.API.Repository.Abstract;

namespace WebEvent.API.Repository.Manager
{
    public class RepositoryManager:IRepositoryManager
    {
        private readonly ApplicationContext _db;
        private IUserRepository _userRepository;
        public RepositoryManager(ApplicationContext db)
        {
            _db = db;
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_db);
                }
                return _userRepository;
            }

        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
