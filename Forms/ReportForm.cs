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
    public partial class ReportForm : Form
    {
        private DatabaseManager _dbManager;
        
        public ReportForm()
        {
            InitializeComponent();
            _dbManager = new DatabaseManager();
        }
        
        private void ReportForm_Load(object sender, EventArgs e)
        {
            // Apply styling
            UIHelper.StyleForm(this);
            UIHelper.StyleDataGridView(dgvReportData);
            
            // Initialize reports
            LoadReportTypes();
        }
        
        private void LoadReportTypes()
        {
            // Add report types to combo box
            cmbReportType.Items.Clear();
            cmbReportType.Items.Add("-- Select Report Type --");
            cmbReportType.Items.Add("Student List");
            cmbReportType.Items.Add("Course List");
            cmbReportType.Items.Add("Exam Results");
            cmbReportType.Items.Add("Department Summary");
            cmbReportType.SelectedIndex = 0;
        }
        
        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedIndex <= 0)
            {
                dgvReportData.DataSource = null;
                return;
            }
            
            string reportType = cmbReportType.SelectedItem.ToString();
            DataTable reportData = null;
            
            try
            {
                switch (reportType)
                {
                    case "Student List":
                        reportData = _dbManager.GetStudentListReport();
                        break;
                    case "Course List":
                        reportData = _dbManager.GetCourseListReport();
                        break;
                    case "Exam Results":
                        reportData = _dbManager.GetExamResultsReport();
                        break;
                    case "Department Summary":
                        reportData = _dbManager.GetDepartmentSummaryReport();
                        break;
                }
                
                if (reportData != null)
                {
                    dgvReportData.DataSource = reportData;
                    lblRecordCount.Text = $"Total Records: {reportData.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (dgvReportData.DataSource == null || dgvReportData.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string reportType = cmbReportType.SelectedItem.ToString();
            
            try
            {
                string fileName = $"{reportType}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel Files|*.xlsx";
                saveDialog.FileName = fileName;
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable data = (DataTable)dgvReportData.DataSource;
                    
                    // Use the static method directly - it already shows success message
                    ExcelExporter.ExportToExcel(data, reportType, saveDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterReportData();
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterReportData();
        }
        
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            FilterReportData();
        }
        
        private void FilterReportData()
        {
            try
            {
                if (dgvReportData.DataSource == null)
                    return;
                
                string searchText = txtSearch.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(searchText))
                {
                    // Reset filter
                    ((DataTable)dgvReportData.DataSource).DefaultView.RowFilter = "";
                    lblRecordCount.Text = $"Total Records: {((DataTable)dgvReportData.DataSource).Rows.Count}";
                    return;
                }
                
                // Build filter expression for all string columns
                DataTable dt = (DataTable)dgvReportData.DataSource;
                string filterExpression = "";
                
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.DataType == typeof(string))
                    {
                        if (!string.IsNullOrEmpty(filterExpression))
                            filterExpression += " OR ";
                            
                        filterExpression += $"[{column.ColumnName}] LIKE '%{searchText}%'";
                    }
                }
                
                dt.DefaultView.RowFilter = filterExpression;
                lblRecordCount.Text = $"Total Records: {dt.DefaultView.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering data: {ex.Message}", "Filter Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}