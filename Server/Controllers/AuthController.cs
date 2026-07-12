using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Server.Factories;
using Server.Normaizators;
using Server.Service;
using Server.Validators;
using System.Security.Claims;
using Domain;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ValidationCheck _val;
        private readonly AuthService _authService;
        private readonly ClaimFactory _claimFactory;
        public AuthController(AuthService service, ValidationCheck vaidation, ClaimFactory claimFactory)
        {
            _authService = service;
            _val = vaidation;
            _claimFactory = claimFactory;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            request = InputNormalizator.NormalizeRegisterDTO(request);

            bool validResult;
            string message;
            (validResult, message) = await _val.ValidateRegisterRequest(request);

            if (!validResult)
            {
                return BadRequest(message);
            }

            User user = await _authService.Register(request);

            ClaimsPrincipal principal = _claimFactory.CreateClaims(user);
            await HttpContext.SignInAsync("Cookies", principal);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            request = InputNormalizator.NormalizeLoginDTO(request);
            bool validResult;
            string message;
            (validResult, message) = await _val.ValidateLoginRequest(request);

            if (!validResult)
            {
                return BadRequest(message);
            }

            User user = await _authService.Login(request);

            if (user != null)
            {
                ClaimsPrincipal principal = _claimFactory.CreateClaims(user);
                await HttpContext.SignInAsync("Cookies", principal);
                return Ok("Успешно");
            }
            else
            {
                return BadRequest("Неправильное имя пользователя или пароль");
            }
            
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return Ok();
        }
    }
}
