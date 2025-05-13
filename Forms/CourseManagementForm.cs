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
    public partial class CourseManagementForm : Form
    {
        private DatabaseManager _dbManager;
        private List<Course> _courses;
        private List<Department> _departments;
        private List<Professor> _professors;
        private ErrorProvider _errorProvider;
        private Course _selectedCourse = null;
        private enum FormMode { View, Add, Edit }
        private FormMode _currentMode = FormMode.View;
        
        public CourseManagementForm()
        {
            InitializeComponent();
            _dbManager = new DatabaseManager();
            _errorProvider = new ErrorProvider();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void CourseManagementForm_Load(object sender, EventArgs e)
        {
            // Apply styling
            UIHelper.StyleForm(this);
            UIHelper.StyleDataGridView(dgvCourses);
            UIHelper.StyleInputControls(this);
            
            // Initialize form
            LoadDepartments();
            LoadProfessors();
            LoadCourses();
            SetFormMode(FormMode.View);
        }

        private void LoadDepartments()
        {
            try
            {
                _departments = _dbManager.GetAllDepartments();
                
                // Bind to combo box
                cmbDepartment.Items.Clear();
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "DepartmentId";
                
                // Add "None" option
                cmbDepartment.Items.Add(new Department { DepartmentId = 0, DepartmentName = "-- None --" });
                
                foreach (var dept in _departments)
                {
                    cmbDepartment.Items.Add(dept);
                }
                
                cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProfessors()
        {
            try
            {
                _professors = _dbManager.GetAllProfessors();
                
                // Bind to combo box
                cmbProfessor.Items.Clear();
                cmbProfessor.DisplayMember = "ProfessorName";
                cmbProfessor.ValueMember = "ProfessorId";
                
                // Add "None" option
                cmbProfessor.Items.Add(new Professor { ProfessorId = 0, ProfessorName = "-- None --" });
                
                foreach (var prof in _professors)
                {
                    cmbProfessor.Items.Add(prof);
                }
                
                cmbProfessor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading professors: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCourses()
        {
            try
            {
                _courses = _dbManager.GetAllCourses();
                
                // Bind to DataGridView
                dgvCourses.DataSource = null;
                dgvCourses.DataSource = _courses;
                
                // Configure columns
                if (dgvCourses.Columns.Contains("CourseId"))
                {
                    dgvCourses.Columns["CourseId"].HeaderText = "ID";
                    dgvCourses.Columns["CourseId"].Width = 50;
                }
                
                if (dgvCourses.Columns.Contains("CourseName"))
                {
                    dgvCourses.Columns["CourseName"].HeaderText = "Course Name";
                    dgvCourses.Columns["CourseName"].Width = 150;
                }
                
                if (dgvCourses.Columns.Contains("DepartmentId"))
                {
                    dgvCourses.Columns["DepartmentId"].Visible = false;
                }
                
                if (dgvCourses.Columns.Contains("ProfessorId"))
                {
                    dgvCourses.Columns["ProfessorId"].Visible = false;
                }
                
                if (dgvCourses.Columns.Contains("DepartmentName"))
                {
                    dgvCourses.Columns["DepartmentName"].HeaderText = "Department";
                    dgvCourses.Columns["DepartmentName"].Width = 150;
                }
                
                if (dgvCourses.Columns.Contains("ProfessorName"))
                {
                    dgvCourses.Columns["ProfessorName"].HeaderText = "Professor";
                    dgvCourses.Columns["ProfessorName"].Width = 150;
                }
                
                // Update record count
                lblRecordCount.Text = $"Total Records: {_courses.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error", 
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
                    pnlCourseDetails.Enabled = false;
                    dgvCourses.Enabled = true;
                    
                    // Set button visibility
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    
                    // Disable enrolled count
                    txtEnrolledCount.ReadOnly = true;
                    btnGetEnrolled.Enabled = false;
                    
                    // Clear selection if needed
                    if (_selectedCourse == null)
                    {
                        ClearInputs();
                    }
                    break;
                    
                case FormMode.Add:
                    // Disable grid and enable input fields
                    pnlCourseDetails.Enabled = true;
                    dgvCourses.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    
                    // Disable enrolled count
                    txtEnrolledCount.ReadOnly = true;
                    btnGetEnrolled.Enabled = false;
                    
                    // Clear inputs for new course
                    ClearInputs();
                    
                    // Focus on first field
                    txtCourseId.Focus();
                    break;
                    
                case FormMode.Edit:
                    // Disable grid and enable input fields
                    pnlCourseDetails.Enabled = true;
                    dgvCourses.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    
                    // Enable enrolled count
                    txtEnrolledCount.ReadOnly = true;
                    btnGetEnrolled.Enabled = true;
                    
                    // Disable course ID editing
                    txtCourseId.ReadOnly = true;
                    break;
            }
        }

        private void ClearInputs()
        {
            txtCourseId.Clear();
            txtCourseName.Clear();
            txtEnrolledCount.Clear();
            cmbDepartment.SelectedIndex = 0;
            cmbProfessor.SelectedIndex = 0;
            _selectedCourse = null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetFormMode(FormMode.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedCourse == null)
            {
                MessageBox.Show("Please select a course to edit.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            SetFormMode(FormMode.Edit);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCourse == null)
            {
                MessageBox.Show("Please select a course to delete.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (MessageBox.Show("Are you sure you want to delete this course?", "Confirm Delete", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (_dbManager.DeleteCourse(_selectedCourse.CourseId))
                    {
                        MessageBox.Show("Course deleted successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Reload the data
                        LoadCourses();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete course.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting course: {ex.Message}", "Error", 
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
                int courseId = int.Parse(txtCourseId.Text);
                string courseName = txtCourseName.Text;
                
                // Get department ID
                int? departmentId = null;
                if (cmbDepartment.SelectedIndex > 0) // Not "None"
                {
                    Department selectedDept = (Department)cmbDepartment.SelectedItem;
                    departmentId = selectedDept.DepartmentId;
                }
                
                // Get professor ID
                int? professorId = null;
                if (cmbProfessor.SelectedIndex > 0) // Not "None"
                {
                    Professor selectedProf = (Professor)cmbProfessor.SelectedItem;
                    professorId = selectedProf.ProfessorId;
                }
                
                if (_currentMode == FormMode.Add)
                {
                    // Add new course
                    Course newCourse = new Course
                    {
                        CourseId = courseId,
                        CourseName = courseName,
                        DepartmentId = departmentId,
                        ProfessorId = professorId
                    };
                    
                    success = _dbManager.AddCourse(newCourse);
                    
                    if (success)
                    {
                        MessageBox.Show("Course added successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (_currentMode == FormMode.Edit && _selectedCourse != null)
                {
                    // Update existing course
                    Course updatedCourse = new Course
                    {
                        CourseId = courseId,
                        CourseName = courseName,
                        DepartmentId = departmentId,
                        ProfessorId = professorId
                    };
                    
                    success = _dbManager.UpdateCourse(updatedCourse);
                    
                    if (success)
                    {
                        MessageBox.Show("Course updated successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
                if (success)
                {
                    // Reload the data
                    LoadCourses();
                    SetFormMode(FormMode.View);
                }
                else
                {
                    MessageBox.Show("Failed to save course.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving course: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // If in edit mode, reload the selected course details
            if (_currentMode == FormMode.Edit && _selectedCourse != null)
            {
                LoadCourseDetails(_selectedCourse);
            }
            
            SetFormMode(FormMode.View);
        }

        private void btnGetEnrolled_Click(object sender, EventArgs e)
        {
            if (_selectedCourse == null)
            {
                return;
            }
            
            try
            {
                int enrolledCount = _dbManager.GetTotalEnrolled(_selectedCourse.CourseId);
                txtEnrolledCount.Text = enrolledCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting enrolled count: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // If search is empty, reload all courses
                LoadCourses();
                return;
            }
            
            try
            {
                // Filter the data by name or department
                var filteredCourses = _courses.Where(c => 
                    c.CourseName.ToLower().Contains(searchTerm) || 
                    (c.DepartmentName != null && c.DepartmentName.ToLower().Contains(searchTerm)) ||
                    (c.ProfessorName != null && c.ProfessorName.ToLower().Contains(searchTerm))
                ).ToList();
                
                dgvCourses.DataSource = null;
                dgvCourses.DataSource = filteredCourses;
                
                lblRecordCount.Text = $"Total Records: {filteredCourses.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching courses: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadCourses();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _courses.Count)
            {
                _selectedCourse = _courses[e.RowIndex];
                LoadCourseDetails(_selectedCourse);
            }
        }

        private void LoadCourseDetails(Course course)
        {
            if (course == null)
            {
                return;
            }
            
            txtCourseId.Text = course.CourseId.ToString();
            txtCourseName.Text = course.CourseName;
            txtEnrolledCount.Clear();
            
            // Set department
            if (course.DepartmentId.HasValue)
            {
                for (int i = 0; i < cmbDepartment.Items.Count; i++)
                {
                    if (cmbDepartment.Items[i] is Department dept && dept.DepartmentId == course.DepartmentId.Value)
                    {
                        cmbDepartment.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbDepartment.SelectedIndex = 0; // None
            }
            
            // Set professor
            if (course.ProfessorId.HasValue)
            {
                for (int i = 0; i < cmbProfessor.Items.Count; i++)
                {
                    if (cmbProfessor.Items[i] is Professor prof && prof.ProfessorId == course.ProfessorId.Value)
                    {
                        cmbProfessor.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbProfessor.SelectedIndex = 0; // None
            }
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            
            // Clear previous errors
            _errorProvider.Clear();
            
            // Validate Course ID
            if (string.IsNullOrWhiteSpace(txtCourseId.Text))
            {
                _errorProvider.SetError(txtCourseId, "Course ID is required");
                isValid = false;
            }
            else if (!int.TryParse(txtCourseId.Text, out int id))
            {
                _errorProvider.SetError(txtCourseId, "Course ID must be a number");
                isValid = false;
            }
            
            // Validate Course Name
            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                _errorProvider.SetError(txtCourseName, "Course Name is required");
                isValid = false;
            }
            
            return isValid;
        }
    }
}
