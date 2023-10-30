using Microsoft.Identity.Client;
using WebEvent.API.Model.Entity;
using WebEvent.API.Repository.Abstract;
using WebEvent.API.Services.Abstract;

namespace WebEvent.API.Services
{
    public class EventService:IEventService
    {
        private readonly IRepositoryManager _repository;
        public EventService(IRepositoryManager repository)
        {
            _repository= repository;
        }
        public async Task<List<Event>> GetAllEvents()
        {
            return (await _repository.Events.GetAll(false)).ToList();
        }
    }
}
