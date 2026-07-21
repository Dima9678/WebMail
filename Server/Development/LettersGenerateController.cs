using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Server.Development
{
    [ApiController]
    [Route("api/dev/[controller]")]
    public class DevController : ControllerBase
    {
        private DatabaseContext _db { get; set; }
        public DevController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> AddLetters()
        {
            Guid adresseeId = Guid.Parse("dedba6a8-2bd7-42c9-b5c8-ee3099c04dca");
            Guid recipientId = Guid.Parse("cb3f8751-8aea-44d0-a1ff-32dbda067eca");

            var adressee = await _db.Users.SingleOrDefaultAsync(u => u.Id == adresseeId);
            var recipient = await _db.Users.SingleOrDefaultAsync(u => u.Id == recipientId);

            for (int i = 0; i < 45 ; i++)
            {
                Letter letter = new Letter()
                {
                    AddresseeId = adressee.Id,
                    RecipientId = recipient.Id,
                    Title = $"Письмо {i}",
                    Text = $"Здравствуйте Дмитрий, это письмо {i}",
                    SendTime = DateTime.UtcNow,

                    LetterStates = new List<LetterState>()
                {
                    new LetterState()
                    {
                        IsRead = true,
                        UserId = adressee.Id,
                    },
                    new LetterState()
                    {
                        IsRead = false,
                        UserId = recipient.Id,
                    },
                }
                };
                _db.Letters.Add(letter);
                _db.SaveChanges();
            }

            return Ok();
        }
    }
}

    