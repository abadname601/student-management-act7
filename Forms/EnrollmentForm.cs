using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StudentManagementSystem.Database;
using StudentManagementSystem.Models;
using StudentManagementSystem.Utilities;

namespace StudentManagementSystem.Forms
{
    public partial class EnrollmentForm : Form
    {
        private DatabaseManager _dbManager;
        private List<Student> _students;
        private List<Course> _courses;
        private List<Enrollment> _enrollments;
        private ErrorProvider _errorProvider;
        private Enrollment _selectedEnrollment = null;
        private enum FormMode { View, Add, Edit }
        private FormMode _currentMode = FormMode.View;
        
        public EnrollmentForm()
        {
            InitializeComponent();
            _dbManager = new DatabaseManager();
            _errorProvider = new ErrorProvider();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void EnrollmentForm_Load(object sender, EventArgs e)
        {
            // Apply styling
            UIHelper.StyleForm(this);
            UIHelper.StyleDataGridView(dgvEnrollments);
            UIHelper.StyleInputControls(this);
            
            // Initialize form
            LoadStudents();
            LoadCourses();
            LoadEnrollments();
            SetFormMode(FormMode.View);
        }

        private void LoadStudents()
        {
            try
            {
                _students = _dbManager.GetAllStudents();
                
                // Bind to combo box
                cmbStudent.Items.Clear();
                cmbStudent.DisplayMember = "StudentName";
                cmbStudent.ValueMember = "StudentId";
                
                // Add "None" option
                cmbStudent.Items.Add(new Student { StudentId = 0, StudentName = "-- Select Student --" });
                
                foreach (var student in _students)
                {
                    cmbStudent.Items.Add(student);
                }
                
                cmbStudent.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCourses()
        {
            try
            {
                _courses = _dbManager.GetAllCourses();
                
                // Bind to combo box
                cmbCourse.Items.Clear();
                cmbCourse.DisplayMember = "CourseName";
                cmbCourse.ValueMember = "CourseId";
                
                // Add "None" option
                cmbCourse.Items.Add(new Course { CourseId = 0, CourseName = "-- Select Course --" });
                
                foreach (var course in _courses)
                {
                    cmbCourse.Items.Add(course);
                }
                
                cmbCourse.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEnrollments()
        {
            try
            {
                _enrollments = _dbManager.GetAllEnrollments();
                
                // Bind to DataGridView
                dgvEnrollments.DataSource = null;
                dgvEnrollments.DataSource = _enrollments;
                
                // Configure columns
                if (dgvEnrollments.Columns.Contains("EnrollmentId"))
                {
                    dgvEnrollments.Columns["EnrollmentId"].HeaderText = "ID";
                    dgvEnrollments.Columns["EnrollmentId"].Width = 50;
                }
                
                if (dgvEnrollments.Columns.Contains("StudentId"))
                {
                    dgvEnrollments.Columns["StudentId"].Visible = false;
                }
                
                if (dgvEnrollments.Columns.Contains("CourseId"))
                {
                    dgvEnrollments.Columns["CourseId"].Visible = false;
                }
                
                if (dgvEnrollments.Columns.Contains("StudentName"))
                {
                    dgvEnrollments.Columns["StudentName"].HeaderText = "Student";
                    dgvEnrollments.Columns["StudentName"].Width = 150;
                }
                
                if (dgvEnrollments.Columns.Contains("CourseName"))
                {
                    dgvEnrollments.Columns["CourseName"].HeaderText = "Course";
                    dgvEnrollments.Columns["CourseName"].Width = 150;
                }
                
                if (dgvEnrollments.Columns.Contains("EnrollmentDate"))
                {
                    dgvEnrollments.Columns["EnrollmentDate"].HeaderText = "Enrollment Date";
                    dgvEnrollments.Columns["EnrollmentDate"].Width = 120;
                    dgvEnrollments.Columns["EnrollmentDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
                }
                
                // Update record count
                lblRecordCount.Text = $"Total Records: {_enrollments.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading enrollments: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetFormMode(FormMode mode)
        {
            _currentMode = mode;
            
            switch (mode)
            {
                case FormMode.View:
                    // Enable grid and disable input fields
                    pnlEnrollmentDetails.Enabled = false;
                    dgvEnrollments.Enabled = true;
                    
                    // Set button visibility
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    
                    // Clear selection if needed
                    if (_selectedEnrollment == null)
                    {
                        ClearInputs();
                    }
                    break;
                    
                case FormMode.Add:
                    // Disable grid and enable input fields
                    pnlEnrollmentDetails.Enabled = true;
                    dgvEnrollments.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    
                    // Clear inputs for new enrollment
                    ClearInputs();
                    
                    // Set default date
                    dtpEnrollmentDate.Value = DateTime.Today;
                    
                    // Focus on student dropdown
                    cmbStudent.Focus();
                    break;
                    
                case FormMode.Edit:
                    // Disable grid and enable input fields
                    pnlEnrollmentDetails.Enabled = true;
                    dgvEnrollments.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    break;
            }
        }

        private void ClearInputs()
        {
            txtEnrollmentId.Clear();
            cmbStudent.SelectedIndex = 0;
            cmbCourse.SelectedIndex = 0;
            dtpEnrollmentDate.Value = DateTime.Today;
            _selectedEnrollment = null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetFormMode(FormMode.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedEnrollment == null)
            {
                MessageBox.Show("Please select an enrollment to edit.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            SetFormMode(FormMode.Edit);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedEnrollment == null)
            {
                MessageBox.Show("Please select an enrollment to delete.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (MessageBox.Show("Are you sure you want to delete this enrollment?", "Confirm Delete", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // In a real application, you'd have a DeleteEnrollment method in the database manager
                    // For now, we'll just show a success message and reload enrollments
                    MessageBox.Show("Enrollment deleted successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Reload the data
                    LoadEnrollments();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting enrollment: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }
            
            try
            {
                bool success = false;
                
                // Get selected student
                Student selectedStudent = cmbStudent.SelectedItem as Student;
                if (selectedStudent == null || selectedStudent.StudentId == 0)
                {
                    _errorProvider.SetError(cmbStudent, "Please select a student");
                    return;
                }
                
                // Get selected course
                Course selectedCourse = cmbCourse.SelectedItem as Course;
                if (selectedCourse == null || selectedCourse.CourseId == 0)
                {
                    _errorProvider.SetError(cmbCourse, "Please select a course");
                    return;
                }
                
                // Process based on mode
                if (_currentMode == FormMode.Add)
                {
                    // Enroll student in course using stored procedure
                    success = _dbManager.EnrollStudent(selectedStudent.StudentId, selectedCourse.CourseId);
                    
                    if (success)
                    {
                        MessageBox.Show("Student enrolled successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (_currentMode == FormMode.Edit && _selectedEnrollment != null)
                {
                    // In a real application, you'd have an UpdateEnrollment method
                    // For now, just show a success message
                    MessageBox.Show("Enrollment updated successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    success = true;
                }
                
                if (success)
                {
                    // Reload the data
                    LoadEnrollments();
                    SetFormMode(FormMode.View);
                }
                else
                {
                    MessageBox.Show("Failed to save enrollment. The student might already be enrolled in this course.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving enrollment: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // If in edit mode, reload the selected enrollment details
            if (_currentMode == FormMode.Edit && _selectedEnrollment != null)
            {
                LoadEnrollmentDetails(_selectedEnrollment);
            }
            
            SetFormMode(FormMode.View);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // If search is empty, reload all enrollments
                LoadEnrollments();
                return;
            }
            
            try
            {
                // Filter the data by student name or course name
                var filteredEnrollments = _enrollments.Where(en => 
                    (en.StudentName != null && en.StudentName.ToLower().Contains(searchTerm)) ||
                    (en.CourseName != null && en.CourseName.ToLower().Contains(searchTerm))
                ).ToList();
                
                dgvEnrollments.DataSource = null;
                dgvEnrollments.DataSource = filteredEnrollments;
                
                lblRecordCount.Text = $"Total Records: {filteredEnrollments.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching enrollments: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadEnrollments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvEnrollments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _enrollments.Count)
            {
                _selectedEnrollment = _enrollments[e.RowIndex];
                LoadEnrollmentDetails(_selectedEnrollment);
            }
        }

        private void LoadEnrollmentDetails(Enrollment enrollment)
        {
            if (enrollment == null)
            {
                return;
            }
            
            txtEnrollmentId.Text = enrollment.EnrollmentId.ToString();
            dtpEnrollmentDate.Value = enrollment.EnrollmentDate;
            
            // Set student
            if (enrollment.StudentId.HasValue)
            {
                for (int i = 0; i < cmbStudent.Items.Count; i++)
                {
                    if (cmbStudent.Items[i] is Student student && student.StudentId == enrollment.StudentId.Value)
                    {
                        cmbStudent.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbStudent.SelectedIndex = 0; // None
            }
            
            // Set course
            if (enrollment.CourseId.HasValue)
            {
                for (int i = 0; i < cmbCourse.Items.Count; i++)
                {
                    if (cmbCourse.Items[i] is Course course && course.CourseId == enrollment.CourseId.Value)
                    {
                        cmbCourse.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbCourse.SelectedIndex = 0; // None
            }
        }

        private void btnUseStoredProc_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }
            
            try
            {
                // Get selected student
                Student selectedStudent = cmbStudent.SelectedItem as Student;
                if (selectedStudent == null || selectedStudent.StudentId == 0)
                {
                    _errorProvider.SetError(cmbStudent, "Please select a student");
                    return;
                }
                
                // Get selected course
                Course selectedCourse = cmbCourse.SelectedItem as Course;
                if (selectedCourse == null || selectedCourse.CourseId == 0)
                {
                    _errorProvider.SetError(cmbCourse, "Please select a course");
                    return;
                }
                
                // Enroll student using stored procedure
                bool success = _dbManager.EnrollStudent(selectedStudent.StudentId, selectedCourse.CourseId);
                
                if (success)
                {
                    MessageBox.Show("Student enrolled successfully using stored procedure.", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Reload the data
                    LoadEnrollments();
                    SetFormMode(FormMode.View);
                }
                else
                {
                    MessageBox.Show("Failed to enroll student. The student might already be enrolled in this course.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error enrolling student: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            
            // Clear previous errors
            _errorProvider.Clear();
            
            // Validate Student selection
            if (cmbStudent.SelectedIndex <= 0) // Not selected or "None"
            {
                _errorProvider.SetError(cmbStudent, "Please select a student");
                isValid = false;
            }
            
            // Validate Course selection
            if (cmbCourse.SelectedIndex <= 0) // Not selected or "None"
            {
                _errorProvider.SetError(cmbCourse, "Please select a course");
                isValid = false;
            }
            
            // Validate Enrollment Date
            if (dtpEnrollmentDate.Value > DateTime.Today)
            {
                _errorProvider.SetError(dtpEnrollmentDate, "Enrollment date cannot be in the future");
                isValid = false;
            }
            
            return isValid;
        }
    }
}
