﻿using System.ComponentModel.DataAnnotations;
using WebEvent.API.Model.Entity;

namespace WebEvent.API.Model.DTO
{
    public class EventDto
    {
        public string EventName { get; set; } 
        public DateTime Date { get; set; } 
        public ICollection<ParameterDto>? Parameters { get; set; } = new List<ParameterDto>();

    }
}
