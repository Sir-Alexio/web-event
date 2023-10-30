using WebEvent.API.Model.Entity;

namespace WebEvent.API.Services.Abstract
{
    public interface IEventService
    {
        public Task<List<Event>> GetAllEvents();
    }
}