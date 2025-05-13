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

namespace StudentManagementSystem.Forms
{
    public partial class ForgotPasswordForm : Form
    {
        private ErrorProvider _errorProvider;
        private string _username;
        private string _recoveryQuestion;
        private enum RecoveryState { EnterUsername, AnswerQuestion, ResetPassword }
        private RecoveryState _currentState;

        public ForgotPasswordForm()
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _currentState = RecoveryState.EnterUsername;
        }

        private void ForgotPasswordForm_Load(object sender, EventArgs e)
        {
            // Apply UI styling
            UIHelper.StyleForm(this);
            UIHelper.StyleInputControls(this);
            
            // Initialize UI
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (_currentState)
            {
                case RecoveryState.EnterUsername:
                    lblTitle.Text = "Password Recovery - Step 1";
                    lblPrompt.Text = "Please enter your username:";
                    
                    pnlUsername.Visible = true;
                    pnlQuestion.Visible = false;
                    pnlNewPassword.Visible = false;
                    
                    btnNext.Text = "Next";
                    break;
                    
                case RecoveryState.AnswerQuestion:
                    lblTitle.Text = "Password Recovery - Step 2";
                    lblPrompt.Text = "Answer your security question:";
                    lblQuestion.Text = _recoveryQuestion;
                    
                    pnlUsername.Visible = false;
                    pnlQuestion.Visible = true;
                    pnlNewPassword.Visible = false;
                    
                    btnNext.Text = "Verify";
                    break;
                    
                case RecoveryState.ResetPassword:
                    lblTitle.Text = "Password Recovery - Step 3";
                    lblPrompt.Text = "Enter your new password:";
                    
                    pnlUsername.Visible = false;
                    pnlQuestion.Visible = false;
                    pnlNewPassword.Visible = true;
                    
                    btnNext.Text = "Reset Password";
                    break;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            switch (_currentState)
            {
                case RecoveryState.EnterUsername:
                    ProcessUsername();
                    break;
                    
                case RecoveryState.AnswerQuestion:
                    VerifyAnswer();
                    break;
                    
                case RecoveryState.ResetPassword:
                    ResetPassword();
                    break;
            }
        }

        private void ProcessUsername()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                _errorProvider.SetError(txtUsername, "Username is required");
                return;
            }
            
            _username = txtUsername.Text;
            
            try
            {
                using (var db = new DatabaseManager())
                {
                    _recoveryQuestion = db.GetRecoveryQuestion(_username);
                    
                    if (string.IsNullOrEmpty(_recoveryQuestion))
                    {
                        MessageBox.Show("Username not found or no recovery question set.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    // Move to next step
                    _currentState = RecoveryState.AnswerQuestion;
                    UpdateUI();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving recovery information: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VerifyAnswer()
        {
            if (string.IsNullOrWhiteSpace(txtAnswer.Text))
            {
                _errorProvider.SetError(txtAnswer, "Answer is required");
                return;
            }
            
            try
            {
                using (var db = new DatabaseManager())
                {
                    if (db.CheckRecoveryAnswer(_username, txtAnswer.Text))
                    {
                        // Answer is correct, move to password reset
                        _currentState = RecoveryState.ResetPassword;
                        UpdateUI();
                    }
                    else
                    {
                        MessageBox.Show("The answer provided is incorrect.", 
                            "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error verifying answer: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetPassword()
        {
            // Validate new password
            if (!ValidationHelper.IsStrongPassword(txtNewPassword, _errorProvider))
            {
                return;
            }
            
            // Validate password confirmation
            if (!ValidationHelper.PasswordsMatch(txtNewPassword, txtConfirmPassword, _errorProvider))
            {
                return;
            }
            
            try
            {
                // Hash the new password
                string newPasswordHash = Security.HashPassword(txtNewPassword.Text);
                
                using (var db = new DatabaseManager())
                {
                    if (db.ResetPassword(_username, newPasswordHash))
                    {
                        MessageBox.Show("Your password has been reset successfully. You can now login with your new password.", 
                            "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to reset password. Please try again later.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting password: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
