namespace MishMash.Services.Contracts
{
    using Models;
    using Models.Enums;

    public interface IUserService
    {
        bool IsUserExists(string username);

        User AddUser(string username, string password, string email);

        User GetUserByUsernameAndPassword(string username, string password);

        User GetUserByUsername(string username);
    }
}