namespace Torshia.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enums;

    public class ReportService : IReportService
    {
        public void AddReport(DateTime date, Status status, int taskId, int userId)
        {
            using (var db = new TorshiaDbContext())
            {
                var reporter = new Report
                {
                    ReportedOn = date,
                    Status = status,
                    TaskId = taskId,
                    ReporterId = userId
                };

                db.Reports.Add(reporter);
                db.SaveChanges();
            }
        }

        public Report GetReport()
        {
            using (var db = new TorshiaDbContext())
            {
                return db.Reports
                    .Include(r => r.Task)
                    .First();
            }
        }

        public List<Report> GetAllReports()
        {
            using (var db = new TorshiaDbContext())
            {
                var reports = db.Reports
                    .Include(r => r.Task)
                    .ToList();

                return reports;
            }
        }
    }
}