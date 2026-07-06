using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;
using static Server.Controllers.UserController;

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
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _db.Users
    .Include(u => u.SentLetters)
        .ThenInclude(l => l.Recipient)
    .Include(u => u.SentLetters)
        .ThenInclude(l => l.Addressee)
    .Include(u => u.AcceptLetters)
        .ThenInclude(l => l.Recipient)
    .Include(u => u.AcceptLetters)
        .ThenInclude(l => l.Addressee)
    .SingleOrDefaultAsync(u => u.Id == Guid.Parse(userId));


            return Ok(new UserDto()
            {
                SentLetters = user.SentLetters.Select(l => new LetterDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Text = l.Text,
                    SendTime = l.SendTime,
                    RecipientId = l.RecipientId,
                    AddresseeId = l.AddresseeId
                }).ToList(),
                AcceptLetters = user.AcceptLetters.Select(l => new LetterDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Text = l.Text,
                    SendTime = l.SendTime,
                    RecipientId = l.RecipientId,
                    AddresseeId = l.AddresseeId,
                }).ToList()

            });
        }


        public class UserDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public List<LetterDto> SentLetters { get; set; }
            public List<LetterDto> AcceptLetters { get; set; }
        }
        public class LetterDto
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public string Author { get; set; }
            public bool IsReaden { get; set; }
            public DateTime SendTime { get; set; }
            public Guid RecipientId { get; set; }
            public Guid AddresseeId { get; set; }
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