using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficViolationApp.model
{
    public class User
    {
        private int userID;
        private string fullName;
        private string email;
        private string password;
        private string role;
        private string phone;
        private string address;

        public User() { }

        public User(int userID, string fullName, string email, string password, string role, string phone, string address)
        {
            this.UserID = userID;
            this.FullName = fullName;
            this.Email = email;
            this.Password = password;
            this.Role = role;
            this.Phone = phone;
            this.Address = address;
        }

        public int UserID { get => userID; set => userID = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Role { get => role; set => role = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Address { get => address; set => address = value; }
    }
}
