using System;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public int? DepartmentId { get; set; }
        
        // Navigation properties
        public string DepartmentName { get; set; }
    }
}
