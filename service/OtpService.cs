using System;

namespace TrafficViolationApp.service
{
    public static class OtpService
    {
        private static readonly Random random = new Random();

        public static string GenerateOtp(int length = 6)
        {
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10).ToString();
            }
            return otp;
        }

        public static DateTime GetExpiryTime(int minutes = 10)
        {
            return DateTime.UtcNow.AddMinutes(minutes);
        }

        public static bool IsOtpValid(string inputOtp, string storedOtp, DateTime? expiryTime)
        {
            if (string.IsNullOrEmpty(inputOtp) || string.IsNullOrEmpty(storedOtp) || !expiryTime.HasValue)
                return false;

            return inputOtp == storedOtp && DateTime.UtcNow <= expiryTime.Value;
        }
    }
}