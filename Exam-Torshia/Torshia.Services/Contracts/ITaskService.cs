namespace Torshia.Services.Contracts
{
    using System;
    using Models;

    public interface ITaskService
    {
        Task AddTask(string title, DateTime dueDate, string description, string participants);
        
        void AddTaskSectors(string sectorCustomers, string sectorMarketing, string sectorFinances, string sectorInternal, string sectorManagement, int taskId);

        int GetTaskLevel(int taskId);
    }
}