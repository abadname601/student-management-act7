using System;

namespace StudentManagementSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string RecoveryQuestion { get; set; }
        public string RecoveryAnswer { get; set; }
    }
}
