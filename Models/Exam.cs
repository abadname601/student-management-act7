using System;

namespace StudentManagementSystem.Models
{
    public class Exam
    {
        public int ExamId { get; set; }
        public int? CourseId { get; set; }
        public DateTime ExamDate { get; set; }
        public string Description { get; set; }
        
        // Navigation properties
        public string CourseName { get; set; }
        
        // Display property for combo box
        public string ExamIdWithCourse { get; set; }
    }
}
