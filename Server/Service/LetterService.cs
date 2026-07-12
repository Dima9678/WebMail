using Domain;
using Domain.Models;
using Domain.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Server.Controllers;
using Server.Mappers;
using System.Security.Claims;

namespace Server.Service
{
    public class LetterService
    {
        private readonly DatabaseContext _db;
        public LetterService(DatabaseContext db)
        {
            _db = db;
        }
        //Сделай тоже чтобы принимал Guid
        public async Task Add(NewLetterDTO request, Guid adresseeId)
        {
            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Recipient);
            var adressee = await _db.Users.SingleOrDefaultAsync(u => u.Id == adresseeId);

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
        }
        public async Task<LetterDTO> GetById(Guid id)
        {
            Letter? letterInDb = await _db.Letters
                .Include(u => u.Addressee)
                .SingleOrDefaultAsync(l => l.Id == id);

            LetterDTO letterDTO = LetterMapper.ToDto(letterInDb);

            ChangeIsReaden(letterInDb);
            

            return letterDTO;
        }
        public async Task<List<LetterDTO>> GetAcceptLetters(Guid id)
        {
            List<LetterDTO> userLetters = await _db.Letters
                .Include (l => l.Addressee)
                .Where(l => l.RecipientId == id)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l)).ToListAsync();

            return userLetters;
        }
        public async Task<List<LetterDTO>> GetStarredLetters(Guid id)
        {
            List<LetterDTO> userLetters = await _db.Letters
                .Where(l => l.RecipientId == id)
                .Where(l => l.Starred == true)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l)).ToListAsync();

            return userLetters;
        }
        public async Task<List<LetterDTO>> GetSentLetters(Guid id)
        {
            List<LetterDTO> userLetters = await _db.Letters
                .Where(l => l.AddresseeId == id)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l)).ToListAsync();

            return userLetters;
        }
        public async Task ChangeStarred(Guid id)
        {
            var letter = await _db.Letters
        .FirstOrDefaultAsync(l => l.Id == id);

            letter.Starred = !letter.Starred;

            await _db.SaveChangesAsync();
        }
        private async Task ChangeIsReaden(Letter letterInDb)
        {
            letterInDb.IsReaden = true;
            await _db.SaveChangesAsync();
        }
    }
}
