using Microsoft.AspNetCore.Mvc;
using Performance.Testing.Common.Domain;
using Performance.Testing.Business.Services.Interfaces;


namespace Performance.Testing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPut("add")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO @params)
        {
            var user = await _userService.CreateUserAsync(@params);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("byUsername/{username}")]
        public async Task<IActionResult> GetUserByUSername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            return Ok(user);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO @params)
        {
            var user = await _userService.UpdateUserAsync(@params);
            return Ok(user);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return Ok(result);
        }
    }
}
