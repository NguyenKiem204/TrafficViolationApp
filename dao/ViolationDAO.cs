using TrafficViolationApp.model; // Import the Violation model
using Microsoft.EntityFrameworkCore; // To use Entity Framework functionality

namespace TrafficViolationApp.dao
{
    class ViolationDAO : IDAOInterface<Violation, int>
    {
        private readonly TrafficDBContext context;

        public ViolationDAO()
        {
            context = new TrafficDBContext(); // Assuming TrafficDBContext is your EF DbContext
        }

        // Fetch violations by userId (ViolatorId)
        public List<Violation> selectByUserId(int userId)
        {
            // Retrieve violations for the given userId (ViolatorId)
            return context.Violations
                .Where(v => v.ViolatorId == userId) // Filter by ViolatorId
                .Include(v => v.Report) // Include Report to avoid lazy loading issues
                .Include(v => v.Violator) // Include Violator to load the User (violator)
                .ToList();
        }

        // Other CRUD methods (delete, insert, selectAll, selectById) remain the same

        public int delete(Violation t)
        {
            var existingViolation = context.Violations.Find(t.ViolationId);
            if (existingViolation == null) return 0;
            context.Violations.Remove(t);
            return context.SaveChanges();
        }

        public int insert(Violation t)
        {
            context.Violations.Add(t);
            return context.SaveChanges();
        }

        public List<Violation> selectAll()
        {
            return context.Violations.ToList();
        }

        public Violation? selectById(int id)
        {
            return context.Violations.Find(id);
        }

        public int update(Violation t)
        {
            var existingViolation = context.Violations.Find(t.ViolationId);
            if (existingViolation == null) return 0;

            existingViolation.PlateNumber = t.PlateNumber;
            existingViolation.FineAmount = t.FineAmount;
            existingViolation.FineDate = t.FineDate;
            existingViolation.PaidStatus = t.PaidStatus;
            existingViolation.Description = t.Description;
            existingViolation.ViolationType = t.ViolationType;

            return context.SaveChanges();
        }
    }
}
