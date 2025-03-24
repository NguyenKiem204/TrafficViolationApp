using TrafficViolationApp.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficViolationApp.dao
{
    class ReportDAO : IDAOInterface<Report, int>
    {
        private readonly TrafficDBContext context;

        public ReportDAO()
        {
            context = new TrafficDBContext();
        }

        public int delete(Report t)
        {
            var existingReport = context.Reports.Find(t.ReportId);
            if (existingReport == null) return 0;
            context.Reports.Remove(t);
            return context.SaveChanges();
        }

        public int insert(Report t)
        {
            t.ReportDate = DateTime.Now;
            t.Status = "Pending";
            context.Reports.Add(t);
            return context.SaveChanges();
        }

        public List<Report> selectAll()
        {
            return context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ProcessedByNavigation)
                .ToList();
        }

        public Report? selectById(int id)
        {
            return context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ProcessedByNavigation)
                .FirstOrDefault(r => r.ReportId == id);
        }

        public int update(Report t)
        {
            var existingReport = context.Reports.Find(t.ReportId);
            if (existingReport == null) return 0;

            existingReport.ViolationType = t.ViolationType;
            existingReport.Description = t.Description;
            existingReport.PlateNumber = t.PlateNumber;
            existingReport.ImageUrl = t.ImageUrl;
            existingReport.VideoUrl = t.VideoUrl;
            existingReport.Location = t.Location;
            existingReport.Status = t.Status;
            existingReport.ProcessedBy = t.ProcessedBy;

            return context.SaveChanges();
        }

        public List<Report> selectByStatus(string status)
        {
            return context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ProcessedByNavigation)
                .Where(r => r.Status == status)
                .ToList();
        }

        public List<Report> searchReports(string searchTerm)
        {
            return context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ProcessedByNavigation)
                .Where(r => r.PlateNumber.Contains(searchTerm) ||
                           r.Location.Contains(searchTerm) ||
                           r.ViolationType.Contains(searchTerm) ||
                           r.Reporter.FullName.Contains(searchTerm))
                .ToList();
        }

        public int updateStatus(int reportId, string status, int processedBy)
        {
            var report = context.Reports.Find(reportId);
            if (report == null) return 0;

            report.Status = status;
            report.ProcessedBy = processedBy;

            return context.SaveChanges();
        }
    }
}
