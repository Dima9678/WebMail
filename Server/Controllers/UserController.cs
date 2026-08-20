using Domain;
using Domain.Intefraces;
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
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            UserDTO user = await _userService.Get(userId);

            return Ok(user);
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> ChangeUserData([FromBody]UserDataChangeRequest request)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _userService.ChangeUserInfo(request, userId);
            return Ok();
        }

        [Authorize]
        [HttpPost("{userId:guid}/add-spam")]
        public async Task<IActionResult> AddToSpam([FromBody] string adresseeEmail)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _userService.AddSpamEmail(userId, adresseeEmail);
            return Ok();
        }

        [Authorize]
        [HttpPost("{userId:guid}/remove-spam")]
        public async Task<IActionResult> RemoveFromSpam([FromBody] string adresseeEmail)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _userService.RemoveSpamEmail(userId, adresseeEmail);
            return Ok();
        }
    }
}
