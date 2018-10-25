﻿namespace Torshia.Models
{
    using System;
    using System.Collections.Generic;

    public class Task
    {
        public Task()
        {
            this.AffectedSectors = new List<TaskSector>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsReported { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public virtual ICollection<TaskSector> AffectedSectors { get; set; }
    }
}