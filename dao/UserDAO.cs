using TrafficViolationApp.model;
using Microsoft.Data.SqlClient;
using BCrypt.Net;
using System.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using TrafficViolationApp.service;

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
            context.Users.Remove(existingUser);
            return context.SaveChanges();
        }

        public int insert(User t)
        {
            t.Password = BCrypt.Net.BCrypt.HashPassword(t.Password);
            t.OtpCode = OtpService.GenerateOtp();
            t.OtpExpiry = OtpService.GetExpiryTime();

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

        public User? selectByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email == email);
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
            existingUser.OtpCode = t.OtpCode;
            existingUser.OtpExpiry = t.OtpExpiry;

            if (!string.IsNullOrEmpty(t.Password.Trim()))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(t.Password);
            }

            return context.SaveChanges();
        }

        public (User? user, string message) CheckLogin(string email, string password)
        {
            var existingUser = context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser == null)
                return (null, "Email hoặc mật khẩu không đúng!");

            if (!existingUser.IsEmailVerified)
                return (null, "Tài khoản chưa xác thực email. Vui lòng kiểm tra email để xác nhận.");

            if (!BCrypt.Net.BCrypt.Verify(password, existingUser.Password))
                return (null, "Email hoặc mật khẩu không đúng!");

            return (existingUser, "Đăng nhập thành công!");
        }



        public bool VerifyEmail(string email, string otp)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;

            if (user.OtpCode == otp && user.OtpExpiry > DateTime.UtcNow)
            {
                user.IsEmailVerified = true;
                user.OtpCode = null;
                user.OtpExpiry = null;
                return context.SaveChanges() > 0;
            }

            return false;
        }

        public bool ResendVerificationEmail(string email)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email && !u.IsEmailVerified);
            if (user == null) return false;

            user.OtpCode = OtpService.GenerateOtp();
            user.OtpExpiry = OtpService.GetExpiryTime();

            return context.SaveChanges() > 0;
        }

        public bool emailExists(string email)
        {
            return context.Users.Any(u => u.Email == email);
        }
    }
}
