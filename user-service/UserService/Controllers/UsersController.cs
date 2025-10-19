using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO.UserDTO;
using UserService.Interfaces.IServices;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterUserDTO newUser)
        {
            var result = await _userService.Register(newUser);
            if (result == "failed")
                return BadRequest("Failed to register");
            if (result == "emailexists")
                return BadRequest("Email already registered!");
            if (result == "usernameexists")
                return BadRequest("Username already registered!");
            if (result == "weakpass")
                return BadRequest("Password must be at least 7 characters long!");
            return Ok();
        }

        [HttpPost("authentication")]
        [AllowAnonymous]

        public async Task<IActionResult> Authentication(LoginUserDTO loginUser)
        {
            var response = await _userService.Authenticate(loginUser);

            if (response == null)
                return BadRequest("Invalid credentials!");


            return Ok(response);
        }

        [HttpPost("usernames-with-pictures")]
        public async Task<IActionResult> GetUsernamesWithPictures([FromBody] List<long> ids)
        {
            var result = await _userService.GetUsersByIdsAsync(ids);
            return Ok(result);
        }
    }
}
