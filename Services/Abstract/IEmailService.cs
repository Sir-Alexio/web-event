namespace WebEvent.API.Services.Abstract
{
    public interface IEmailService
    {
        public Task SendVerificationEmailAsync(string body);
    }
}