﻿namespace WebEvent.API.Model.DTO
{
    public class UserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? VerificationToken { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;
    }
}
