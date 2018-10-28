namespace Torshia.Services
{
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;

    public class UserService : IUserService
    {
        public void AddUser(string username, string password, string email, Role role)
        {
            using (var db = new TorshiaDbContext())
            {
                var user = new User
                {
                    Username = username,
                    Password = password,
                    Email = email,
                    Role = role
                };

                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public User GetUser(string username)
        {
            using (var db = new TorshiaDbContext())
            {
                var user = db
                    .Users
                    .FirstOrDefault(u => u.Username == username);

                return user;
            }
        }

        public User GetUser(string username, string password)
        {
            using (var db = new TorshiaDbContext())
            {
                var user = db
                    .Users
                    .FirstOrDefault(u => u.Username == username && u.Password == password);

                return user;
            }
        }

        public bool IsUserExist(string username)
        {
            throw new System.NotImplementedException();
        }

        public bool AreThereUsers()
        {
            using (var db = new TorshiaDbContext())
            {
                return db.Users.Any();
            }
        }
    }
}