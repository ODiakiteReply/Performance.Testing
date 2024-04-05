﻿using System;

namespace AZWebAppCDB1.Common.Domain
{
    public class CommentDTO
    {
        public string Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? Author { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? PostId { get; set; }
        public PostDTO? Post { get; set; }
        public string? UserId { get; set; }
        public UserDTO? User { get; set; }
    }
}
