using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficViolationApp.model
{
    public class Report
    {
        private int reportID;
        private int reporterID;
        private string violationType;
        private string description;
        private string plateNumber;
        private string imageURL;
        private string videoURL;
        private string location;
        private string reportDate;
        private string status;
        private int processedBy;

        public Report() { }

        public Report(int reportID, int reporterID, string violationType, string description, string plateNumber, string imageURL, string videoURL, string location, string reportDate, string status, int processedBy)
        {
            this.ReportID = reportID;
            this.ReporterID = reporterID;
            this.ViolationType = violationType;
            this.Description = description;
            this.PlateNumber = plateNumber;
            this.ImageURL = imageURL;
            this.VideoURL = videoURL;
            this.Location = location;
            this.ReportDate = reportDate;
            this.Status = status;
            this.ProcessedBy = processedBy;
        }

        public int ReportID { get => reportID; set => reportID = value; }
        public int ReporterID { get => reporterID; set => reporterID = value; }
        public string ViolationType { get => violationType; set => violationType = value; }
        public string Description { get => description; set => description = value; }
        public string PlateNumber { get => plateNumber; set => plateNumber = value; }
        public string ImageURL { get => imageURL; set => imageURL = value; }
        public string VideoURL { get => videoURL; set => videoURL = value; }
        public string Location { get => location; set => location = value; }
        public string ReportDate { get => reportDate; set => reportDate = value; }
        public string Status { get => status; set => status = value; }
        public int ProcessedBy { get => processedBy; set => processedBy = value; }
    }
}
