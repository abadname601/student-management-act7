using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

namespace StudentManagementSystem.Utilities
{
    public class ExcelExporter
    {
        /// <summary>
        /// Exports a DataTable to Excel with formatting and filtering options using EPPlus
        /// </summary>
        public static void ExportToExcel(DataTable dt, string title, string filePath = null)
        {
            try
            {
                // Set the license context (required for EPPlus)
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                
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

                using (var package = new ExcelPackage())
                {
                    // Add a new worksheet to the workbook
                    var worksheet = package.Workbook.Worksheets.Add("Report");

                    // Report title
                    worksheet.Cells["A1:G1"].Merge = true;
                    worksheet.Cells["A1"].Value = title;
                    using (var range = worksheet.Cells["A1:G1"])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Size = 16;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    }

                    // Generated date
                    worksheet.Cells["A2:G2"].Merge = true;
                    worksheet.Cells["A2"].Value = "Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    using (var range = worksheet.Cells["A2:G2"])
                    {
                        range.Style.Font.Italic = true;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    // Add column headers at row 4
                    int colIndex = 1;
                    foreach (DataColumn column in dt.Columns)
                    {
                        worksheet.Cells[4, colIndex].Value = column.ColumnName;
                        using (var cell = worksheet.Cells[4, colIndex])
                        {
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        colIndex++;
                    }

                    // Add data starting from row 5
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            worksheet.Cells[i + 5, j + 1].Value = dt.Rows[i][j].ToString();
                            using (var cell = worksheet.Cells[i + 5, j + 1])
                            {
                                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                // For numeric values, align right
                                if (dt.Columns[j].DataType == typeof(int) || 
                                    dt.Columns[j].DataType == typeof(decimal) || 
                                    dt.Columns[j].DataType == typeof(double))
                                {
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0.00";
                                }
                            }
                        }

                        // Alternate row coloring
                        if (i % 2 == 1)
                        {
                            using (var rowRange = worksheet.Cells[i + 5, 1, i + 5, dt.Columns.Count])
                            {
                                rowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rowRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 240, 240));
                            }
                        }
                    }

                    // Add table with filtering
                    var dataRange = worksheet.Cells[4, 1, dt.Rows.Count + 4, dt.Columns.Count];
                    var table = worksheet.Tables.Add(dataRange, "DataTable");
                    table.ShowFilter = true;
                    table.TableStyle = TableStyles.Medium2;

                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Save the file
                    package.SaveAs(new FileInfo(filePath));
                }

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
