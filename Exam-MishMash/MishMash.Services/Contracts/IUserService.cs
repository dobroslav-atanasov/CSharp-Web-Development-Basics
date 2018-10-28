namespace MishMash.Services.Contracts
{
    using Models.Enums;

    public interface IUserService
    {
        bool IsUserExists(string username);

        void AddUser(string username, string password, string email, Role role);

        bool HaveUsers();
    }
}