using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TrafficViolationApp.dao
{
    public class DbContext
    {
        private const string ConnectionString = @"Server=NGUYENKIEM\SQLEXPRESS;Database=TrafficViolationApp;User Id=sa;Password=123456789;TrustServerCertificate=True;";
        public SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Kết nối thành công!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi kết nối: {ex.Message}");
                Console.WriteLine($"Chi tiết: {ex.StackTrace}");

                return false;
            }

        }
    }
}
