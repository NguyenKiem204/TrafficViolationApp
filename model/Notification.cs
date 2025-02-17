using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrafficViolationApp.model
{
    public class Notification
    {
        private int notificationID;
        private int userID;
        private string message;
        private string plateNumber;
        private string sentDate;
        private bool isRead;
        public Notification() { }

        public Notification(int notificationID, int userID, string message, string plateNumber, string sentDate, bool isRead)
        {
            this.NotificationID = notificationID;
            this.UserID = userID;
            this.Message = message;
            this.PlateNumber = plateNumber;
            this.SentDate = sentDate;
            this.IsRead = isRead;
        }

        public int NotificationID { get => notificationID; set => notificationID = value; }
        public int UserID { get => userID; set => userID = value; }
        public string Message { get => message; set => message = value; }
        public string PlateNumber { get => plateNumber; set => plateNumber = value; }
        public string SentDate { get => sentDate; set => sentDate = value; }
        public bool IsRead { get => isRead; set => isRead = value; }
    }
}
