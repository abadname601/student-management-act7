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
    public partial class StudentManagementForm : Form
    {
        private DatabaseManager _dbManager;
        private List<Student> _students;
        private List<Department> _departments;
        private ErrorProvider _errorProvider;
        private Student _selectedStudent = null;
        private enum FormMode { View, Add, Edit }
        private FormMode _currentMode = FormMode.View;
        
        public StudentManagementForm()
        {
            InitializeComponent();
            _dbManager = new DatabaseManager();
            _errorProvider = new ErrorProvider();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void StudentManagementForm_Load(object sender, EventArgs e)
        {
            // Apply styling
            UIHelper.StyleForm(this);
            UIHelper.StyleDataGridView(dgvStudents);
            UIHelper.StyleInputControls(this);
            
            // Initialize form
            LoadDepartments();
            LoadStudents();
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

        private void LoadStudents()
        {
            try
            {
                _students = _dbManager.GetAllStudents();
                
                // Bind to DataGridView
                dgvStudents.DataSource = null;
                dgvStudents.DataSource = _students;
                
                // Configure columns
                if (dgvStudents.Columns.Contains("StudentId"))
                {
                    dgvStudents.Columns["StudentId"].HeaderText = "ID";
                    dgvStudents.Columns["StudentId"].Width = 50;
                }
                
                if (dgvStudents.Columns.Contains("StudentName"))
                {
                    dgvStudents.Columns["StudentName"].HeaderText = "Name";
                    dgvStudents.Columns["StudentName"].Width = 150;
                }
                
                if (dgvStudents.Columns.Contains("Email"))
                {
                    dgvStudents.Columns["Email"].HeaderText = "Email";
                    dgvStudents.Columns["Email"].Width = 180;
                }
                
                if (dgvStudents.Columns.Contains("DepartmentId"))
                {
                    dgvStudents.Columns["DepartmentId"].Visible = false;
                }
                
                if (dgvStudents.Columns.Contains("DepartmentName"))
                {
                    dgvStudents.Columns["DepartmentName"].HeaderText = "Department";
                    dgvStudents.Columns["DepartmentName"].Width = 150;
                }
                
                // Update record count
                lblRecordCount.Text = $"Total Records: {_students.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error", 
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
                    pnlStudentDetails.Enabled = false;
                    dgvStudents.Enabled = true;
                    
                    // Set button visibility
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    
                    // Disable average score calculation
                    txtAvgScore.ReadOnly = true;
                    btnCalculateAvg.Enabled = false;
                    
                    // Clear selection if needed
                    if (_selectedStudent == null)
                    {
                        ClearInputs();
                    }
                    break;
                    
                case FormMode.Add:
                    // Disable grid and enable input fields
                    pnlStudentDetails.Enabled = true;
                    dgvStudents.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    
                    // Disable average score calculation
                    txtAvgScore.ReadOnly = true;
                    btnCalculateAvg.Enabled = false;
                    
                    // Clear inputs for new student
                    ClearInputs();
                    
                    // Focus on first field
                    txtStudentId.Focus();
                    break;
                    
                case FormMode.Edit:
                    // Disable grid and enable input fields
                    pnlStudentDetails.Enabled = true;
                    dgvStudents.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    
                    // Enable average score calculation
                    txtAvgScore.ReadOnly = true;
                    btnCalculateAvg.Enabled = true;
                    
                    // Disable student ID editing
                    txtStudentId.ReadOnly = true;
                    break;
            }
        }

        private void ClearInputs()
        {
            txtStudentId.Clear();
            txtStudentName.Clear();
            txtEmail.Clear();
            txtAvgScore.Clear();
            cmbDepartment.SelectedIndex = 0;
            _selectedStudent = null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetFormMode(FormMode.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedStudent == null)
            {
                MessageBox.Show("Please select a student to edit.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            SetFormMode(FormMode.Edit);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedStudent == null)
            {
                MessageBox.Show("Please select a student to delete.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (MessageBox.Show("Are you sure you want to delete this student?", "Confirm Delete", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (_dbManager.DeleteStudent(_selectedStudent.StudentId))
                    {
                        MessageBox.Show("Student deleted successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Reload the data
                        LoadStudents();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete student.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting student: {ex.Message}", "Error", 
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
                int studentId = int.Parse(txtStudentId.Text);
                string studentName = txtStudentName.Text;
                string email = txtEmail.Text;
                
                // Get department ID
                int? departmentId = null;
                if (cmbDepartment.SelectedIndex > 0) // Not "None"
                {
                    Department selectedDept = (Department)cmbDepartment.SelectedItem;
                    departmentId = selectedDept.DepartmentId;
                }
                
                if (_currentMode == FormMode.Add)
                {
                    // Add new student
                    Student newStudent = new Student
                    {
                        StudentId = studentId,
                        StudentName = studentName,
                        Email = email,
                        DepartmentId = departmentId
                    };
                    
                    success = _dbManager.AddStudent(newStudent);
                    
                    if (success)
                    {
                        MessageBox.Show("Student added successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (_currentMode == FormMode.Edit && _selectedStudent != null)
                {
                    // Update existing student
                    Student updatedStudent = new Student
                    {
                        StudentId = studentId,
                        StudentName = studentName,
                        Email = email,
                        DepartmentId = departmentId
                    };
                    
                    success = _dbManager.UpdateStudent(updatedStudent);
                    
                    if (success)
                    {
                        MessageBox.Show("Student updated successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
                if (success)
                {
                    // Reload the data
                    LoadStudents();
                    SetFormMode(FormMode.View);
                }
                else
                {
                    MessageBox.Show("Failed to save student.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving student: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // If in edit mode, reload the selected student details
            if (_currentMode == FormMode.Edit && _selectedStudent != null)
            {
                LoadStudentDetails(_selectedStudent);
            }
            
            SetFormMode(FormMode.View);
        }

        private void btnCalculateAvg_Click(object sender, EventArgs e)
        {
            if (_selectedStudent == null)
            {
                return;
            }
            
            try
            {
                double avgScore = _dbManager.GetStudentAverageScore(_selectedStudent.StudentId);
                txtAvgScore.Text = avgScore.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating average score: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // If search is empty, reload all students
                LoadStudents();
                return;
            }
            
            try
            {
                // Filter the data by name or email
                var filteredStudents = _students.Where(s => 
                    s.StudentName.ToLower().Contains(searchTerm) || 
                    s.Email.ToLower().Contains(searchTerm)
                ).ToList();
                
                dgvStudents.DataSource = null;
                dgvStudents.DataSource = filteredStudents;
                
                lblRecordCount.Text = $"Total Records: {filteredStudents.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching students: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadStudents();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _students.Count)
            {
                _selectedStudent = _students[e.RowIndex];
                LoadStudentDetails(_selectedStudent);
            }
        }

        private void LoadStudentDetails(Student student)
        {
            if (student == null)
            {
                return;
            }
            
            txtStudentId.Text = student.StudentId.ToString();
            txtStudentName.Text = student.StudentName;
            txtEmail.Text = student.Email;
            txtAvgScore.Clear();
            
            // Set department
            if (student.DepartmentId.HasValue)
            {
                for (int i = 0; i < cmbDepartment.Items.Count; i++)
                {
                    if (cmbDepartment.Items[i] is Department dept && dept.DepartmentId == student.DepartmentId.Value)
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
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            
            // Clear previous errors
            _errorProvider.Clear();
            
            // Validate Student ID
            if (string.IsNullOrWhiteSpace(txtStudentId.Text))
            {
                _errorProvider.SetError(txtStudentId, "Student ID is required");
                isValid = false;
            }
            else if (!int.TryParse(txtStudentId.Text, out int id))
            {
                _errorProvider.SetError(txtStudentId, "Student ID must be a number");
                isValid = false;
            }
            
            // Validate Student Name
            if (string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                _errorProvider.SetError(txtStudentName, "Student Name is required");
                isValid = false;
            }
            
            // Validate Email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                _errorProvider.SetError(txtEmail, "Email is required");
                isValid = false;
            }
            else if (!ValidationHelper.IsValidEmail(txtEmail, _errorProvider))
            {
                isValid = false;
            }
            
            return isValid;
        }
    }
}
