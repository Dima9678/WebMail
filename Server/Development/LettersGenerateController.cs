using Microsoft.AspNetCore.Mvc;
using Persistence;
using Domain.Models;

namespace Server.Development
{
    [ApiController]
    [Route("api/dev/[controller]")]
    public class LettersGenerateController : ControllerBase
    {
        private DatabaseContext _db { get; set; }
        public LettersGenerateController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> AddLetters()
        {
            for (int i = 0; i < 150; i++)
            {
                Letter letter = new Letter()
                {
                    RecipientId = new Guid("8cf8a5f5-e42b-4a73-b904-2aaf4afa1996"),
                    Title = $"Письмо {i}",
                    Text = $"Здравствуйте Дмитрий, это тестовое письмо под номером {i}",
                    AddresseeId = new Guid("8cf8a5f5-e42b-4a73-b904-2aaf4afa1996"),
                    Starred = false,
                };
                _db.Letters.Add(letter);
                
            }
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}

    