using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

using WebEvent.API.Model.DTO;
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
        private readonly IMapper _mapper;

        public EventController(IEventService eventService, IUserService userService, IMapper mapper)
        {
            _eventService = eventService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("events")]
        public async Task<IActionResult> GetAllEvents()
        {
            List<Event> events = await _eventService.GetAllEvents();

            List<EventDto> dtos = _mapper.Map<List<EventDto>>(events);

            string json = JsonSerializer.Serialize(dtos);

            return Ok(json);
        }

        [HttpPost]
        [Authorize]
        [Route("create-event")]
        public async Task<IActionResult> CreateEvent(EventDto eventDto)
        {
            User currentUser = await _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);

            if (currentUser.CreatedEvents == null)
            {
                currentUser.CreatedEvents = new List<Event>();
            }

            var ev = _mapper.Map<EventDto, Event>(eventDto);

            currentUser.CreatedEvents.Add(ev);

            await _userService.UpdateUser(currentUser);
            
            return Ok();
        }

        [HttpGet]
        [Route("user-events")]
        [Authorize]
        public async Task<IActionResult> GetUserEvents()
        {
            User currentUser = await _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            List<Event>? events = currentUser.CreatedEvents.ToList();

            List<EventDto> eventDtos = _mapper.Map<List<EventDto>>(events);

            string json = JsonSerializer.Serialize(eventDtos);

            return Ok(json);
        }

        [HttpGet]
        [Route("registrated-events")]
        [Authorize]
        public async Task<IActionResult> GetRegistratedEvents()
        {
            User currentUser = await _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);

            List<Event> events = _eventService.GetRegistratedEvents(currentUser.UserId);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = System.Text.Json.JsonSerializer.Serialize(events, options);

            return Ok(json);
        }

        [HttpPost]
        [Route("registrate-user")]
        [Authorize]
        public async Task<IActionResult> RegistrateUser([FromBody] string eventName)
        {
            User currentUser = await _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);

            Event ev = await _eventService.GetEventByName(eventName);

            if (ev.RegistedUsers == null)
            {
                ev.RegistedUsers = new List<User>();
            }

            ev.RegistedUsers.Add(currentUser);

            await _eventService.UpdateEvent(ev);

            return Ok();
        }
    }
}
