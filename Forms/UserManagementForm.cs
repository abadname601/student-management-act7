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
    public partial class UserManagementForm : Form
    {
        private DatabaseManager _dbManager;
        private DataTable _usersTable;
        private ErrorProvider _errorProvider;
        private int? _selectedUserId = null;
        private enum FormMode { View, Add, Edit }
        private FormMode _currentMode = FormMode.View;
        
        public UserManagementForm()
        {
            InitializeComponent();
            _dbManager = new DatabaseManager();
            _errorProvider = new ErrorProvider();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            // Apply styling
            UIHelper.StyleForm(this);
            UIHelper.StyleDataGridView(dgvUsers);
            UIHelper.StyleInputControls(this);
            
            // Initialize form
            SetupRoleComboBox();
            LoadUsers();
            SetFormMode(FormMode.View);
        }

        private void SetupRoleComboBox()
        {
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Administrator");
            cmbRole.Items.Add("Teacher");
            cmbRole.Items.Add("Student");
            cmbRole.SelectedIndex = 2; // Default to Student
        }

        private void LoadUsers()
        {
            try
            {
                _usersTable = _dbManager.GetAllUsers();
                dgvUsers.DataSource = _usersTable;
                
                // Hide password hash and recovery answer columns
                if (dgvUsers.Columns.Contains("password_hash"))
                {
                    dgvUsers.Columns["password_hash"].Visible = false;
                }
                
                if (dgvUsers.Columns.Contains("recovery_answer"))
                {
                    dgvUsers.Columns["recovery_answer"].Visible = false;
                }
                
                // Format column headers
                if (dgvUsers.Columns.Contains("user_id"))
                {
                    dgvUsers.Columns["user_id"].HeaderText = "ID";
                    dgvUsers.Columns["user_id"].Width = 50;
                }
                
                if (dgvUsers.Columns.Contains("username"))
                {
                    dgvUsers.Columns["username"].HeaderText = "Username";
                    dgvUsers.Columns["username"].Width = 120;
                }
                
                if (dgvUsers.Columns.Contains("email"))
                {
                    dgvUsers.Columns["email"].HeaderText = "Email";
                    dgvUsers.Columns["email"].Width = 180;
                }
                
                if (dgvUsers.Columns.Contains("role"))
                {
                    dgvUsers.Columns["role"].HeaderText = "Role";
                    dgvUsers.Columns["role"].Width = 100;
                }
                
                if (dgvUsers.Columns.Contains("recovery_question"))
                {
                    dgvUsers.Columns["recovery_question"].HeaderText = "Recovery Question";
                    dgvUsers.Columns["recovery_question"].Width = 200;
                }
                
                // If no users, show message
                lblRecordCount.Text = $"Total Records: {_usersTable.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", 
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
                    pnlUserDetails.Enabled = false;
                    dgvUsers.Enabled = true;
                    
                    // Set button visibility
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    
                    // Clear selection if needed
                    if (_selectedUserId == null)
                    {
                        ClearInputs();
                    }
                    break;
                    
                case FormMode.Add:
                    // Disable grid and enable input fields
                    pnlUserDetails.Enabled = true;
                    dgvUsers.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    
                    // Clear inputs for new user
                    ClearInputs();
                    
                    // Set password fields visible and required
                    lblPassword.Visible = true;
                    txtPassword.Visible = true;
                    lblConfirmPassword.Visible = true;
                    txtConfirmPassword.Visible = true;
                    
                    // Focus on first field
                    txtUsername.Focus();
                    break;
                    
                case FormMode.Edit:
                    // Disable grid and enable input fields
                    pnlUserDetails.Enabled = true;
                    dgvUsers.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    
                    // Hide password fields in edit mode
                    lblPassword.Visible = false;
                    txtPassword.Visible = false;
                    lblConfirmPassword.Visible = false;
                    txtConfirmPassword.Visible = false;
                    break;
            }
        }

        private void ClearInputs()
        {
            txtUsername.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtRecoveryQuestion.Clear();
            txtRecoveryAnswer.Clear();
            cmbRole.SelectedIndex = 2; // Default to Student
            _selectedUserId = null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetFormMode(FormMode.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedUserId == null)
            {
                MessageBox.Show("Please select a user to edit.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            SetFormMode(FormMode.Edit);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedUserId == null)
            {
                MessageBox.Show("Please select a user to delete.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (_dbManager.DeleteUser(_selectedUserId.Value))
                    {
                        MessageBox.Show("User deleted successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Reload the data
                        LoadUsers();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete user.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting user: {ex.Message}", "Error", 
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
                
                if (_currentMode == FormMode.Add)
                {
                    // Add new user
                    string passwordHash = Security.HashPassword(txtPassword.Text);
                    success = _dbManager.AddUser(
                        txtUsername.Text,
                        passwordHash,
                        txtEmail.Text,
                        cmbRole.SelectedItem.ToString(),
                        txtRecoveryQuestion.Text,
                        txtRecoveryAnswer.Text
                    );
                    
                    if (success)
                    {
                        MessageBox.Show("User added successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (_currentMode == FormMode.Edit && _selectedUserId.HasValue)
                {
                    // Update existing user
                    success = _dbManager.UpdateUser(
                        _selectedUserId.Value,
                        txtUsername.Text,
                        txtEmail.Text,
                        cmbRole.SelectedItem.ToString()
                    );
                    
                    if (success)
                    {
                        MessageBox.Show("User updated successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
                if (success)
                {
                    // Reload the data
                    LoadUsers();
                    SetFormMode(FormMode.View);
                }
                else
                {
                    MessageBox.Show("Failed to save user.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // If in edit mode, reload the selected user details
            if (_currentMode == FormMode.Edit && _selectedUserId.HasValue)
            {
                LoadUserDetails(_selectedUserId.Value);
            }
            
            SetFormMode(FormMode.View);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // If search is empty, reload all users
                LoadUsers();
                return;
            }
            
            try
            {
                // Filter the data by username or email
                DataView dv = _usersTable.DefaultView;
                dv.RowFilter = $"username LIKE '%{searchTerm}%' OR email LIKE '%{searchTerm}%'";
                
                dgvUsers.DataSource = dv.ToTable();
                lblRecordCount.Text = $"Total Records: {dv.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching users: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadUsers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int userId = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells["user_id"].Value);
                _selectedUserId = userId;
                LoadUserDetails(userId);
            }
        }

        private void LoadUserDetails(int userId)
        {
            try
            {
                DataRow[] rows = _usersTable.Select($"user_id = {userId}");
                
                if (rows.Length > 0)
                {
                    DataRow userRow = rows[0];
                    
                    txtUsername.Text = userRow["username"].ToString();
                    txtEmail.Text = userRow["email"].ToString();
                    txtRecoveryQuestion.Text = userRow["recovery_question"].ToString();
                    
                    // Do not show password or recovery answer
                    txtPassword.Clear();
                    txtConfirmPassword.Clear();
                    txtRecoveryAnswer.Clear();
                    
                    // Set role
                    string role = userRow["role"].ToString();
                    int index = cmbRole.FindString(role);
                    if (index >= 0)
                    {
                        cmbRole.SelectedIndex = index;
                    }
                    else
                    {
                        cmbRole.SelectedIndex = 2; // Default to Student
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            
            // Clear previous errors
            _errorProvider.Clear();
            
            // Validate username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                _errorProvider.SetError(txtUsername, "Username is required");
                isValid = false;
            }
            
            // Validate email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                _errorProvider.SetError(txtEmail, "Email is required");
                isValid = false;
            }
            else if (!ValidationHelper.IsValidEmail(txtEmail, _errorProvider))
            {
                isValid = false;
            }
            
            // Validate password fields if in Add mode
            if (_currentMode == FormMode.Add)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    _errorProvider.SetError(txtPassword, "Password is required");
                    isValid = false;
                }
                else if (!ValidationHelper.IsStrongPassword(txtPassword, _errorProvider))
                {
                    isValid = false;
                }
                
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    _errorProvider.SetError(txtConfirmPassword, "Passwords do not match");
                    isValid = false;
                }
            }
            
            // Validate recovery question and answer
            if (_currentMode == FormMode.Add)
            {
                if (string.IsNullOrWhiteSpace(txtRecoveryQuestion.Text))
                {
                    _errorProvider.SetError(txtRecoveryQuestion, "Recovery question is required");
                    isValid = false;
                }
                
                if (string.IsNullOrWhiteSpace(txtRecoveryAnswer.Text))
                {
                    _errorProvider.SetError(txtRecoveryAnswer, "Recovery answer is required");
                    isValid = false;
                }
            }
            
            return isValid;
        }
    }
}
