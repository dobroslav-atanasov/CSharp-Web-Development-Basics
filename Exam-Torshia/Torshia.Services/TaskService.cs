namespace Torshia.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;

    public class TaskService : ITaskService
    {
        public Task AddTask(string title, DateTime dueDate, string description, string participants)
        {
            using (var db = new TorshiaDbContext())
            {
                var task = new Task
                {
                    Title = title,
                    DueDate = dueDate,
                    Description = description,
                    Participants = participants
                };

                db.Tasks.Add(task);
                db.SaveChanges();

                return task;
            }
        }

        public void AddTaskSectors(string sectorCustomers, string sectorMarketing, string sectorFinances, string sectorInternal, string sectorManagement, int taskId)
        {
            var sectors = new List<string>
            {
                sectorCustomers,
                sectorMarketing,
                sectorFinances,
                sectorInternal,
                sectorManagement
            };
            
            using (var db = new TorshiaDbContext())
            {
                foreach (var sectorString in sectors)
                {
                    if (Enum.TryParse<Sector>(sectorString, true, out var sector))
                    {
                        var taskSector = new TaskSector
                        {
                            TaskId = taskId,
                            Sector = sector
                        };

                        db.TaskSectors.Add(taskSector);
                        db.SaveChanges();
                    }
                }
            }
        }

        public int GetTaskLevel(int taskId)
        {
            using (var db = new TorshiaDbContext())
            {
                var level = db.TaskSectors
                    .Where(t => t.TaskId == taskId)
                    .Count();

                return level;
            }
        }

        public string GetAllAffectedSectors(int taskId)
        {
            using (var db = new TorshiaDbContext())
            {
                var taskSectors = db.TaskSectors
                    .Where(ts => ts.TaskId == taskId)
                    .Select(ts => ts.Sector.ToString())
                    .ToList();

                return string.Join(", ", taskSectors);
            }
        }

        public void ChangeIsReported(int taskId)
        {
            using (var db = new TorshiaDbContext())
            {
                var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);

                task.IsReported = true;

                db.SaveChanges();
            }
        }

        public List<Task> GetAllNonReportedTasks()
        {
            using (var db = new TorshiaDbContext())
            {
                var tasks = db.Tasks
                    .Where(t => t.IsReported == false)
                    .ToList();

                return tasks;
            }
        }
    }
}