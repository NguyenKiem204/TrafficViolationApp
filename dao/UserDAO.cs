using TrafficViolationApp.model;
using Microsoft.Data.SqlClient;
using BCrypt.Net;
using TrafficViolationApp.dao;
using System.Data;
using System.Net;
using System;
using System.Windows;

namespace TrafficViolationApp.dao
{ 
    class UserDAO : IDAOInterface<User, int>
    {
        private readonly TrafficDBContext context;
        public UserDAO()
        {
            context = new TrafficDBContext();
        }

        public int delete(User t)
        {
            var existingUser = context.Users.Find(t.UserId);
            if (existingUser == null) return 0;
            context.Users.Remove(t);
            return context.SaveChanges();

        }

        public int insert(User t)
        {
            t.Password = BCrypt.Net.BCrypt.HashPassword(t.Password);
            context.Users.Add(t);
            return context.SaveChanges();
        }

        public List<User> selectAll()
        {
            return context.Users.ToList();
        }

        public User? selectById(int id)
        {
            return context.Users.Find(id);
        }

        public int update(User t)
        {
            var existingUser = context.Users.Find(t.UserId);
            if (existingUser == null) return 0;

            existingUser.FullName = t.FullName;
            existingUser.Email = t.Email;
            existingUser.Role = t.Role;
            existingUser.Phone = t.Phone;
            existingUser.Address = t.Address;

            if (!string.IsNullOrEmpty(t.Password.Trim()))
            {
               MessageBox.Show("Password not empty! "+ t.Password, "Validate Error", MessageBoxButton.OK, MessageBoxImage.Error);
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(t.Password);
            }

            return context.SaveChanges();
        }
        public User? CheckLogin(string email, string password)
        {
            var existingUser = context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser == null) return null;

            if (BCrypt.Net.BCrypt.Verify(password, existingUser.Password))
                return existingUser;

            return null;
        }

    }
}
