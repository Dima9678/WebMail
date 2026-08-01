using Domain;
using Domain.Models;
using Domain.Models.DTO;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Persistence;
using Server.Mappers;

namespace Server.Service
{
    /*
     * Отправленные
     * Избранные
     * Черновики
     * Спам
     * Корзина
     */

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
        public async Task AddReply(ReplyDTO replyText, Guid adresseeId, Guid parentLetterId)
        {
            var parentLetter = await _db.Letters.SingleOrDefaultAsync(u => u.Id == parentLetterId);
            var adressee = await _db.Users.SingleOrDefaultAsync(u => u.Id == adresseeId);

            Letter letter = new Letter()
            {
                AddresseeId = adressee.Id,
                RecipientId = parentLetter.AddresseeId,
                Title = $"Ответ на письмо: \"{parentLetter.Title}\"",
                Text = replyText.ReplyText,
                SendTime = DateTime.UtcNow,
                ParentLetter = parentLetter,
                ParentLetterId = parentLetterId,

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
                        UserId = parentLetter.AddresseeId,
                    },
                }
            };

            parentLetter.AddChild(letter);

            _db.Letters.Add(letter);
            _db.SaveChanges();
        }
        public async Task<FullLetterDTO> GetById(Guid letterId, Guid userId)
        {
            Letter? letterInDb = await _db.Letters
                .Where(u => u.Id == letterId)
                .Include(u => u.LetterStates)
                .Include(u => u.Addressee)
                .Include(u => u.ParentLetter)
                .Include(u => u.ParentLetter.Addressee)
                .Include(u => u.ParentLetter.LetterStates)
                .Include(u => u.ChildrenLetters
                    .Where(l => l.RecipientId == userId || l.AddresseeId == userId))
                    .ThenInclude(l => l.Addressee)
                .SingleOrDefaultAsync();

            if (letterInDb == null)
            {
                return null;
            }
            if (letterInDb.Forwarded == true)
            {
                await _db.Entry(letterInDb)
                    .Reference(l => l.OriginalAuthor)
                    .LoadAsync();
            }


            await ChangeIsReaden(userId, letterInDb);

            FullLetterDTO fullLetterDTO = LetterMapper.ToFullDto(letterInDb);
            fullLetterDTO = await AppendNavigationInfo(fullLetterDTO);
            return fullLetterDTO;
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
        public async Task<List<LetterDTO>> GetSentLetters(Guid userId, int startIndex, int endIndex)
        {
            List<LetterDTO> userLetters = await _db.Letters
                .Where(l => l.AddresseeId == userId)
                .Include(l => l.Addressee)
                .Include(l => l.LetterStates)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l)).ToListAsync();

            List<LetterDTO> filtredetters = new List<LetterDTO>();

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i < userLetters.Count)
                {
                    filtredetters.Add(userLetters[i]);
                }
                else
                {
                    break;
                }
            }

            return filtredetters;
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
        public async Task ChangeIsReaden(Guid letterId, Guid userId)
        {
            LetterState? state = _db.LetterStates
                .Where(x => x.LetterId == letterId)
                .FirstOrDefault(x => x.UserId == userId);

            state.IsRead = !state.IsRead;

            await _db.SaveChangesAsync();
        }
        public async Task<int> GetTotalAcceptCount(Guid userId)
        {
            int count = await _db.Letters
                .Where(l => l.RecipientId == userId)
                .CountAsync();

            return count;
        }
        public async Task<int> GetTotalSendCount(Guid userId)
        {
            int count = await _db.Letters
                .Where(l => l.AddresseeId == userId)
                .CountAsync();

            return count;
        }
        public async Task<FullLetterDTO> AppendNavigationInfo([FromBody] FullLetterDTO fullLetter)
        {
            var letters = await _db.Letters
                .Where(l => l.RecipientId == fullLetter.RecipientId)
                .OrderByDescending(l => l.SendTime)
                .Select(l => l.Id)
                .ToListAsync();

            int letterIndex = letters.IndexOf(fullLetter.Id);

            Guid? nextId = letterIndex > 0 ? letters[letterIndex - 1] : null;
            Guid? previousId = letterIndex < letters.Count - 1 ? letters[letterIndex + 1] : null;
            int letterNumber = letterIndex + 1;

            fullLetter.PreviousLetterId = previousId;
            fullLetter.NextLetterId = nextId;
            fullLetter.LetterNumber = letterNumber;

            return fullLetter;
        }
        public async Task<OperationResult> Forward(ForwardRequest request, Guid userId)
        {
            //Это юзер, который пересылает сообщение
            User? adressee = await _db.Users.SingleOrDefaultAsync(u => u.Id == userId);
            //Это получатель, которому будет переслано сообщение
            User? recipient = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.ForwardEmail);
            //Если не найден, выбросить ошибку

            if (adressee == null)
            {
                return new OperationResult()
                {
                    Sucsessed = false,
                    ErrorMessage = "Такого пользователя не существует",
                };
            }

            if (recipient == null)
            {
                return new OperationResult()
                {
                    Sucsessed = false,
                    ErrorMessage = "Получатель не найден",
                };
            }

            //оригинальное письмо
            Letter? letterInDb = await _db.Letters
                .Include(x => x.Addressee)
                .Include(x => x.Recipient)
                .Include(x => x.Recipient)
                .FirstOrDefaultAsync(x => x.Id == request.LetterId);

            if (letterInDb == null)
            {
                return new OperationResult()
                {
                    Sucsessed = false,
                    ErrorMessage = "Письмо не найдено",
                };
            }

            Letter forwardLetter = new()
            {
                Title = letterInDb.Title,
                Text = letterInDb.Text,

                Addressee = adressee,
                AddresseeId = adressee.Id,

                Recipient = recipient,
                RecipientId = recipient.Id,

                Forwarded = true,
                OriginalAuthor = letterInDb.Addressee,
                OriginalAuthorId = letterInDb.AddresseeId,

                SendTime = letterInDb.SendTime,

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
                },

                ChildrenLetters = letterInDb.ChildrenLetters,
            };

            _db.Letters.Add(forwardLetter);
            _db.SaveChanges();

            return new OperationResult()
            {
                Sucsessed = true,
                ErrorMessage = null,
            };
        }
    }
}