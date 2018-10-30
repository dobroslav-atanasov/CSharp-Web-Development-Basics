namespace MishMash.Services.Contracts
{
    using Models;
    using Models.Enums;

    public interface IUserService
    {
        bool IsUserExists(string username);

        void AddUser(string username, string password, string email, Role role);

        bool HaveUsers();

        User GetUser(string username, string password);

        User GetUser(string username);
    }
}