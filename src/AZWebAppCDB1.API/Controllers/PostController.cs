using Microsoft.AspNetCore.Mvc;
using AZWebAppCDB1.Common.Domain;
using AZWebAppCDB1.Business.Services.Interfaces;


namespace AZWebAppCDB1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly IPostService _postService;

        public PostController(
            ILogger<PostController> logger, 
            IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [HttpPut("add")]
        public async Task<IActionResult> CreatePost([FromBody] PostDTO @params)
        {
            var post = await _postService.CreateAsync(@params);
            return Ok(post);
        }

        [HttpGet]
        public async Task<IActionResult> GetPoss()
        {
            var posts = await _postService.GetAllAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(string id)
        {
            var post = await _postService.GetByIdAsync(id);
            return Ok(post);
        }

        [HttpGet("byUserId/{userId}")]
        public async Task<IActionResult> GetPostByUsername(string userId)
        {
            var posts = await _postService.GetByUserAsync(userId);
            return Ok(posts);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdatePost([FromBody] PostDTO @params)
        {
            var post = await _postService.UpdateAsync(@params);
            return Ok(post);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _postService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
