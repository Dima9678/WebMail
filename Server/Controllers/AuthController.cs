using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private ValidationCheck _val = new();
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
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            string hashedPassword = hasher.HashPassword(new User() ,request.Password);

            user.PasswordHash = hashedPassword;

            Console.WriteLine("Юзер создан");
            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok();

        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            Console.WriteLine("Попытка входа");
            Console.WriteLine(request.Email);
            Console.WriteLine(request.Password);

            return Ok();
        }
    }
}
