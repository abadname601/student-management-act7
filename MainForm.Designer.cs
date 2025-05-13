using System.Windows.Forms;

namespace StudentManagementSystem
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.managementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStudentManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCourseManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEnrollment = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExamResults = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReports = new System.Windows.Forms.ToolStripMenuItem();
            this.administrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUserManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnUserManagement = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnExamResults = new System.Windows.Forms.Button();
            this.btnEnrollment = new System.Windows.Forms.Button();
            this.btnCourseManagement = new System.Windows.Forms.Button();
            this.btnStudentManagement = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.managementToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.administrationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLogout,
            this.toolStripSeparator1,
            this.mnuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // mnuLogout
            // 
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.Size = new System.Drawing.Size(112, 22);
            this.mnuLogout.Text = "Logout";
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(109, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(112, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // managementToolStripMenuItem
            // 
            this.managementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStudentManagement,
            this.mnuCourseManagement,
            this.mnuEnrollment,
            this.mnuExamResults});
            this.managementToolStripMenuItem.Name = "managementToolStripMenuItem";
            this.managementToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.managementToolStripMenuItem.Text = "Management";
            // 
            // mnuStudentManagement
            // 
            this.mnuStudentManagement.Name = "mnuStudentManagement";
            this.mnuStudentManagement.Size = new System.Drawing.Size(191, 22);
            this.mnuStudentManagement.Text = "Student Management";
            this.mnuStudentManagement.Click += new System.EventHandler(this.mnuStudentManagement_Click);
            // 
            // mnuCourseManagement
            // 
            this.mnuCourseManagement.Name = "mnuCourseManagement";
            this.mnuCourseManagement.Size = new System.Drawing.Size(191, 22);
            this.mnuCourseManagement.Text = "Course Management";
            this.mnuCourseManagement.Click += new System.EventHandler(this.mnuCourseManagement_Click);
            // 
            // mnuEnrollment
            // 
            this.mnuEnrollment.Name = "mnuEnrollment";
            this.mnuEnrollment.Size = new System.Drawing.Size(191, 22);
            this.mnuEnrollment.Text = "Enrollment";
            this.mnuEnrollment.Click += new System.EventHandler(this.mnuEnrollment_Click);
            // 
            // mnuExamResults
            // 
            this.mnuExamResults.Name = "mnuExamResults";
            this.mnuExamResults.Size = new System.Drawing.Size(191, 22);
            this.mnuExamResults.Text = "Exam Results";
            this.mnuExamResults.Click += new System.EventHandler(this.mnuExamResults_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuReports});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // mnuReports
            // 
            this.mnuReports.Name = "mnuReports";
            this.mnuReports.Size = new System.Drawing.Size(180, 22);
            this.mnuReports.Text = "Generate Reports";
            this.mnuReports.Click += new System.EventHandler(this.mnuReports_Click);
            // 
            // administrationToolStripMenuItem
            // 
            this.administrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUserManagement});
            this.administrationToolStripMenuItem.Name = "administrationToolStripMenuItem";
            this.administrationToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.administrationToolStripMenuItem.Text = "Administration";
            // 
            // mnuUserManagement
            // 
            this.mnuUserManagement.Name = "mnuUserManagement";
            this.mnuUserManagement.Size = new System.Drawing.Size(180, 22);
            this.mnuUserManagement.Text = "User Management";
            this.mnuUserManagement.Click += new System.EventHandler(this.mnuUserManagement_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 589);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 17);
            this.lblStatus.Text = "Ready.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.lblWelcome);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 100);
            this.panel1.TabIndex = 2;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(96, 36);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(111, 32);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(12, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 80);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 124);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(20);
            this.panel2.Size = new System.Drawing.Size(984, 465);
            this.panel2.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnLogout, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnUserManagement, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnReports, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnExamResults, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnEnrollment, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCourseManagement, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnStudentManagement, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(944, 425);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(631, 285);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(310, 137);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnUserManagement
            // 
            this.btnUserManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnUserManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUserManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserManagement.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUserManagement.ForeColor = System.Drawing.Color.White;
            this.btnUserManagement.Location = new System.Drawing.Point(317, 285);
            this.btnUserManagement.Name = "btnUserManagement";
            this.btnUserManagement.Size = new System.Drawing.Size(308, 137);
            this.btnUserManagement.TabIndex = 5;
            this.btnUserManagement.Text = "User Management";
            this.btnUserManagement.UseVisualStyleBackColor = false;
            this.btnUserManagement.Click += new System.EventHandler(this.btnUserManagement_Click);
            // 
            // btnReports
            // 
            this.btnReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReports.ForeColor = System.Drawing.Color.White;
            this.btnReports.Location = new System.Drawing.Point(3, 285);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(308, 137);
            this.btnReports.TabIndex = 4;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = false;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnExamResults
            // 
            this.btnExamResults.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnExamResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExamResults.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExamResults.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExamResults.ForeColor = System.Drawing.Color.White;
            this.btnExamResults.Location = new System.Drawing.Point(631, 144);
            this.btnExamResults.Name = "btnExamResults";
            this.btnExamResults.Size = new System.Drawing.Size(310, 135);
            this.btnExamResults.TabIndex = 3;
            this.btnExamResults.Text = "Exam Results";
            this.btnExamResults.UseVisualStyleBackColor = false;
            this.btnExamResults.Click += new System.EventHandler(this.btnExamResults_Click);
            // 
            // btnEnrollment
            // 
            this.btnEnrollment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnEnrollment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnrollment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnrollment.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnrollment.ForeColor = System.Drawing.Color.White;
            this.btnEnrollment.Location = new System.Drawing.Point(317, 144);
            this.btnEnrollment.Name = "btnEnrollment";
            this.btnEnrollment.Size = new System.Drawing.Size(308, 135);
            this.btnEnrollment.TabIndex = 2;
            this.btnEnrollment.Text = "Enrollment";
            this.btnEnrollment.UseVisualStyleBackColor = false;
            this.btnEnrollment.Click += new System.EventHandler(this.btnEnrollment_Click);
            // 
            // btnCourseManagement
            // 
            this.btnCourseManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnCourseManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCourseManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCourseManagement.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCourseManagement.ForeColor = System.Drawing.Color.White;
            this.btnCourseManagement.Location = new System.Drawing.Point(3, 144);
            this.btnCourseManagement.Name = "btnCourseManagement";
            this.btnCourseManagement.Size = new System.Drawing.Size(308, 135);
            this.btnCourseManagement.TabIndex = 1;
            this.btnCourseManagement.Text = "Course Management";
            this.btnCourseManagement.UseVisualStyleBackColor = false;
            this.btnCourseManagement.Click += new System.EventHandler(this.btnCourseManagement_Click);
            // 
            // btnStudentManagement
            // 
            this.btnStudentManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.tableLayoutPanel1.SetColumnSpan(this.btnStudentManagement, 3);
            this.btnStudentManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStudentManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudentManagement.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStudentManagement.ForeColor = System.Drawing.Color.White;
            this.btnStudentManagement.Location = new System.Drawing.Point(3, 3);
            this.btnStudentManagement.Name = "btnStudentManagement";
            this.btnStudentManagement.Size = new System.Drawing.Size(938, 135);
            this.btnStudentManagement.TabIndex = 0;
            this.btnStudentManagement.Text = "Student Management";
            this.btnStudentManagement.UseVisualStyleBackColor = false;
            this.btnStudentManagement.Click += new System.EventHandler(this.btnStudentManagement_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Management System";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem managementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuStudentManagement;
        private System.Windows.Forms.ToolStripMenuItem mnuCourseManagement;
        private System.Windows.Forms.ToolStripMenuItem mnuEnrollment;
        private System.Windows.Forms.ToolStripMenuItem mnuExamResults;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuReports;
        private System.Windows.Forms.ToolStripMenuItem administrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuUserManagement;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnUserManagement;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnExamResults;
        private System.Windows.Forms.Button btnEnrollment;
        private System.Windows.Forms.Button btnCourseManagement;
        private System.Windows.Forms.Button btnStudentManagement;
    }
}
