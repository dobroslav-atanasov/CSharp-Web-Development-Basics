namespace Chushka.Services
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
            using (var db = new ChushkaDbContext())
            {
                return db.Users.Any(u => u.Username == username);
            }
        }

        public void AddUser(string username, string password, string fullName, string email)
        {
            using (var db = new ChushkaDbContext())
            {
                var user = new User
                {
                    Username = username,
                    Password = password,
                    FullName = fullName,
                    Email = email,
                    Role = Role.User
                };

                if (!db.Users.Any())
                {
                    user.Role = Role.Admin;
                }

                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var db = new ChushkaDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Username == username);
            }
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            using (var db = new ChushkaDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
        }
    }
}