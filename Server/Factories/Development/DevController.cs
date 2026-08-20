using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace Server.Factories.Development
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevController : ControllerBase
    {
        private DatabaseContext _db { get; set; }
        private Random _rnd = new Random();
        public DevController(DatabaseContext db)
        {
            _db = db;
        }

        //https://localhost:7094/api/dev/generate10
        [Authorize]
        [HttpGet("generate/{count:int}")]
        public async Task<IActionResult> AddLetters(int count)
        {
            /*
             * Сгенерировать:
             * полученные
             * отправленные
             * часть из них пометить звездочкой
             * часть отправить в спам
             */

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = await _db.Users.FindAsync(userId);
            List<User> users = await _db.Users
                .Where(r => r.Id != userId)
                .ToListAsync();

            //Генерация полученных писем
            for (int i = 0; i < count; i++)
            {
                Letter letter = new Letter()
                {
                    Recipients = new List<User>()
                    {
                        user
                    },
                    Addressee = users[_rnd.Next(users.Count)],
                    SendTime = DateTime.UtcNow
                        .AddDays(-_rnd.Next(0, 30))
                        .AddHours(-_rnd.Next(0, 24))
                        .AddMinutes(-_rnd.Next(0, 60))
                        .AddSeconds(-_rnd.Next(0, 60)),
                    Title = "Письмо " + i,
                    Text = "Здравствуйте, полученное письмо " + i,
                    LetterStates = new List<LetterState>(),


                };

                letter.AddresseeId = letter.Addressee.Id;

                //Отправитель 
                letter.LetterStates.Add(new LetterState()
                {
                    IsRead = true,
                    UserId = letter.Addressee.Id,
                    IsSpam = false,
                });

                //Получатель
                letter.LetterStates.Add(new LetterState()
                {
                    IsRead = false,
                    UserId = letter.Recipients[0].Id,
                    IsSpam = _rnd.Next(100) >= 80 ? true : false,
                    Starred = _rnd.Next(100) >= 60 ? true : false,
                });

                _db.Letters.Add(letter);

                if (i % 100 == 0)
                {
                    Console.WriteLine($"полученные: {i}/{count}");
                }

            }

            //Генерация отправленных
            for (int i = 0; i < count; i++)
            {
                Letter letter = new Letter()
                {
                    Recipients = new List<User>()
                    {
                        users[_rnd.Next(users.Count)],
                    },
                    Addressee = user,
                    SendTime = DateTime.UtcNow
                        .AddDays(-_rnd.Next(0, 30))
                        .AddHours(-_rnd.Next(0, 24))
                        .AddMinutes(-_rnd.Next(0, 60))
                        .AddSeconds(-_rnd.Next(0, 60)),
                    Title = "Письмо " + i,
                    Text = "Здравствуйте, отправленное письмо " + i,
                    LetterStates = new List<LetterState>()
                };

                letter.AddresseeId = letter.Addressee.Id;

                letter.LetterStates.Add(new LetterState()
                {
                    IsRead = true,
                    UserId = letter.Addressee.Id,
                    IsSpam = false,
                });

                //Получатель
                letter.LetterStates.Add(new LetterState()
                {
                    IsRead = false,
                    UserId = letter.Recipients[0].Id,
                    IsSpam = _rnd.Next(100) >= 80 ? true : false,
                    Starred = _rnd.Next(100) >= 60 ? true : false,
                });

                _db.Letters.Add(letter);
                if (i % 100 == 0)
                {
                    Console.WriteLine($"полученные: {i}/{count}");
                }
            }
            await _db.SaveChangesAsync();
            return Ok();
        }

        //https://localhost:7094/api/dev/dev/generatedrafts
        [Authorize]
        [HttpGet("generatedrafts")]
        public async Task<IActionResult> AddDrafts()
        {


            return Ok();
        }
    }
}

