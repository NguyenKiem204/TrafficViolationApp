using TrafficViolationApp.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficViolationApp.dao
{
    class NotificationDAO : IDAOInterface<Notification, int>
    {
        private readonly TrafficDBContext context;

        public NotificationDAO()
        {
            context = new TrafficDBContext();
        }

        public int delete(Notification t)
        {
            var existingNotification = context.Notifications.Find(t.NotificationId);
            if (existingNotification == null) return 0;
            context.Notifications.Remove(t);
            return context.SaveChanges();
        }

        public int insert(Notification t)
        {
            t.SentDate = DateTime.Now;
            t.IsRead = false;
            context.Notifications.Add(t);
            return context.SaveChanges();
        }

        public List<Notification> selectAll()
        {
            return context.Notifications
                .Include(n => n.User)
                .ToList();
        }

        public Notification? selectById(int id)
        {
            return context.Notifications
                .Include(n => n.User)
                .FirstOrDefault(n => n.NotificationId == id);
        }

        public int update(Notification t)
        {
            var existingNotification = context.Notifications.Find(t.NotificationId);
            if (existingNotification == null) return 0;

            existingNotification.Message = t.Message;
            existingNotification.IsRead = t.IsRead;

            return context.SaveChanges();
        }

        public List<Notification> selectByUserId(int userId)
        {
            return context.Notifications
                .Include(n => n.User)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.SentDate)
                .ToList();
        }

        public List<Notification> selectByPlateNumber(string plateNumber)
        {
            return context.Notifications
                .Include(n => n.User)
                .Where(n => n.PlateNumber == plateNumber)
                .OrderByDescending(n => n.SentDate)
                .ToList();
        }
    }
}