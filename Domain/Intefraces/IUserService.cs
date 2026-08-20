namespace Domain.Intefraces
{
    public interface IUserService
    {
        Task<UserDTO> Get(Guid userId);
        Task ChangeUserInfo(UserDataChangeRequest request, Guid userId);
        Task AddSpamEmail(Guid userId, string adresseeEmail);
        Task RemoveSpamEmail(Guid userId, string adresseeEmail);
    }
}