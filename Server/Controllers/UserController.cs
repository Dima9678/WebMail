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
            Random rnd = new Random();
            User user = new User();
            //Напиши бд
            for (int i = 0; i < 15; i++)
            {
                Letter newLetter = new Letter()
                {
                    IsSent = rnd.Next(0, 2) == 0 ? true : false
                };

                if (newLetter.IsSent)
                {
                    newLetter.Text = "Исходящее письмо";
                    user.SentLetters.Add(newLetter);
                }
                else
                {
                    newLetter.Text = "Входящее письмо";
                    user.AcceptLetters.Add(newLetter);
                }

                user.Letters.Add(newLetter);
            }
            Console.WriteLine("Запрос");
            return user;
        }
    }
}