using Microsoft.AspNetCore.Mvc;
using AZWebAppCDB1.Common.Domain;
using AZWebAppCDB1.Business.Services.Interfaces;


namespace AZWebAppCDB1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly ICommentService _commentService;

        public CommentController(
            ILogger<CommentController> logger,
            ICommentService commentService)
        {
            _logger = logger;
            _commentService=commentService;
        }

        [HttpPut("add")]
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO @params)
        {
            var comment = await _commentService.CreateAsync(@params);
            return Ok(comment);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            //var comments = await _commentService.GetAllAsync();
            Console.WriteLine("GetComments");
            return Ok(new List<CommentDTO> { new() {
                    Id = string.Empty,
                    Content = "test content",
                    CreatedAt = DateTime.Now,
                    Author = "test author",
                    UpdatedAt = DateTime.Now,
                } 
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            return Ok(comment);
        }

        [HttpGet("byPostId/{postId}")]
        public async Task<IActionResult> GetCommentByPostId(string postId)
        {
            var comment = await _commentService.GetByPostAsync(postId);
            return Ok(comment);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentDTO @params)
        {
            var comment = await _commentService.UpdateAsync(@params);
            return Ok(comment);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            var result = await _commentService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
