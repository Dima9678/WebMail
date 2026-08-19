using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Server.Factories;
using Server.Normaizators;
using Server.Service;
using Server.Validators;
using System.Security.Claims;
using Domain;
using Domain.Models.Requests;
using Domain.Models;

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

            OperationResult result = await _val.ValidateRegisterRequest(request);

            if (!result.Sucsessed)
            {
                return BadRequest(result.ErrorMessage);
            }

            User user = await _authService.RegisterAsync(request);

            ClaimsPrincipal principal = _claimFactory.CreateClaims(user);
            await HttpContext.SignInAsync
                ("Cookies",
                principal,
                new AuthenticationProperties { IsPersistent = request.RememberMe });


            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            request = InputNormalizator.NormalizeLoginDTO(request);
            OperationResult result = await _val.ValidateLoginRequest(request);

            if (!result.Sucsessed)
            {
                return BadRequest(result.ErrorMessage);
            }

            User user = await _authService.LoginAsync(request);
            OperationResult userExistResult = new OperationResult();

            if (user == null)
            {
                userExistResult.ErrorMessage = "Неправильное имя пользователя или пароль";
                userExistResult.Sucsessed = false;

                return BadRequest(userExistResult.ErrorMessage);
            }

            ClaimsPrincipal principal = _claimFactory.CreateClaims(user);
            await HttpContext.SignInAsync
                ("Cookies",
                principal,
                new AuthenticationProperties { IsPersistent = request.RememberMe });

            return Ok(userExistResult.ErrorMessage);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return Ok();
        }
    }
}
