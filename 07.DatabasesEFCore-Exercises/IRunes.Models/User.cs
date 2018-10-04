namespace IRunes.Models
{
    using System.Collections.Generic;

    public class User : BaseModel<int>
    {
        public User()
        {
            this.Albums = new HashSet<Album>();
            this.Tracks = new HashSet<Track>();
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}