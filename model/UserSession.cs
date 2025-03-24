using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficViolationApp.model
{
    public class UserSession
    {
        private static UserSession? _instance;
        private static readonly object _lock = new object();

        public User? User { get; set; }
        private UserSession() { }
        public static UserSession Instance
        {
            get
            {
                lock (_lock)
                {
                    _instance ??= new UserSession();
                    return _instance;
                }
            }
        }
        public void Logout()
        {
            User = null;
        }
        public void Reset()
        {
            User = null;
            _instance = null;
        }

    }


}
