namespace Torshia.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Models.Enums;

    public interface IReportService
    {
        void AddReport(DateTime date, Status status, int taskId, int userId);

        Report GetReport();

        List<Report> GetAllReports();
    }
}