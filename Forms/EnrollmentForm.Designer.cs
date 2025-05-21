namespace StudentManagementSystem.Forms
{
    partial class EnrollmentForm
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
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            dgvEnrollments = new DataGridView();
            panel3 = new Panel();
            lblRecordCount = new Label();
            btnClear = new Button();
            btnSearch = new Button();
            txtSearch = new TextBox();
            label2 = new Label();
            panel4 = new Panel();
            btnClose = new Button();
            btnCancel = new Button();
            btnSave = new Button();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            pnlEnrollmentDetails = new Panel();
            btnUseStoredProc = new Button();
            dtpEnrollmentDate = new DateTimePicker();
            label7 = new Label();
            cmbCourse = new ComboBox();
            label6 = new Label();
            cmbStudent = new ComboBox();
            label4 = new Label();
            txtEnrollmentId = new TextBox();
            label8 = new Label();
            label5 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEnrollments).BeginInit();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            pnlEnrollmentDetails.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(41, 128, 185);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(984, 60);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(984, 60);
            label1.TabIndex = 0;
            label1.Text = "Student Enrollment";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Controls.Add(dgvEnrollments);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 60);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(10);
            panel2.Size = new Size(600, 451);
            panel2.TabIndex = 1;
            // 
            // dgvEnrollments
            // 
            dgvEnrollments.AllowUserToAddRows = false;
            dgvEnrollments.AllowUserToDeleteRows = false;
            dgvEnrollments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEnrollments.Dock = DockStyle.Fill;
            dgvEnrollments.Location = new Point(10, 142);
            dgvEnrollments.MultiSelect = false;
            dgvEnrollments.Name = "dgvEnrollments";
            dgvEnrollments.ReadOnly = true;
            dgvEnrollments.RowHeadersWidth = 51;
            dgvEnrollments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEnrollments.Size = new Size(580, 299);
            dgvEnrollments.TabIndex = 1;
            dgvEnrollments.CellClick += dgvEnrollments_CellClick;
            // 
            // panel3
            // 
            panel3.Controls.Add(lblRecordCount);
            panel3.Controls.Add(btnClear);
            panel3.Controls.Add(btnSearch);
            panel3.Controls.Add(txtSearch);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(10, 10);
            panel3.Name = "panel3";
            panel3.Size = new Size(580, 132);
            panel3.TabIndex = 0;
            // 
            // lblRecordCount
            // 
            lblRecordCount.AutoSize = true;
            lblRecordCount.Location = new Point(0, 0);
            lblRecordCount.Name = "lblRecordCount";
            lblRecordCount.Size = new Size(136, 25);
            lblRecordCount.TabIndex = 4;
            lblRecordCount.Text = "Total Records: 0";
            // 
            // btnClear
            // 
            btnClear.Location = new Point(484, 42);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 40);
            btnClear.TabIndex = 3;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(379, 43);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 39);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(147, 42);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(194, 31);
            txtSearch.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 42);
            label2.Name = "label2";
            label2.Size = new Size(138, 25);
            label2.TabIndex = 0;
            label2.Text = "Search by name";
            // 
            // panel4
            // 
            panel4.Controls.Add(btnClose);
            panel4.Controls.Add(btnSave);
            panel4.Controls.Add(btnEdit);
            panel4.Controls.Add(btnAdd);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(600, 461);
            panel4.Name = "panel4";
            panel4.Size = new Size(384, 50);
            panel4.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.None;
            btnClose.BackColor = Color.FromArgb(192, 0, 0);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(289, 6);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(80, 35);
            btnClose.TabIndex = 5;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.None;
            btnCancel.BackColor = Color.FromArgb(192, 0, 0);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(289, 360);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 35);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Visible = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.None;
            btnSave.BackColor = Color.FromArgb(0, 192, 0);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(89, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(80, 35);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Visible = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.None;
            btnDelete.BackColor = Color.FromArgb(192, 0, 0);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(189, 360);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 35);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEdit
            // 
            btnEdit.Anchor = AnchorStyles.None;
            btnEdit.BackColor = Color.FromArgb(41, 128, 185);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(189, 6);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(94, 35);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.None;
            btnAdd.BackColor = Color.FromArgb(0, 192, 0);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(13, 8);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(70, 35);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // pnlEnrollmentDetails
            // 
            pnlEnrollmentDetails.Controls.Add(btnUseStoredProc);
            pnlEnrollmentDetails.Controls.Add(btnCancel);
            pnlEnrollmentDetails.Controls.Add(btnDelete);
            pnlEnrollmentDetails.Controls.Add(dtpEnrollmentDate);
            pnlEnrollmentDetails.Controls.Add(label7);
            pnlEnrollmentDetails.Controls.Add(cmbCourse);
            pnlEnrollmentDetails.Controls.Add(label6);
            pnlEnrollmentDetails.Controls.Add(cmbStudent);
            pnlEnrollmentDetails.Controls.Add(label4);
            pnlEnrollmentDetails.Controls.Add(txtEnrollmentId);
            pnlEnrollmentDetails.Controls.Add(label8);
            pnlEnrollmentDetails.Controls.Add(label5);
            pnlEnrollmentDetails.Dock = DockStyle.Fill;
            pnlEnrollmentDetails.Location = new Point(600, 60);
            pnlEnrollmentDetails.Name = "pnlEnrollmentDetails";
            pnlEnrollmentDetails.Padding = new Padding(10);
            pnlEnrollmentDetails.Size = new Size(384, 401);
            pnlEnrollmentDetails.TabIndex = 3;
            // 
            // btnUseStoredProc
            // 
            btnUseStoredProc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnUseStoredProc.BackColor = Color.FromArgb(41, 128, 185);
            btnUseStoredProc.FlatStyle = FlatStyle.Flat;
            btnUseStoredProc.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnUseStoredProc.ForeColor = Color.White;
            btnUseStoredProc.Location = new Point(132, 289);
            btnUseStoredProc.Name = "btnUseStoredProc";
            btnUseStoredProc.Size = new Size(237, 35);
            btnUseStoredProc.TabIndex = 14;
            btnUseStoredProc.Text = "Use Stored Procedure to Enroll";
            btnUseStoredProc.UseVisualStyleBackColor = false;
            btnUseStoredProc.Click += btnUseStoredProc_Click;
            // 
            // dtpEnrollmentDate
            // 
            dtpEnrollmentDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dtpEnrollmentDate.Format = DateTimePickerFormat.Short;
            dtpEnrollmentDate.Location = new Point(189, 227);
            dtpEnrollmentDate.Name = "dtpEnrollmentDate";
            dtpEnrollmentDate.Size = new Size(180, 31);
            dtpEnrollmentDate.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 232);
            label7.Name = "label7";
            label7.Size = new Size(143, 25);
            label7.TabIndex = 11;
            label7.Text = "Enrollment Date:";
            // 
            // cmbCourse
            // 
            cmbCourse.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourse.FormattingEnabled = true;
            cmbCourse.Location = new Point(132, 189);
            cmbCourse.Name = "cmbCourse";
            cmbCourse.Size = new Size(237, 33);
            cmbCourse.TabIndex = 10;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 192);
            label6.Name = "label6";
            label6.Size = new Size(71, 25);
            label6.TabIndex = 9;
            label6.Text = "Course:";
            // 
            // cmbStudent
            // 
            cmbStudent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbStudent.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStudent.FormattingEnabled = true;
            cmbStudent.Location = new Point(132, 150);
            cmbStudent.Name = "cmbStudent";
            cmbStudent.Size = new Size(237, 33);
            cmbStudent.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 153);
            label4.Name = "label4";
            label4.Size = new Size(77, 25);
            label4.TabIndex = 3;
            label4.Text = "Student:";
            // 
            // txtEnrollmentId
            // 
            txtEnrollmentId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtEnrollmentId.Location = new Point(132, 111);
            txtEnrollmentId.Name = "txtEnrollmentId";
            txtEnrollmentId.ReadOnly = true;
            txtEnrollmentId.Size = new Size(237, 31);
            txtEnrollmentId.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 114);
            label8.Name = "label8";
            label8.Size = new Size(124, 25);
            label8.TabIndex = 1;
            label8.Text = "Enrollment ID:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(116, 46);
            label5.Name = "label5";
            label5.Size = new Size(225, 32);
            label5.TabIndex = 0;
            label5.Text = "Enrollment Details";
            // 
            // EnrollmentForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 511);
            Controls.Add(pnlEnrollmentDetails);
            Controls.Add(panel4);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "EnrollmentForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Student Enrollment";
            Load += EnrollmentForm_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEnrollments).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            pnlEnrollmentDetails.ResumeLayout(false);
            pnlEnrollmentDetails.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvEnrollments;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel pnlEnrollmentDetails;
        private System.Windows.Forms.DateTimePicker dtpEnrollmentDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbCourse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbStudent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEnrollmentId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnUseStoredProc;
    }
}
