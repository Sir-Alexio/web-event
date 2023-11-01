using Microsoft.EntityFrameworkCore;
using WebEvent.API.Context;
using WebEvent.API.Model.DTO;
using WebEvent.API.Model.Entity;
using WebEvent.API.Model.Enums;
using WebEvent.API.Model.ErrorModel;
using WebEvent.API.Repository.Abstract;
using WebEvent.API.Repository.Base;

namespace WebEvent.API.Repository
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(ApplicationContext db) : base(db)
        {
        }

        //ToDo
        public List<Event> GetRegistedEvents(int userId)
        {
            List<Event> events = _db.Events
            .Where(e => e.RegistedUsers.Any(u => u.UserId == userId))
            .ToList();

            return events;
        }

        public async Task<Event> GetEventByName(string name)
        {
            Event? ev = await GetByCondition(x => x.EventName == name, false).Result.FirstOrDefaultAsync();
            return ev;
        }
    }
}
