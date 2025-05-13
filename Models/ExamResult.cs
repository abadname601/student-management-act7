using System;

namespace StudentManagementSystem.Models
{
    public class ExamResult
    {
        public int ResultId { get; set; }
        public int? StudentId { get; set; }
        public int? ExamId { get; set; }
        public decimal Score { get; set; }
        public string ExamName { get; set; }
        
        // Navigation properties
        public string StudentName { get; set; }
    }
}
