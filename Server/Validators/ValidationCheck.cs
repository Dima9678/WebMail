using Microsoft.EntityFrameworkCore;
using Persistence;
using Server.Controllers;

namespace Server.Validators
{
    public class ValidationCheck
    {
        private DatabaseContext _db { get; set; }
        public ValidationCheck(DatabaseContext db)
        {
            _db = db;
        }
        public bool EqualInputPasswords(string firstPassword, string secondPassword)
        {
            if (firstPassword == secondPassword)
            {
                return true;
            }
            return false;
        }
        public bool CorrectEmai(string email)
        {
            //@mymail.com
            if (email.Length <= 11)
            {
                return false;
            }
            else if (email[^11..^0] != "@mymail.com")
            {
                return false;
            }
            return true;
        }
        public bool PasswordLength(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }
            return true;
        }

        public async Task<(bool, string)> ValidateWriteLetterRequest(NewLetterDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Recipient))
            {
                return (false, "Не введен получатель");
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return (false, "Тема письма не может быть пустой");
            }

            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return (false, "Текст письма не может быть пустым");
            }

            if (!CorrectEmai(request.Recipient))
            {
                return (false, "Некорректный формат почты получателя");
            }

            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Recipient);
            if (userInDb == null)
            {
                return (false, "Такого пользователя не существует");
            }

            return (true, string.Empty);
        }
    }
}
