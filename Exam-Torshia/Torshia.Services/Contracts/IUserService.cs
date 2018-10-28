namespace Torshia.Services.Contracts
{
    using Models;
    using Models.Enums;

    public interface IUserService
    {
        void AddUser(string username, string password, string email, Role role);

        User GetUser(string username);

        User GetUser(string username, string password);

        bool IsUserExist(string username);

        bool AreThereUsers();
    }
}