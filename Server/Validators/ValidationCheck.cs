using Microsoft.EntityFrameworkCore;
using Persistence;
using Domain.Models.DTO;
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
        public async Task<(bool, string)> ValidateWriteLetterRequest(NewDraftDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Recipient) &&
                string.IsNullOrWhiteSpace(request.Title) &&
                string.IsNullOrWhiteSpace(request.Text)
                )
            {
                return (false, "Для созранения черновика должно быть хотя-бы одно значение");
            }

            if (!string.IsNullOrWhiteSpace(request.Recipient))
            {
                if (!CorrectEmai(request.Recipient))
                {
                    return (false, "Некорректный формат почты получателя");
                }

                var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Recipient);
                if (userInDb == null)
                {
                    return (false, "Такого пользователя не существует");
                }
            }

            return (true, string.Empty);
        }
        public async Task<(bool, string)> ValidateRegisterRequest(RegisterDTO request)
        {
            if (!EqualInputPasswords(request.Password, request.RepeatPassword))
            {
                return (false, "Пароли не совпадают");
            }

            if (!PasswordLength(request.Password))
            {
                return (false, "Длина пароля должна быть больше либо равна 8 символам");
            }

            if (request.Email.Contains("kal"))
            {
                return (false, "Иди нахер со своим калом");
            }

            //поиск юзера
            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (userInDb != null)
            {
                return (false, "Пользователь с таким Email уже существует");
            }

            return (true, null);
        }
        public async Task<(bool, string)> ValidateLoginRequest(LoginDTO request)
        {
            if (!CorrectEmai(request.Email))
            {
                return (false, "Невалидное значение Email");
            }

            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (userInDb == null)
            {
                return (false, "Пользователя с таким Email не существует");
            }

            return (true, null);
        }
    }
}
