using WebEvent.API.Model.Entity;

namespace WebEvent.API.Repository.Abstract
{
    public interface IEventRepository:IRepositoryBase<Event>
    {
        public List<Event> GetRegistedEvents(int userId);
        public Task<Event> GetEventByName(string name);
        public Task<List<Event>> GetAllEvents();
    }
}
