using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public User Get()
        {
            User user = new User();
            //Напиши бд
            user.Letters = new List<Letter>();
            for (int i = 0; i < 15; i++)
            {
                user.Letters.Add(new Letter()
                {
                    Text = "Письмо"
                });
            }
            Console.WriteLine("Запрос");
            return user;
        }
    }
}