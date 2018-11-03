namespace MishMash.Models
{
    using System.Collections.Generic;
    using Enums;

    public class Channel
    {
        public Channel()
        {
            this.Tags = new List<Tag>();
            this.Users = new List<UserChannel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Type Type { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<UserChannel> Users { get; set; }
    }
}