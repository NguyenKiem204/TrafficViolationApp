using TrafficViolationApp.model;

namespace TrafficViolationApp.dao
{
    public class ReportDAO : IDAOInterface<Report, int>
    {
        private readonly TrafficDBContext context;
        public ReportDAO()
        {
            context = new TrafficDBContext();
        }
        public int delete(Report report)
        {
            var existedReport = context.Reports.Find(report);
            if (existedReport == null) return 0;
            context.Reports.Remove(existedReport);
            return context.SaveChanges();
        }

        public int insert(Report report)
        {
            context.Reports.Add(report);
            return context.SaveChanges();
        }

        public List<Report> selectAll()
        {
            return context.Reports.ToList();
        }

        public Report? selectById(int id)
        {
            return context.Reports.Find(id);
        }

        public int update(Report report)
        {
            var existedReport = context.Reports.Find(report.ReportId);
            if (existedReport == null) return 0;
            context.Entry(existedReport).CurrentValues.SetValues(report);
            return context.SaveChanges();
        }
    }
}
