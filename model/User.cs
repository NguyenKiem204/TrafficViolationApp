using System;
using System.Collections.Generic;

namespace TrafficViolationApp.model;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }
    public bool IsEmailVerified { get; set; } = false;
    public string? OtpCode { get; set; }
    public DateTime? OtpExpiry { get; set; }
    public int FailedOtpAttempts { get; set; } = 0;
    public User()
    {
    }

    public User(int userId, string fullName, string email, string password, string role, string phone, string? address)
    {
        UserId = userId;
        FullName = fullName;
        Email = email;
        Password = password;
        Role = role;
        Phone = phone;
        Address = address;
    }
    public User(string fullName, string email, string password, string role, string phone, string? address)
    {
        FullName = fullName;
        Email = email;
        Password = password;
        Role = role;
        Phone = phone;
        Address = address;
    }

    public User(int userId, string fullName, string email, string password, string role, string phone, string? address, bool isEmailVerified, string? otpCode, DateTime? otpExpiry, int failedOtpAttempts) : this(userId, fullName, email, password, role, phone, address)
    {
        IsEmailVerified = isEmailVerified;
        OtpCode = otpCode;
        OtpExpiry = otpExpiry;
        FailedOtpAttempts = failedOtpAttempts;
    }

    public override string? ToString()
    {
        return $"User({UserId} - {FullName} - {Email} - {Password} - {Role} - {Phone} - {Address})";
    }

    public string GetInitials()
    {
        string[] words = FullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (words.Length >= 1)
        {
            string firstWord = words[0];
            string lastWord = words[^1];

            return $"{char.ToUpper(firstWord[0])}{char.ToUpper(lastWord[0])}";
        }

        return "";
    }
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Report> ReportProcessedByNavigations { get; set; } = new List<Report>();

    public virtual ICollection<Report> ReportReporters { get; set; } = new List<Report>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
