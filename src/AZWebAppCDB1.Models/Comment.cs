using System;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;


namespace AZWebAppCDB1.Models
{
    public class Comment : TableEntity
    {
        public Comment() : base("/PostId", "id") { }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}
