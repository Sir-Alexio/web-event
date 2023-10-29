using System.ComponentModel.DataAnnotations;

namespace WebEvent.API.Model.Entity
{
    public class Parameter
    {
        [Key]
        public int ParameterId {  get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public int EventId{ get; set; }
        public virtual Event Event{ get; set; }
    }
}
