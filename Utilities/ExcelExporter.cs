using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Collections.Generic;

namespace StudentManagementSystem.Utilities
{
    public class ExcelExporter
    {
        /// <summary>
        /// Exports a DataTable to Excel with formatting and filtering options
        /// </summary>
        public static void ExportToExcel(DataTable dt, string title, string filePath = null)
        {
            try
            {
                // If no file path is provided, ask the user
                if (string.IsNullOrEmpty(filePath))
                {
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx",
                        Title = "Save Excel File",
                        FileName = title.Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd")
                    };

                    if (saveDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    filePath = saveDialog.FileName;
                }

                // Start Excel and create a new workbook
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.DisplayAlerts = false;
                Workbook workbook = excel.Workbooks.Add(Type.Missing);
                Worksheet worksheet = workbook.ActiveSheet;

                // Report title
                Range headerRange = worksheet.Range["A1", "G1"];
                headerRange.Merge();
                headerRange.Value = title;
                headerRange.Font.Bold = true;
                headerRange.Font.Size = 16;
                headerRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                headerRange.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);

                // Generated date
                Range dateRange = worksheet.Range["A2", "G2"];
                dateRange.Merge();
                dateRange.Value = "Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dateRange.Font.Italic = true;
                dateRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                // Add column headers at row 4
                int colIndex = 1;
                foreach (DataColumn column in dt.Columns)
                {
                    worksheet.Cells[4, colIndex] = column.ColumnName;
                    Range colHeader = worksheet.Cells[4, colIndex];
                    colHeader.Font.Bold = true;
                    colHeader.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                    colHeader.Borders.LineStyle = XlLineStyle.xlContinuous;
                    colHeader.Borders.Weight = XlBorderWeight.xlThin;
                    colHeader.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    colIndex++;
                }

                // Add data starting from row 5
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 5, j + 1] = dt.Rows[i][j].ToString();
                        Range cell = worksheet.Cells[i + 5, j + 1];
                        cell.Borders.LineStyle = XlLineStyle.xlContinuous;
                        cell.Borders.Weight = XlBorderWeight.xlThin;

                        // For numeric values, align right
                        if (dt.Columns[j].DataType == typeof(int) || 
                            dt.Columns[j].DataType == typeof(decimal) || 
                            dt.Columns[j].DataType == typeof(double))
                        {
                            cell.HorizontalAlignment = XlHAlign.xlHAlignRight;
                            cell.NumberFormat = "#,##0.00";
                        }
                    }

                    // Alternate row coloring
                    if (i % 2 == 1)
                    {
                        Range rowRange = worksheet.Range[worksheet.Cells[i + 5, 1], worksheet.Cells[i + 5, dt.Columns.Count]];
                        rowRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(240, 240, 240));
                    }
                }

                // Add AutoFilter to the header row
                Range dataRange = worksheet.Range[worksheet.Cells[4, 1], worksheet.Cells[dt.Rows.Count + 4, dt.Columns.Count]];
                dataRange.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);

                // Auto-fit columns
                worksheet.Columns.AutoFit();

                // Save and close
                workbook.SaveAs(filePath);
                workbook.Close();
                excel.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                MessageBox.Show($"Report has been exported to: {filePath}", "Export Successful", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Exports data from a DataGridView to Excel with formatting and filtering options
        /// </summary>
        public static void ExportDataGridViewToExcel(DataGridView dgv, string title, string filePath = null)
        {
            try
            {
                // Convert DataGridView to DataTable
                DataTable dt = new DataTable();
                
                // Add columns
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    if (column.Visible)
                    {
                        dt.Columns.Add(column.HeaderText);
                    }
                }

                // Add rows
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataRow dataRow = dt.NewRow();
                        int colIndex = 0;
                        
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            if (dgv.Columns[i].Visible)
                            {
                                dataRow[colIndex] = row.Cells[i].Value?.ToString() ?? "";
                                colIndex++;
                            }
                        }
                        
                        dt.Rows.Add(dataRow);
                    }
                }

                // Use the existing method to export the DataTable
                ExportToExcel(dt, title, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Export Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
