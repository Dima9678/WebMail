using Server.Controllers;

namespace Server.Normaizators
{
    public static class InputNormalizator
    {
        public static RegisterDTO NormalizeRegisterDTO(RegisterDTO dto)
        {
            dto.Name = dto.Name.Trim();
            dto.Email = dto.Email.Trim();
            dto.Email = dto.Email.ToLower();
            dto.Password = dto.Password.Trim();
            dto.RepeatPassword = dto.Password.Trim();

            dto.Email += "@mymail.com";

            return dto;
        }
        public static LoginDTO NormalizeLoginDTO(LoginDTO dto)
        {
            dto.Email = dto.Email.Trim();
            dto.Email = dto.Email.ToLower();
            dto.Password = dto.Password.Trim();

            return dto;
        }
    }
}
