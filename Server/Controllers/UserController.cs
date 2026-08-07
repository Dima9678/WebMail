using Domain;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Server.Service;
using System.Security.Claims;
using System.Xml.Linq;

namespace Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _userService;

        public UserController(UserService userService)
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
    }
}
