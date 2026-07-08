using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;
using Domain.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LetterController : ControllerBase
    {
        private DatabaseContext _db { get; set; }
        public LetterController(DatabaseContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpPost("write")]
        public async Task<IActionResult> Write([FromBody] NewLetterRequest request)
        {
            ValidationCheck check = new ValidationCheck();

            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Recipient);
            if (userInDb == null)
            {
                return BadRequest("Такого пользователя не существует");
            }
            //вынести валидацию
            if (request.Recipient == null)
            {
                return BadRequest("Не введен получатель");
            }
            if (request.Title == null)
            {
                return BadRequest("Тема письма не может быть пустой");
            }
            if (request.Text == null)
            {
                return BadRequest("Текст письма не может быть пустым");
            }

            if (!check.CorrectEmai(request.Recipient))
            {
                return BadRequest("Некорректный формат почты получателя");
            }

            string? adresseeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var adressee = await _db.Users.SingleOrDefaultAsync(u => u.Id == Guid.Parse(adresseeId));

            Letter letter = new Letter()
            {
                AddresseeId = adressee.Id,
                RecipientId = userInDb.Id,
                Title = request.Title,
                Text = request.Text,
                SendTime = DateTime.UtcNow,

            };
            _db.Letters.Add(letter);
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var letterInDb = await _db.Letters
                .Include(u => u.Addressee)
                .SingleOrDefaultAsync(l => l.Id == id);

            if (letterInDb == null)
                return NotFound();

            LetterDTO letter= new LetterDTO()
            {
                Id = letterInDb.Id,
                AdresseeName = letterInDb.Addressee.Name,
                Title = letterInDb.Title,
                Text = letterInDb.Text,
                SendTime = letterInDb.SendTime,
                Starred = letterInDb.Starred,
            };

            letterInDb.IsReaden = true;
            await _db.SaveChangesAsync();

            return Ok(letter);
        }

        [Authorize]
        [HttpGet("getuserletters")]
        public async Task<IActionResult> GetUserLetters()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userLetters = await _db.Letters
                .Where(l => l.RecipientId == userId)
                .OrderByDescending(l => l.SendTime)
                .Select(l => new LetterDTO
                {
                    Id = l.Id,
                    AdresseeName = l.Addressee.Name,
                    AdresseeEmail = l.Addressee.Email,
                    IsReaden = l.IsReaden,
                    Title= l.Title,
                    Text = l.Text,
                    SendTime = l.SendTime,
                    Starred = l.Starred,

                })
                .ToListAsync();
            return Ok(userLetters);
        }
        
    }

    public class NewLetterRequest
    {
        public string Recipient { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
