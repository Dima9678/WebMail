using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;
using static Server.Controllers.UserController;

using Server.Mappers;
using Server.Controllers;
using Server.Service;
using Server.Validators;

namespace Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private DatabaseContext _db { get; set; }
        public UserController(DatabaseContext db)
        {
            _db = db;
        }

        private UserMapper _userMapper { get; set; }
        private UserService _userService { get; set; }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            UserDTO user = _userService.Get(userId);

            return Ok(user);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public List<ReturningLetter> RandomLetters()
        {
            string[] lettersText = new string[]
            {
                "Димончик, мой хороший, установи линукс",
                "Вы пиздячили так много кода, что обогнали майкрософт за последние несколько дней",
                "Нам пришла попытка регистрации",
                "Новая версия .NET 11.0 уже вышла",
            };
            string[] lettersTitle = new string[]
            {
                "Установи линукс",
                "Вы обогнали Майкрософт",
                "Проверочный код",
                "Обновления в NET Core",
            };
            string[] lettersAuthor = new string[]
            {
                "Linus Torwalds",
                "GitHub",
                "Goolge",
                "Microsoft",
            };
            List<ReturningLetter> letters = new List<ReturningLetter>();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                int randomLetterIndex = random.Next(0,4);

                ReturningLetter letter = new ReturningLetter()
                {
                    Author = lettersAuthor[randomLetterIndex],
                    Text = lettersText[randomLetterIndex],
                    Title = lettersTitle[randomLetterIndex],
                    SendTime = DateTime.Today
                };

                if (random.Next(0, 5) <= 2)
                {
                    letter.IsReaden = true;
                }
                else
                {
                    letter.IsReaden = false;
                }
                letters.Add(letter);

            }
            return letters;
        }
    }

    public class ReturningLetter
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public bool IsReaden { get; set; }
        public DateTime SendTime { get; set; }
    }
}