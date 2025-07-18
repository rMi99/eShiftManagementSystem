using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Windows.Forms;
using MaterialSkin.Controls;
using System.Drawing;

namespace eShiftManagementSystem.Services
{
    public class PdfService
    {
        public void GeneratePdfReport(DataGridView dgv, string title)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = $"{title.Replace(" ", "_")}_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Corrected: Use Document.Create to build the document
                    Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Margin(50);
                            page.Header().Element(header => ComposeHeader(header, title));
                            page.Content().Element(content => ComposeContent(content, dgv));
                            page.Footer().Element(ComposeFooter);
                        });
                    })
                    .GeneratePdf(saveFileDialog.FileName); // Chain the GeneratePdf method here

                    MaterialMessageBox.Show("PDF report generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"An error occurred while generating the PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ComposeHeader(IContainer container, string title)
        {
            var textStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(title).Style(textStyle);
                    column.Item().Text($"Report Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                });
            });
        }

        private void ComposeContent(IContainer container, DataGridView dgv)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);
                column.Item().Element(table => ComposeTable(table, dgv));
            });
        }

        private void ComposeTable(IContainer container, DataGridView dgv)
        {
            var headerStyle = TextStyle.Default.SemiBold();

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        columns.RelativeColumn();
                    }
                });

                table.Header(header =>
                {
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(5).Text(column.HeaderText).Style(headerStyle);
                    }
                });

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        table.Cell().Border(1).Padding(5).Text(cell.Value?.ToString() ?? string.Empty);
                    }
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
            });
        }
    }
}