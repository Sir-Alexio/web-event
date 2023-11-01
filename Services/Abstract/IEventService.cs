using WebEvent.API.Model.DTO;
using WebEvent.API.Model.Entity;

namespace WebEvent.API.Services.Abstract
{
    public interface IEventService
    {
        public Task<List<Event>> GetAllEvents();
        public List<Event> GetRegistratedEvents(int userId);
        public Task<bool> UpdateEvent(Event ev);
        public Task<Event> GetEventByName(string name);
    }
}