using Domain.Models;
using Domain.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Server.Controllers;
using Server.Mappers;

namespace Server.Service
{
    public class LetterService
    {
        private readonly DatabaseContext _db;
        public LetterService(DatabaseContext db)
        {
            _db = db;
        }
        public async Task Add(NewLetterDTO request, Guid adresseeId)
        {
            var recipient = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Recipient);
            var adressee = await _db.Users.SingleOrDefaultAsync(u => u.Id == adresseeId);

            Letter letter = new Letter()
            {
                AddresseeId = adressee.Id,
                RecipientId = recipient.Id,
                Title = request.Title,
                Text = request.Text,
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
        public async Task<LetterDTO> GetById(Guid letterId, Guid userId)
        {
            Letter? letterInDb = await _db.Letters
                .Where(u  => u.Id == letterId)
                .Include(u => u.LetterStates)
                .Include(u => u.Addressee)
                .SingleOrDefaultAsync();

            LetterDTO letterDTO = LetterMapper.ToDto(letterInDb);

            await ChangeIsReaden(userId, letterInDb);
            

            return letterDTO;
        }
        public async Task<List<LetterDTO>> GetAcceptLetters(Guid userId, int startIndex, int endIndex)
        {
            List<LetterDTO> letters = await _db.Letters
                .Where(l => l.RecipientId == userId)
                .Include(l => l.Addressee)
                .Include(l => l.LetterStates)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l)).ToListAsync();

            List<LetterDTO> filtredetters = new List<LetterDTO>();

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i < letters.Count)
                {
                    filtredetters.Add(letters[i]);
                }
                else
                {
                    break;
                }
            }

            return filtredetters;
        }
        public async Task<List<LetterDTO>> GetStarredLetters(Guid userId)
        {
            List<LetterDTO> userLetters = await _db.Letters
                .Where(l => l.RecipientId == userId || l.AddresseeId == userId)
                .Where(l => l.LetterStates.Any(s => s.Starred))
                .Include(l => l.LetterStates)
                .Include(l => l.Addressee)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l))
                .ToListAsync();

            return userLetters;
        }
        public async Task<List<LetterDTO>> GetSentLetters(Guid userId)
        {
            List<LetterDTO> userLetters = await _db.Letters
                .Where(l => l.AddresseeId == userId)
                .Include(l => l.Addressee)
                .Include(l => l.LetterStates)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l)).ToListAsync();

            return userLetters;
        }
        public async Task ChangeStarred(Guid letterId, Guid userId)
        {
            Letter? letterInDb = await _db.Letters
                .Include(u => u.LetterStates)
                .SingleOrDefaultAsync(l => l.Id == letterId);

            var state = letterInDb.LetterStates
        .Single(x => x.UserId == userId);

            state.Starred = !state.Starred;

            await _db.SaveChangesAsync();
        }
        private async Task ChangeIsReaden(Guid userId, Letter letterInDb)
        {
            var state = letterInDb.LetterStates
        .FirstOrDefault(x => x.UserId == userId);

            state.IsRead = true;

            await _db.SaveChangesAsync();
        }

        public async Task<int> GetTotalLettersCount(Guid userId)
        {
            int count = await _db.Letters
                .Where(l => l.RecipientId == userId)
                .CountAsync();

            return count;
        }
    }
}
