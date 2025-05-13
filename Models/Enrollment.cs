using System;

namespace StudentManagementSystem.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        
        // Navigation properties
        public string StudentName { get; set; }
        public string CourseName { get; set; }
    }
}
