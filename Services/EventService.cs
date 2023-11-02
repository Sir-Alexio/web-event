using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebEvent.API.Model.DTO;
using WebEvent.API.Model.Entity;
using WebEvent.API.Model.Enums;
using WebEvent.API.Model.ErrorModel;
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
            return (await _repository.Events.GetAllEvents());
        }

        public async Task<Event> GetEventByName(string name)
        {
            return await _repository.Events.GetEventByName(name);
        }

        public List<Event> GetRegistratedEvents(int userId)
        {
            var events = _repository.Events.GetRegistedEvents(userId);
            return events;
        }

        public async Task<bool> UpdateEvent(Event ev)
        {
            //Get user from database
            Event currentEvent = await _repository.Events.GetEventByName(ev.EventName);

            if (currentEvent == null)
            {
                return false;
            }

            currentEvent.RegistedUsers = ev.RegistedUsers;

            try
            {
                //Update entity
                await _repository.Events.Update(currentEvent);

                //Try save changes to the database
                await _repository.Save();
            }
            catch (DbUpdateException)
            {

                throw new CustomException(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }
    }
}
