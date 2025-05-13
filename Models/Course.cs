using System;

namespace StudentManagementSystem.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProfessorId { get; set; }
        
        // Navigation properties
        public string DepartmentName { get; set; }
        public string ProfessorName { get; set; }
    }
}
