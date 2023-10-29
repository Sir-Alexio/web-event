using WebEvent.API.Model.Enums;

namespace WebEvent.API.Model.ErrorModel
{
    public class CustomException: Exception
    {
        public CustomException(string message) : base(message)
        {
        }
        public StatusCode StatusCode { get; set; }
    }
}
