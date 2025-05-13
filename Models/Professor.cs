using System;

namespace StudentManagementSystem.Models
{
    public class Professor
    {
        public int ProfessorId { get; set; }
        public string ProfessorName { get; set; }
        public int? DepartmentId { get; set; }
        
        // Navigation properties
        public string DepartmentName { get; set; }
    }
}
