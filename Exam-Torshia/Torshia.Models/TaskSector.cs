namespace Torshia.Models
{
    using Enums;

    public class TaskSector
    {
        public int Id { get; set; }

        public Sector Sector { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}