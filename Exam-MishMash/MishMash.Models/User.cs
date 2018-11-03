namespace MishMash.Models
{
    using System.Collections.Generic;
    using Enums;

    public class User
    {
        public User()
        {
            this.Channels = new List<UserChannel>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public ICollection<UserChannel> Channels { get; set; }

        public Role Role { get; set; }
    }
}