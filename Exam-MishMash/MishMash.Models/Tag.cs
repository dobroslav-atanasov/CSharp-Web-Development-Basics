namespace MishMash.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}