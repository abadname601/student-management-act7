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
    public partial class ExamResultsForm : Form
    {
        private DatabaseManager _dbManager;
        private List<ExamResult> _examResults;
        private List<Student> _students;
        private List<Exam> _exams;
        private ErrorProvider _errorProvider;
        private ExamResult _selectedResult = null;
        private enum FormMode { View, Add, Edit }
        private FormMode _currentMode = FormMode.View;
        
        public ExamResultsForm()
        {
            InitializeComponent();
            _dbManager = new DatabaseManager();
            _errorProvider = new ErrorProvider();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void ExamResultsForm_Load(object sender, EventArgs e)
        {
            // Apply styling
            UIHelper.StyleForm(this);
            UIHelper.StyleDataGridView(dgvExamResults);
            UIHelper.StyleInputControls(this);
            
            // Initialize form
            LoadStudents();
            LoadExams();
            LoadExamResults();
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

        private void LoadExams()
        {
            try
            {
                _exams = _dbManager.GetAllExams();
                
                // Bind to combo box
                cmbExam.Items.Clear();
                cmbExam.DisplayMember = "ExamIdWithCourse";
                cmbExam.ValueMember = "ExamId";
                
                // Add "None" option
                var noneExam = new Exam { ExamId = 0, CourseName = "-- Select Exam --" };
                cmbExam.Items.Add(noneExam);
                
                foreach (var exam in _exams)
                {
                    // Add a custom property for display
                    exam.ExamIdWithCourse = $"Exam {exam.ExamId} - {exam.CourseName} ({exam.ExamDate.ToShortDateString()})";
                    cmbExam.Items.Add(exam);
                }
                
                cmbExam.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading exams: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExamResults()
        {
            try
            {
                _examResults = _dbManager.GetExamResults();
                
                // Bind to DataGridView
                dgvExamResults.DataSource = null;
                dgvExamResults.DataSource = _examResults;
                
                // Configure columns
                if (dgvExamResults.Columns.Contains("ResultId"))
                {
                    dgvExamResults.Columns["ResultId"].HeaderText = "ID";
                    dgvExamResults.Columns["ResultId"].Width = 50;
                }
                
                if (dgvExamResults.Columns.Contains("StudentId"))
                {
                    dgvExamResults.Columns["StudentId"].Visible = false;
                }
                
                if (dgvExamResults.Columns.Contains("ExamId"))
                {
                    dgvExamResults.Columns["ExamId"].Visible = false;
                }
                
                if (dgvExamResults.Columns.Contains("StudentName"))
                {
                    dgvExamResults.Columns["StudentName"].HeaderText = "Student";
                    dgvExamResults.Columns["StudentName"].Width = 150;
                }
                
                if (dgvExamResults.Columns.Contains("ExamName"))
                {
                    dgvExamResults.Columns["ExamName"].HeaderText = "Exam Name";
                    dgvExamResults.Columns["ExamName"].Width = 200;
                }
                
                if (dgvExamResults.Columns.Contains("Score"))
                {
                    dgvExamResults.Columns["Score"].HeaderText = "Score";
                    dgvExamResults.Columns["Score"].Width = 80;
                    dgvExamResults.Columns["Score"].DefaultCellStyle.Format = "F2";
                }
                
                // Update record count
                lblRecordCount.Text = $"Total Records: {_examResults.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading exam results: {ex.Message}", "Error", 
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
                    pnlResultDetails.Enabled = false;
                    dgvExamResults.Enabled = true;
                    
                    // Set button visibility
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    
                    // Disable top scorer button
                    btnGetTopScorer.Enabled = false;
                    
                    // Clear selection if needed
                    if (_selectedResult == null)
                    {
                        ClearInputs();
                    }
                    break;
                    
                case FormMode.Add:
                    // Disable grid and enable input fields
                    pnlResultDetails.Enabled = true;
                    dgvExamResults.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    
                    // Disable top scorer button
                    btnGetTopScorer.Enabled = false;
                    
                    // Clear inputs for new result
                    ClearInputs();
                    
                    // Focus on first field
                    txtResultId.Focus();
                    break;
                    
                case FormMode.Edit:
                    // Disable grid and enable input fields
                    pnlResultDetails.Enabled = true;
                    dgvExamResults.Enabled = false;
                    
                    // Set button visibility
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    
                    // Enable top scorer button
                    btnGetTopScorer.Enabled = true;
                    
                    // Disable result ID editing
                    txtResultId.ReadOnly = true;
                    break;
            }
        }

        private void ClearInputs()
        {
            txtResultId.Clear();
            txtExamName.Clear();
            txtScore.Clear();
            txtTopScorer.Clear();
            cmbStudent.SelectedIndex = 0;
            cmbExam.SelectedIndex = 0;
            _selectedResult = null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetFormMode(FormMode.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedResult == null)
            {
                MessageBox.Show("Please select an exam result to edit.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            SetFormMode(FormMode.Edit);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedResult == null)
            {
                MessageBox.Show("Please select an exam result to delete.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (MessageBox.Show("Are you sure you want to delete this exam result?", "Confirm Delete", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool success = _dbManager.DeleteExamResult(_selectedResult.ResultId);
                    
                    if (success)
                    {
                        MessageBox.Show("Exam result deleted successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Reload the data
                        LoadExamResults();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete exam result.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting exam result: {ex.Message}", "Error", 
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
                int resultId = int.Parse(txtResultId.Text);
                string examName = txtExamName.Text;
                decimal score = decimal.Parse(txtScore.Text);
                
                // Get selected student
                Student selectedStudent = cmbStudent.SelectedItem as Student;
                if (selectedStudent == null || selectedStudent.StudentId == 0)
                {
                    _errorProvider.SetError(cmbStudent, "Please select a student");
                    return;
                }
                
                // Get selected exam
                Exam selectedExam = cmbExam.SelectedItem as Exam;
                if (selectedExam == null || selectedExam.ExamId == 0)
                {
                    _errorProvider.SetError(cmbExam, "Please select an exam");
                    return;
                }
                
                if (_currentMode == FormMode.Add)
                {
                    // Add new exam result
                    ExamResult newResult = new ExamResult
                    {
                        ResultId = resultId,
                        StudentId = selectedStudent.StudentId,
                        ExamId = selectedExam.ExamId,
                        Score = score,
                        ExamName = examName,
                        StudentName = selectedStudent.StudentName
                    };
                    
                    success = _dbManager.AddExamResult(newResult);
                    
                    if (success)
                    {
                        MessageBox.Show("Exam result added successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (_currentMode == FormMode.Edit && _selectedResult != null)
                {
                    // Update existing exam result
                    ExamResult updatedResult = new ExamResult
                    {
                        ResultId = resultId,
                        StudentId = selectedStudent.StudentId,
                        ExamId = selectedExam.ExamId,
                        Score = score,
                        ExamName = examName,
                        StudentName = selectedStudent.StudentName
                    };
                    
                    success = _dbManager.UpdateExamResult(updatedResult);
                    
                    if (success)
                    {
                        MessageBox.Show("Exam result updated successfully.", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
                if (success)
                {
                    // Reload the data
                    LoadExamResults();
                    SetFormMode(FormMode.View);
                }
                else
                {
                    MessageBox.Show("Failed to save exam result.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving exam result: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // If in edit mode, reload the selected result details
            if (_currentMode == FormMode.Edit && _selectedResult != null)
            {
                LoadResultDetails(_selectedResult);
            }
            
            SetFormMode(FormMode.View);
        }

        private void btnGetTopScorer_Click(object sender, EventArgs e)
        {
            Exam selectedExam = cmbExam.SelectedItem as Exam;
            if (selectedExam == null || selectedExam.ExamId == 0)
            {
                MessageBox.Show("Please select an exam first.", "No Exam Selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            try
            {
                string topScorer = _dbManager.GetTopScorerForExam(selectedExam.ExamId);
                if (!string.IsNullOrEmpty(topScorer))
                {
                    txtTopScorer.Text = topScorer;
                }
                else
                {
                    txtTopScorer.Text = "No results found for this exam";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting top scorer: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // If search is empty, reload all results
                LoadExamResults();
                return;
            }
            
            try
            {
                // Filter the data by student name or exam name
                var filteredResults = _examResults.Where(r => 
                    (r.StudentName != null && r.StudentName.ToLower().Contains(searchTerm)) ||
                    (r.ExamName != null && r.ExamName.ToLower().Contains(searchTerm))
                ).ToList();
                
                dgvExamResults.DataSource = null;
                dgvExamResults.DataSource = filteredResults;
                
                lblRecordCount.Text = $"Total Records: {filteredResults.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching exam results: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadExamResults();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvExamResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _examResults.Count)
            {
                _selectedResult = _examResults[e.RowIndex];
                LoadResultDetails(_selectedResult);
            }
        }

        private void LoadResultDetails(ExamResult result)
        {
            if (result == null)
            {
                return;
            }
            
            txtResultId.Text = result.ResultId.ToString();
            txtExamName.Text = result.ExamName;
            txtScore.Text = result.Score.ToString("F2");
            txtTopScorer.Clear();
            
            // Set student
            if (result.StudentId.HasValue)
            {
                for (int i = 0; i < cmbStudent.Items.Count; i++)
                {
                    if (cmbStudent.Items[i] is Student student && student.StudentId == result.StudentId.Value)
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
            
            // Set exam
            if (result.ExamId.HasValue)
            {
                for (int i = 0; i < cmbExam.Items.Count; i++)
                {
                    if (cmbExam.Items[i] is Exam exam && exam.ExamId == result.ExamId.Value)
                    {
                        cmbExam.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbExam.SelectedIndex = 0; // None
            }
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            
            // Clear previous errors
            _errorProvider.Clear();
            
            // Validate Result ID
            if (string.IsNullOrWhiteSpace(txtResultId.Text))
            {
                _errorProvider.SetError(txtResultId, "Result ID is required");
                isValid = false;
            }
            else if (!int.TryParse(txtResultId.Text, out int id))
            {
                _errorProvider.SetError(txtResultId, "Result ID must be a number");
                isValid = false;
            }
            
            // Validate Exam Name
            if (string.IsNullOrWhiteSpace(txtExamName.Text))
            {
                _errorProvider.SetError(txtExamName, "Exam Name is required");
                isValid = false;
            }
            
            // Validate Score
            if (string.IsNullOrWhiteSpace(txtScore.Text))
            {
                _errorProvider.SetError(txtScore, "Score is required");
                isValid = false;
            }
            else if (!decimal.TryParse(txtScore.Text, out decimal score))
            {
                _errorProvider.SetError(txtScore, "Score must be a number");
                isValid = false;
            }
            else if (score < 0 || score > 100)
            {
                _errorProvider.SetError(txtScore, "Score must be between 0 and 100");
                isValid = false;
            }
            
            // Validate Student selection
            if (cmbStudent.SelectedIndex <= 0) // Not selected or "None"
            {
                _errorProvider.SetError(cmbStudent, "Please select a student");
                isValid = false;
            }
            
            // Validate Exam selection
            if (cmbExam.SelectedIndex <= 0) // Not selected or "None"
            {
                _errorProvider.SetError(cmbExam, "Please select an exam");
                isValid = false;
            }
            
            return isValid;
        }

        private void cmbExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When exam is selected, update exam name field if in Add mode
            if (_currentMode == FormMode.Add && cmbExam.SelectedIndex > 0)
            {
                Exam selectedExam = cmbExam.SelectedItem as Exam;
                if (selectedExam != null && selectedExam.ExamId > 0)
                {
                    // Set a default name based on the course
                    txtExamName.Text = $"{selectedExam.CourseName} Exam";
                }
            }
        }
    }
}
