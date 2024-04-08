using System;

namespace AZWebAppCDB1.Common.Domain
{
    public class PostDTO
    {
        public string Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? Author { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UserId { get; set; }
        public UserDTO? User { get; set; }

        ICollection<CommentDTO> CommentDTOs { get; set; } = new List<CommentDTO>();
    }
}
