using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebEvent.API.Model.Entity
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public virtual ICollection<Event>? Events{ get; set; }

    }
}
