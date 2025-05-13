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
using StudentManagementSystem.Utilities;
using MySql.Data.MySqlClient;

namespace StudentManagementSystem.Forms
{
    public partial class LoginForm : Form
    {
        private ErrorProvider _errorProvider;

        public LoginForm()
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Set window title
            this.Text = "Login - Student Management System";
            
            // Clear inputs
            txtUsername.Clear();
            txtPassword.Clear();
            
            // Focus username field
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (!ValidateInputs())
            {
                return;
            }
            
            // Check if we're in test mode
            bool useInMemoryMode = Environment.GetEnvironmentVariable("DB_TEST_MODE")?.ToLower() == "true";
            
            try
            {
                if (useInMemoryMode)
                {
                    // In test mode, allow any login with password "password"
                    if (txtPassword.Text == "password")
                    {
                        // In-memory login successful
                        string userRole = txtUsername.Text.ToLower() == "admin" ? "Administrator" : "User";
                        
                        // Open main form
                        var mainForm = new MainForm(txtUsername.Text, userRole);
                        this.Hide();
                        mainForm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        // Invalid login in test mode
                        MessageBox.Show("In test mode, use 'password' as the password for any username.", 
                            "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
                else
                {
                    // Normal database authentication
                    using (var db = new DatabaseManager())
                    {
                        // Hash the password for comparison with the stored hash
                        string passwordHash = Security.HashPassword(txtPassword.Text);
                        
                        if (db.AuthenticateUser(txtUsername.Text, passwordHash))
                        {
                            // Login successful
                            string userRole = GetUserRole(txtUsername.Text);
                            
                            // Open main form
                            var mainForm = new MainForm(txtUsername.Text, userRole);
                            this.Hide();
                            mainForm.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            // Invalid login
                            MessageBox.Show("Invalid username or password. Please try again.", 
                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtPassword.Clear();
                            txtPassword.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetUserRole(string username)
        {
            // Check if we're in test mode
            bool useInMemoryMode = Environment.GetEnvironmentVariable("DB_TEST_MODE")?.ToLower() == "true";
            
            if (useInMemoryMode)
            {
                // In test mode, assign role based on username
                if (username.ToLower() == "admin")
                {
                    return "Administrator";
                }
                else
                {
                    return "User";
                }
            }
            
            // For regular mode
            string role = "User"; // Default role
            
            try
            {
                using (var db = new DatabaseManager())
                {
                    db.OpenConnection();
                    MySqlCommand cmd = new MySqlCommand("SELECT role FROM users WHERE username = @username", db._connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        role = result.ToString();
                    }
                }
            }
            catch
            {
                // If there's an error, return the default role
            }
            
            return role;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var forgotPasswordForm = new ForgotPasswordForm())
            {
                forgotPasswordForm.ShowDialog(this);
            }
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            
            // Validate username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                _errorProvider.SetError(txtUsername, "Username is required");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtUsername, "");
            }
            
            // Validate password
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                _errorProvider.SetError(txtPassword, "Password is required");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtPassword, "");
            }
            
            return isValid;
        }
    }
}
