using TrafficViolationApp.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficViolationApp.dao
{
    class ViolationDAO : IDAOInterface<Violation, int>
    {
        private readonly TrafficDBContext context;

        public ViolationDAO()
        {
            context = new TrafficDBContext();
        }

        public int delete(Violation t)
        {
            var existingViolation = context.Violations.Find(t.ViolationId);
            if (existingViolation == null) return 0;
            context.Violations.Remove(t);
            return context.SaveChanges();
        }

        public int insert(Violation t)
        {
            t.FineDate = DateTime.Now;
            t.PaidStatus = false;
            context.Violations.Add(t);
            return context.SaveChanges();
        }

        public List<Violation> selectAll()
        {
            return context.Violations
                .Include(v => v.Report)
                .Include(v => v.Violator)
                .ToList();
        }

        public Violation? selectById(int id)
        {
            return context.Violations
                .Include(v => v.Report)
                .Include(v => v.Violator)
                .FirstOrDefault(v => v.ViolationId == id);
        }

        public int update(Violation t)
        {
            var existingViolation = context.Violations.Find(t.ViolationId);
            if (existingViolation == null) return 0;

            existingViolation.FineAmount = t.FineAmount;
            existingViolation.PaidStatus = t.PaidStatus;
            existingViolation.ViolatorId = t.ViolatorId;

            return context.SaveChanges();
        }

        public List<Violation> selectByReportId(int reportId)
        {
            return context.Violations
                .Include(v => v.Report)
                .Include(v => v.Violator)
                .Where(v => v.ReportId == reportId)
                .ToList();
        }

        public List<Violation> selectByPlateNumber(string plateNumber)
        {
            return context.Violations
                .Include(v => v.Report)
                .Include(v => v.Violator)
                .Where(v => v.PlateNumber == plateNumber)
                .ToList();
        }
    }
}