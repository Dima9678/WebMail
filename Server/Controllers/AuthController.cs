using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Persistence;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _db;
        public AuthController(DatabaseContext db)
        {
            _db = db;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterRequest request)
        {
            Console.WriteLine("Попытка регистрации");
            Console.WriteLine(request.Name);
            Console.WriteLine(request.Email);
            Console.WriteLine(request.Password);
            Console.WriteLine(request.RepeatPassword);
            if (request.Password == request.RepeatPassword)
            {
                User user = new User()
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = request.Password,
                };

                _db.Users.Add(user);
                _db.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("короче пароли не совпдают");
            }
            
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
