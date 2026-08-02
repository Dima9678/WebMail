using Microsoft.EntityFrameworkCore;
using Persistence;
using Domain.Models.Requests;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Server.Validators
{
    public class ValidationCheck
    {
        private DatabaseContext _db { get; set; }
        public ValidationCheck(DatabaseContext db)
        {
            _db = db;
        }
        public OperationResult EqualInputPasswords(string firstPassword, string secondPassword)
        {
            if (firstPassword != secondPassword)
            {
                return new OperationResult()
                {
                    ErrorMessage = "Пароли не совпадают",
                    Sucsessed = false,
                };
            }
            return new OperationResult();
        }
        public OperationResult CorrectEmail(string email)
        {
            if (email.Length <= 11)
            {
                return new OperationResult()
                {
                    ErrorMessage = "Недостаточная длина почты",
                    Sucsessed = false,
                };
            }

            else if (email[^11..^0] != "@mymail.com")
            {
                return new OperationResult()
                {
                    ErrorMessage = "Некорректный Email",
                    Sucsessed = false,
                };
            }

            return new OperationResult();
        }
        public OperationResult PasswordLength(string password)
        {
            if (password.Length < 8)
            {
                return new OperationResult()
                {
                    ErrorMessage = "лина пароля должна быть не менее 8 символов",
                    Sucsessed = false,
                };
            }
            return new OperationResult();
        }
        public async Task<OperationResult> ValidateWriteLetterRequest(NewLetterDTO request)
        {
            OperationResult emailValResult = CorrectEmail(request.Recipient);
            if (string.IsNullOrWhiteSpace(request.Recipient))
            {
                return new OperationResult()
                {
                    ErrorMessage = "Не введен получатель",
                    Sucsessed = false,
                };
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new OperationResult()
                {
                    ErrorMessage = "Тема письма не может быть пустой",
                    Sucsessed = false,
                };
            }

            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return new OperationResult()
                {
                    ErrorMessage = "Текст письма не может быть пустым",
                    Sucsessed = false,
                };
            }

            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Recipient);
            if (userInDb == null)
            {
                return new OperationResult()
                {
                    ErrorMessage = "Такого пользователя не существует",
                    Sucsessed = false,
                };
            }

            return new OperationResult();
        }

        public async Task<OperationResult> ValidateReplyRequest(ReplyDTO reply)
        {
            OperationResult result = new OperationResult();
            if (string.IsNullOrWhiteSpace(reply.ReplyText))
            {
                result.Sucsessed = false;
                result.ErrorMessage = "Текст письма не может быть пустым";

                return result;
            }
            result.Sucsessed = true;
            return result;
        }
        public async Task<OperationResult> ValidateWriteDraftRequest(NewDraftDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.RecipientEmail) &&
                string.IsNullOrWhiteSpace(request.Title) &&
                string.IsNullOrWhiteSpace(request.Text)
                )
            {
                return new OperationResult()
                {
                    ErrorMessage = "Для созранения черновика должно быть хотя-бы одно значение",
                    Sucsessed = false,
                };
            }

            return new OperationResult();
        }
        public async Task<OperationResult> ValidateRegisterRequest(RegisterDTO request)
        {
            OperationResult equalPasswordsResult = EqualInputPasswords(request.Password, request.RepeatPassword);
            if (!equalPasswordsResult.Sucsessed)
            {
                return new OperationResult()
                {
                    ErrorMessage = "Пароли не совпадают",
                    Sucsessed = false,
                };
            }

            OperationResult pwLength = PasswordLength(request.Password);
            if (!pwLength.Sucsessed)
            {
                return new OperationResult()
                {
                    ErrorMessage = "Длина пароля должна быть больше либо равна 8 символам",
                    Sucsessed = false,
                };
            }

            //поиск юзера
            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (userInDb != null)
            {
                return new OperationResult()
                {
                    ErrorMessage = "Пользователь с таким Email уже существует",
                    Sucsessed = false,
                };
            }

            return new OperationResult();
        }
        public async Task<OperationResult> ValidateLoginRequest(LoginDTO request)
        {
            OperationResult emailVal = CorrectEmail(request.Email);
            if (!emailVal.Sucsessed)
            {
                emailVal.ErrorMessage = "Невалидное значение Email";
                return emailVal;
            }

            var userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (userInDb == null)
            {
                return new OperationResult()
                {
                    Sucsessed = false,
                    ErrorMessage = "Пользователя с таким Email не существует",
                };
            }

            return new OperationResult();
        }
    }
}
