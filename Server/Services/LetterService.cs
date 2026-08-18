using Domain;
using Domain.Models;
using Domain.Models.DTO;
using Domain.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Persistence;
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
        public async Task Create(NewLetterDTO request, Guid adresseeId, string[] recipients)
        {
            User[] recipientsList = new User[recipients.Length];
            for (int i = 0; i < recipients.Length; i++)
            {
                recipientsList[i] = await _db.Users.SingleOrDefaultAsync(u => u.Email == recipients[i]);
            }
            var adressee = await _db.Users.SingleOrDefaultAsync(u => u.Id == adresseeId);

            List<LetterState> recipientsStates = new List<LetterState>();

            //добавляем состояние адресата
            recipientsStates.Add(new LetterState()
            {
                IsRead = true,
                UserId = adressee.Id,
                IsSpam = false,
            });

            //Добавление состояний получателя
            for (int i = 0; i < recipientsList.Length; i++)
            {
                var recipientState = new LetterState()
                {
                    UserId = recipientsList[i].Id,
                    IsRead = false,
                };
                if (recipientsList[i].SpamEmails != null)
                {
                    if (recipientsList[i].SpamEmails.Contains(adressee.Email))
                    {
                        recipientState.IsSpam = true;
                    }
                    else
                    {
                        recipientState.IsSpam = false;
                    }
                    recipientsStates.Add(recipientState);
                }
            }

            Letter letter = new Letter()
            {
                AddresseeId = adressee.Id,
                Title = request.Title,
                Text = request.Text,
                SendTime = DateTime.UtcNow,
                LetterStates = recipientsStates,
                Recipients = recipientsList.ToList(),

            };
            _db.Letters.Add(letter);
            _db.SaveChanges();
        }
        public async Task CreateReply(ReplyDTO replyText, Guid adresseeId, Guid parentLetterId)
        {
            var parentLetter = await _db.Letters.SingleOrDefaultAsync(u => u.Id == parentLetterId);
            var adressee = await _db.Users.SingleOrDefaultAsync(u => u.Id == adresseeId);

            Letter letter = new Letter()
            {
                AddresseeId = adressee.Id,
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
                .Include(x => x.Recipients)
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

                Recipients = letterInDb.Recipients,

                ForwardRecipient = recipient,
                ForwardRecipientId = recipient.Id,

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

        public async Task<FullLetterDTO> GetById(Guid letterId, Guid userId, string from)
        {
            Letter? letterInDb = await _db.Letters
                .Where(u => u.Id == letterId)
                .Include(u => u.LetterStates
                    .Where(r => r.UserId == userId))
                .Include(u => u.Addressee)
                .Include(u => u.Recipients)
                .Include(u => u.ParentLetter)
                    .ThenInclude(u => u.Addressee)
                    .ThenInclude(u => u.LetterStates)
                .Include(u => u.ChildrenLetters
                    .Where(l => l.Recipients.Any(r => r.Id == userId) || l.AddresseeId == userId))
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

            if (from == "starred")
            {
                fullLetterDTO = await AppendStarredNavigationInfo(fullLetterDTO, userId);
            }
            else if (from == "sent")
            {
                fullLetterDTO = await AppendSentNavigationInfo(fullLetterDTO, userId);
            }
            else
            {
                fullLetterDTO = await AppendAcceptNavigationInfo(fullLetterDTO, userId);
            }
            return fullLetterDTO;
        }

        public async Task<List<LetterDTO>> GetAcceptLetters(Guid userId, int startIndex, int endIndex)
        {
            List<LetterDTO> letters = await _db.Letters
                .Where(l => l.Recipients.Any(r => r.Id == userId))
                .Where(l => l.LetterStates.Any(r => r.UserId == userId && !r.IsSpam))
                .Include(l => l.Addressee)
                .Include(l => l.LetterStates)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l))
                .Skip(startIndex)
                .Take(endIndex - startIndex)
                .ToListAsync();

            return letters;
        }
        public async Task<List<LetterDTO>> GetSentLetters(Guid userId, int startIndex, int endIndex)
        {
            List<LetterDTO> userLetters = await _db.Letters
                .Where(l => l.AddresseeId == userId)
                .Include(l => l.Addressee)
                .Include(l => l.LetterStates.Where(r => r.Id == userId))//отправленное мной письмо спамом быть не может
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
        public async Task<List<LetterDTO>> GetStarredLetters(Guid userId)
        {
            List<LetterDTO> userLetters = await _db.Letters
                .Where(l => l.Recipients.Any(r => r.Id == userId) || l.AddresseeId == userId)
                .Where(l => l.LetterStates.Any(s => s.UserId == userId && s.Starred == true && !s.IsSpam))
                .Include(l => l.LetterStates)
                .Include(l => l.Addressee)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l))
                .ToListAsync();

            return userLetters;
        }
        public async Task<List<LetterDTO>> GetSpamLetters(Guid userId)
        {
            List<LetterDTO> userLetters = await _db.Letters
                .Where(l => l.Recipients.Any(r => r.Id == userId) || l.AddresseeId == userId)
                .Where(l => l.LetterStates.Any(s => s.UserId == userId && s.IsSpam == true))
                .Include(l => l.LetterStates)
                .Include(l => l.Addressee)
                .OrderByDescending(l => l.SendTime)
                .Select(l => LetterMapper.ToDto(l))
                .ToListAsync();

            return userLetters;
        }

        public async Task ChangeStarred(Guid letterId, Guid userId)
        {
            Letter? letterInDb = await _db.Letters
                .Include(u => u.LetterStates)
                .SingleOrDefaultAsync(l => l.Id == letterId);

            LetterState state = letterInDb.LetterStates
        .SingleOrDefault(x => x.UserId == userId);

            state.Starred = !state.Starred;

            await _db.SaveChangesAsync();
        }
        private async Task ChangeIsReaden(Guid userId, Letter letterInDb)
        {
            var state = letterInDb.LetterStates
       .FirstOrDefault(x => x.UserId == userId);

            if (state == null)
                return;

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
        public async Task ChangeIsSpam(Guid letterId, Guid userId)
        {
            LetterState? state = _db.LetterStates
                .Where(x => x.LetterId == letterId)
                .FirstOrDefault(x => x.UserId == userId);

            state.IsSpam = !state.IsSpam;

            await _db.SaveChangesAsync();
        }

        public async Task<int> GetAcceptCount(Guid userId)
        {
            int count = await _db.Letters
                .Where(l => l.Recipients.Any(r => r.Id == userId))
                .Where(l => l.LetterStates.Any(r => r.UserId == userId && !r.IsSpam))
                .CountAsync();

            return count;
        }
        public async Task<int> GetSendCount(Guid userId)
        {
            int count = await _db.Letters
                .Where(l => l.AddresseeId == userId)
                .Where(l => l.LetterStates.Any(r => r.Id == userId && !r.IsSpam))
                .CountAsync();

            return count;
        }
        public async Task<int> GetStarredCount(Guid userId)
        {
            int count = await _db.Letters
                .Where(l => l.Recipients.Any(r => r.Id == userId) || l.AddresseeId == userId)
                .Where(l => l.LetterStates.Any(s => s.UserId == userId && s.Starred == true && !s.IsSpam))
                .CountAsync();

            return count;
        }
        public async Task<int> GetSpamCount(Guid userId)
        {
            int count = await _db.Letters
                .Where(l => l.Recipients.Any(r => r.Id == userId) || l.AddresseeId == userId)
                .Where(l => l.LetterStates.Any(s => s.UserId == userId && s.IsSpam == true))
                .CountAsync();

            return count;
        }

        public async Task<FullLetterDTO> AppendAcceptNavigationInfo(FullLetterDTO fullLetter, Guid userId)
        {
            var letters = await _db.Letters
                .Where(l => l.Recipients.Any(r => r.Id == userId))
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
        public async Task<FullLetterDTO> AppendSentNavigationInfo(FullLetterDTO fullLetter, Guid userId)
        {
            var letters = await _db.Letters
                .Where(l => l.AddresseeId == userId)
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
        public async Task<FullLetterDTO> AppendStarredNavigationInfo(FullLetterDTO fullLetter, Guid userId)
        {
            var letters = await _db.Letters
                .Where(l => l.Recipients.Any(r => r.Id == userId) || l.AddresseeId == userId)
                .Where(l => l.LetterStates.Any(s => s.UserId == userId && s.Starred == true))
                .OrderByDescending(l => l.SendTime)
                .Select(l => l.Id)
                .ToListAsync();

            int letterIndex = letters.IndexOf(fullLetter.Id);

            Guid? previousId = letterIndex + 1 < letters.Count ? letters[letterIndex + 1] : null;
            Guid? nextId = letterIndex > 0 ? letters[letterIndex - 1] : null;
            int letterNumber = letterIndex + 1;

            Console.WriteLine("\n\nПредыдущее письмо " + previousId);
            Console.WriteLine("Следующее письмо " + nextId);
            Console.WriteLine("Номер письма " + letterNumber + "\n\n");

            fullLetter.PreviousLetterId = previousId;
            fullLetter.NextLetterId = nextId;
            fullLetter.LetterNumber = letterNumber;

            return fullLetter;
        }
    }
}