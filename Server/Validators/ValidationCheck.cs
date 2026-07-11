using System.Security.Cryptography.X509Certificates;

namespace Server.Validators
{
    public class ValidationCheck
    {
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
    }
}
