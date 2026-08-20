using Domain.Models.Requests;

namespace Domain.Intefraces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterDTO dto);
        Task<User> LoginAsync(LoginDTO dto);
    }
}