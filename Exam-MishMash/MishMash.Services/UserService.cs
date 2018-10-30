namespace MishMash.Services
{
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;

    public class UserService : IUserService
    {
        public bool IsUserExists(string username)
        {
            using (var db = new MishMashDbContext())
            {
                return db.Users.Any(u => u.Username == username);
            }
        }

        public void AddUser(string username, string password, string email, Role role)
        {
            using (var db = new MishMashDbContext())
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

        public bool HaveUsers()
        {
            using (var db = new MishMashDbContext())
            {
                return db.Users.Any();
            }
        }

        public User GetUser(string username, string password)
        {
            using (var db = new MishMashDbContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.Username == username && u.Password == password);

                return user;
            }
        }

        public User GetUser(string username)
        {
            using (var db = new MishMashDbContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.Username == username);

                return user;
            }
        }
    }
}