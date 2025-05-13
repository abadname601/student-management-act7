using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StudentManagementSystem.Forms;

namespace StudentManagementSystem
{
    public partial class MainForm : Form
    {
        private string _currentUsername;
        private string _userRole;

        public MainForm(string username, string userRole)
        {
            InitializeComponent();
            _currentUsername = username;
            _userRole = userRole;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set window title
            this.Text = "Student Management System";
            
            // Set welcome message
            lblWelcome.Text = $"Welcome, {_currentUsername}!";
            
            // Configure access based on user role
            ConfigureAccessRights();
        }

        private void ConfigureAccessRights()
        {
            // Configure menu items based on user role
            bool isAdmin = _userRole.Equals("Administrator", StringComparison.OrdinalIgnoreCase);
            
            // User management is only available to admins
            btnUserManagement.Visible = isAdmin;
            mnuUserManagement.Visible = isAdmin;
        }

        #region Menu Click Events

        private void mnuStudentManagement_Click(object sender, EventArgs e)
        {
            OpenStudentManagement();
        }

        private void mnuCourseManagement_Click(object sender, EventArgs e)
        {
            OpenCourseManagement();
        }

        private void mnuEnrollment_Click(object sender, EventArgs e)
        {
            OpenEnrollmentForm();
        }

        private void mnuExamResults_Click(object sender, EventArgs e)
        {
            OpenExamResultsForm();
        }

        private void mnuReports_Click(object sender, EventArgs e)
        {
            OpenReportsForm();
        }

        private void mnuUserManagement_Click(object sender, EventArgs e)
        {
            OpenUserManagement();
        }

        private void mnuLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Button Click Events

        private void btnStudentManagement_Click(object sender, EventArgs e)
        {
            OpenStudentManagement();
        }

        private void btnCourseManagement_Click(object sender, EventArgs e)
        {
            OpenCourseManagement();
        }

        private void btnEnrollment_Click(object sender, EventArgs e)
        {
            OpenEnrollmentForm();
        }

        private void btnExamResults_Click(object sender, EventArgs e)
        {
            OpenExamResultsForm();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            OpenReportsForm();
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            OpenUserManagement();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        #endregion

        #region Helper Methods

        private void OpenStudentManagement()
        {
            using (var form = new StudentManagementForm())
            {
                form.ShowDialog(this);
            }
        }

        private void OpenCourseManagement()
        {
            using (var form = new CourseManagementForm())
            {
                form.ShowDialog(this);
            }
        }

        private void OpenEnrollmentForm()
        {
            using (var form = new EnrollmentForm())
            {
                form.ShowDialog(this);
            }
        }

        private void OpenExamResultsForm()
        {
            using (var form = new ExamResultsForm())
            {
                form.ShowDialog(this);
            }
        }

        private void OpenReportsForm()
        {
            using (var form = new ReportForm())
            {
                form.ShowDialog(this);
            }
        }

        private void OpenUserManagement()
        {
            if (_userRole.Equals("Administrator", StringComparison.OrdinalIgnoreCase))
            {
                using (var form = new UserManagementForm())
                {
                    form.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show("You don't have permission to access User Management.", 
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Logout()
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                
                // Show login form
                using (var loginForm = new LoginForm())
                {
                    this.Dispose(); // Dispose current form
                    loginForm.ShowDialog();
                }
            }
        }

        #endregion
    }
}
