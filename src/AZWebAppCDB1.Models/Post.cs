using System;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;

namespace AZWebAppCDB1.Models
{
    public class Post : TableEntity
    {
        public Post() : base("/UserId", "id") { }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
