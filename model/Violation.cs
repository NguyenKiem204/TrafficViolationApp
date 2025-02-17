using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrafficViolationApp.model
{
    public class Violation
    {
        private int violationID;
        private int reportID;
        private string plateNumber;
        private int violatorID;
        private double fineAmount;
        private string fineDate;
        private bool paidStatus;
        public Violation() { }

        public Violation(int violationID, int reportID, string plateNumber, int violatorID, double fineAmount, string fineDate, bool paidStatus)
        {
            this.ViolationID = violationID;
            this.ReportID = reportID;
            this.PlateNumber = plateNumber;
            this.ViolatorID = violatorID;
            this.FineAmount = fineAmount;
            this.FineDate = fineDate;
            this.PaidStatus = paidStatus;
        }

        public int ViolationID { get => violationID; set => violationID = value; }
        public int ReportID { get => reportID; set => reportID = value; }
        public string PlateNumber { get => plateNumber; set => plateNumber = value; }
        public int ViolatorID { get => violatorID; set => violatorID = value; }
        public double FineAmount { get => fineAmount; set => fineAmount = value; }
        public string FineDate { get => fineDate; set => fineDate = value; }
        public bool PaidStatus { get => paidStatus; set => paidStatus = value; }
    }
}
