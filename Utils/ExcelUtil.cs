using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MaterialSkin.Controls;

namespace eShiftManagementSystem.Services
{
    public class ExcelService
    {
        public void GenerateExcelReport(DataGridView dgv, string title, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set license context

            try
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Report");

                    // Add Title
                    worksheet.Cells["A1"].Value = title;
                    worksheet.Cells["A1"].Style.Font.Size = 16;
                    worksheet.Cells["A1"].Style.Font.Bold = true;

                    // Add Generation Time
                    worksheet.Cells["A2"].Value = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    worksheet.Cells["A2"].Style.Font.Size = 10;
                    worksheet.Cells["A2"].Style.Font.Italic = true;

                    // Add Headers
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        worksheet.Cells[4, i + 1].Value = dgv.Columns[i].HeaderText;
                        worksheet.Cells[4, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[4, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[4, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[4, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    // Add Data
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgv.Columns.Count; j++)
                        {
                            worksheet.Cells[i + 5, j + 1].Value = dgv.Rows[i].Cells[j].Value?.ToString();
                            worksheet.Cells[i + 5, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }
                    }

                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Save the file
                    File.WriteAllBytes(filePath, package.GetAsByteArray());

                    MaterialMessageBox.Show("Excel report generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"An error occurred while generating the Excel file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}