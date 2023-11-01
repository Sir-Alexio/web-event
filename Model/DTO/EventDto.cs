using System.ComponentModel.DataAnnotations;
using WebEvent.API.Model.Entity;

namespace WebEvent.API.Model.DTO
{
    public class EventDto
    {
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Parameter>? Parameters { get; set; }

        public virtual ICollection<User>? RegistedUsers { get; set; }
    }
}
