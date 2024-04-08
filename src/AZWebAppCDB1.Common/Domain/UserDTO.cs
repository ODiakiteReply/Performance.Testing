using System;
using AZWebAppCDB1.Common.Enums;


namespace AZWebAppCDB1.Common.Domain
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public RoleEnum Role { get; set; }
        public string RoleDescription { 
            get => Role.ToString();
            private set { } }
        public string FullName => 
            $"{FirstName ?? string.Empty} {LastName ?? string.Empty}";
    }
}
