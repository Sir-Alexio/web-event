using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebEvent.API.Model.Entity
{
    [Index(nameof(EventName), IsUnique = true)]
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]
        public DateTime Date{ get; set; }
        public virtual ICollection<Parameter>? Parameters { get; set; }
        public virtual ICollection<User>? RegistedUsers { get; set; }

    }
}
