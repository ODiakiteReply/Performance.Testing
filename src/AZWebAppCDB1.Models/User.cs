using System;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;

namespace Performance.Testing.Models
{
    public class User : TableEntity
    {
        public User() : base("/RoleId", "id") { }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
