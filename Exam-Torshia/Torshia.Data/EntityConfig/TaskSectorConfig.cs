namespace Torshia.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class TaskSectorConfig : IEntityTypeConfiguration<TaskSector>
    {
        public void Configure(EntityTypeBuilder<TaskSector> builder)
        {
            builder.HasOne(ts => ts.Task)
                .WithMany(t => t.AffectedSectors)
                .HasForeignKey(ts => ts.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}