using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Service;
using System.Security.Claims;

namespace Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _userService { get; set; }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            UserDTO user = await _userService.Get(userId);

            return Ok(user);
        }
    }
}