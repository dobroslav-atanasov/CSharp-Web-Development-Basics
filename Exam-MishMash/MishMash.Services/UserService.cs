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

        public User AddUser(string username, string password, string email)
        {
            using (var db = new MishMashDbContext())
            {
                var user = new User
                {
                    Username = username,
                    Password = password,
                    Email = email,
                    Role = Role.User
                };

                if (!db.Users.Any())
                {
                    user.Role = Role.Admin;
                }

                db.Users.Add(user);
                db.SaveChanges();

                return user;
            }
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            using (var db = new MishMashDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var db = new MishMashDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Username == username);
            }
        }
    }
}