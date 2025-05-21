namespace StudentManagementSystem.Forms
{
    partial class ForgotPasswordForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            panel1 = new Panel();
            btnCancel = new Button();
            btnNext = new Button();
            lblPrompt = new Label();
            pnlUsername = new Panel();
            txtUsername = new TextBox();
            pnlQuestion = new Panel();
            lblQuestion = new Label();
            txtAnswer = new TextBox();
            pnlNewPassword = new Panel();
            label2 = new Label();
            txtConfirmPassword = new TextBox();
            label1 = new Label();
            txtNewPassword = new TextBox();
            panel1.SuspendLayout();
            pnlUsername.SuspendLayout();
            pnlQuestion.SuspendLayout();
            pnlNewPassword.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(484, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Password Recovery";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(btnNext);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 311);
            panel1.Name = "panel1";
            panel1.Size = new Size(484, 50);
            panel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.None;
            btnCancel.BackColor = Color.FromArgb(192, 0, 0);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(252, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 35);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.None;
            btnNext.BackColor = Color.FromArgb(41, 128, 185);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnNext.ForeColor = Color.White;
            btnNext.Location = new Point(132, 8);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(100, 35);
            btnNext.TabIndex = 0;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = false;
            btnNext.Click += btnNext_Click;
            // 
            // lblPrompt
            // 
            lblPrompt.Dock = DockStyle.Top;
            lblPrompt.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            lblPrompt.Location = new Point(0, 50);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Padding = new Padding(20, 10, 20, 10);
            lblPrompt.Size = new Size(484, 40);
            lblPrompt.TabIndex = 2;
            lblPrompt.Text = "Please enter your username:";
            // 
            // pnlUsername
            // 
            pnlUsername.Controls.Add(txtUsername);
            pnlUsername.Location = new Point(92, 115);
            pnlUsername.Name = "pnlUsername";
            pnlUsername.Size = new Size(300, 40);
            pnlUsername.TabIndex = 3;
            // 
            // txtUsername
            // 
            txtUsername.Dock = DockStyle.Fill;
            txtUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtUsername.Location = new Point(0, 0);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(300, 39);
            txtUsername.TabIndex = 0;
            // 
            // pnlQuestion
            // 
            pnlQuestion.Controls.Add(lblQuestion);
            pnlQuestion.Controls.Add(txtAnswer);
            pnlQuestion.Location = new Point(92, 115);
            pnlQuestion.Name = "pnlQuestion";
            pnlQuestion.Size = new Size(300, 109);
            pnlQuestion.TabIndex = 4;
            pnlQuestion.Visible = false;
            // 
            // lblQuestion
            // 
            lblQuestion.Dock = DockStyle.Top;
            lblQuestion.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblQuestion.Location = new Point(0, 0);
            lblQuestion.Name = "lblQuestion";
            lblQuestion.Padding = new Padding(0, 0, 0, 10);
            lblQuestion.Size = new Size(300, 50);
            lblQuestion.TabIndex = 1;
            lblQuestion.Text = "Security question here?";
            lblQuestion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtAnswer
            // 
            txtAnswer.Dock = DockStyle.Bottom;
            txtAnswer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtAnswer.Location = new Point(0, 70);
            txtAnswer.Name = "txtAnswer";
            txtAnswer.Size = new Size(300, 39);
            txtAnswer.TabIndex = 0;
            // 
            // pnlNewPassword
            // 
            pnlNewPassword.Controls.Add(label2);
            pnlNewPassword.Controls.Add(txtConfirmPassword);
            pnlNewPassword.Controls.Add(label1);
            pnlNewPassword.Controls.Add(txtNewPassword);
            pnlNewPassword.Location = new Point(92, 115);
            pnlNewPassword.Name = "pnlNewPassword";
            pnlNewPassword.Size = new Size(300, 150);
            pnlNewPassword.TabIndex = 5;
            pnlNewPassword.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(3, 85);
            label2.Name = "label2";
            label2.Size = new Size(172, 28);
            label2.TabIndex = 3;
            label2.Text = "Confirm Password:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtConfirmPassword.Location = new Point(0, 105);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '•';
            txtConfirmPassword.Size = new Size(300, 39);
            txtConfirmPassword.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(3, 15);
            label1.Name = "label1";
            label1.Size = new Size(141, 28);
            label1.TabIndex = 1;
            label1.Text = "New Password:";
            // 
            // txtNewPassword
            // 
            txtNewPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtNewPassword.Location = new Point(0, 35);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '•';
            txtNewPassword.Size = new Size(300, 39);
            txtNewPassword.TabIndex = 0;
            // 
            // ForgotPasswordForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 361);
            Controls.Add(pnlNewPassword);
            Controls.Add(pnlQuestion);
            Controls.Add(pnlUsername);
            Controls.Add(lblPrompt);
            Controls.Add(panel1);
            Controls.Add(lblTitle);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ForgotPasswordForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Password Recovery";
            Load += ForgotPasswordForm_Load;
            panel1.ResumeLayout(false);
            pnlUsername.ResumeLayout(false);
            pnlUsername.PerformLayout();
            pnlQuestion.ResumeLayout(false);
            pnlQuestion.PerformLayout();
            pnlNewPassword.ResumeLayout(false);
            pnlNewPassword.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Panel pnlUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Panel pnlQuestion;
        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.TextBox txtAnswer;
        private System.Windows.Forms.Panel pnlNewPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNewPassword;
    }
}
