using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private ValidationCheck _val = new();
        private PasswordHasher<User> _hasher = new();
        private readonly DatabaseContext _db;
        public AuthController(DatabaseContext db)
        {
            _db = db;
        }
        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            Console.WriteLine("Попытка регистрации");
            Console.WriteLine(request.Name);
            Console.WriteLine(request.Email);
            Console.WriteLine(request.Password);
            Console.WriteLine(request.RepeatPassword);

            request.Name = request.Name.Trim();
            request.Email = request.Email.Trim();
            request.Email = request.Email.ToLower();
            request.Password = request.Password.Trim();
            request.RepeatPassword = request.Password.Trim();

            if (!_val.CorrectEmai(request.Email))
            {
                return BadRequest("Невалидное значение Email");
            }

            if (!_val.EqualInputPasswords(request.Password, request.RepeatPassword))
            {
                return BadRequest("Пароли не совпадают");
            }

            if (!_val.PasswordLength(request.Password))
            {
                return BadRequest("Длина пароля должна быть больше либо равна 8 символам");
            }

            //поиск юзера
            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (userInDb != null)
            {
                return BadRequest("Пользователь с таким Email уже существует");
            }

            User user = new User()
            {
                Name = request.Name,
                Email = request.Email,
            };
            //537 689 93 17
            //dmitry9678@mymail.com
            //5376899317
            string hashedPassword = _hasher.HashPassword(new User() ,request.Password);

            user.PasswordHash = hashedPassword;

            Console.WriteLine("Юзер создан");
            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok();

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            Console.WriteLine("Попытка входа");
            Console.WriteLine(request.Email);
            Console.WriteLine(request.Password);


            request.Email = request.Email.Trim();
            request.Email = request.Email.ToLower();
            request.Password = request.Password.Trim();

            if (!_val.CorrectEmai(request.Email))
            {
                return BadRequest("Невалидное значение Email");
            }

            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

            //Пользователь с таким именем уже существует

            if (userInDb == null)
            {
                return BadRequest("Пользователя с таким Email не существует");
            }

            string hashedPassword = _hasher.HashPassword(new User(), request.Password);

            Console.WriteLine(hashedPassword);
            Console.WriteLine(userInDb.PasswordHash);

            var result = _hasher.VerifyHashedPassword(
            userInDb,
            userInDb.PasswordHash,
            request.Password);

            if (result == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userInDb.Id.ToString()),
                    new Claim(ClaimTypes.Name, userInDb.Name),
                    new Claim(ClaimTypes.NameIdentifier, userInDb.Email)
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return Ok();
            }
            else
            {
                return BadRequest("Неверный пароль");
            }

            
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                Name = User.Identity?.Name,
                Email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return Ok();
        }
    }

    public class UserRegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
