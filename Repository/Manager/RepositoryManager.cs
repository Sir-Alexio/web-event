using WebEvent.API.Context;
using WebEvent.API.Repository.Abstract;

namespace WebEvent.API.Repository.Manager
{
    public class RepositoryManager:IRepositoryManager
    {
        private readonly ApplicationContext _db;
        private IUserRepository _userRepository;
        private IEventRepository _eventRepository;
        public RepositoryManager(ApplicationContext db)
        {
            _db = db;
        }

        public IUserRepository Users
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

        public IEventRepository Events
        {
            get
            {
                if (_eventRepository == null)
                {
                    _eventRepository = new EventRepository(_db) ;
                }
                return _eventRepository;
            }

        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
