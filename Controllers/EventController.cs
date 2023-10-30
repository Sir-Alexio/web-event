using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using WebEvent.API.Model.Entity;
using WebEvent.API.Services.Abstract;

namespace WebEvent.API.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public EventController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }

        [HttpGet]
        [Route("all-events")]
        public async Task<IActionResult> GetAllEvents()
        {
            return Ok(JsonSerializer.Serialize(_eventService.GetAllEvents()));
        }

        [HttpPost]
        [Authorize]
        [Route("create-event")]
        public async Task<IActionResult> CreateEvent()
        {
            Event ev = new Event();
            Parameter param = new Parameter();

            param.Title = "first one";
            param.Value = "Second one";

            ev.EventName = "my first event";
            ev.Date = DateTime.Now;
            ev.Parameters = new List<Parameter>
            {
                param
            };

            User currentUser = await _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);

            if (currentUser.CreatedEvents == null)
            {
                currentUser.CreatedEvents = new List<Event>();
            }

            currentUser.CreatedEvents.Add(ev);

            await _userService.UpdateUser(currentUser);
            
            return Ok();
        }

    }
}
