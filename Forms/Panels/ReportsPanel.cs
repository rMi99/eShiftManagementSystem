using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace eShiftManagementSystem.Forms
{
    public partial class ReportsPanel : UserControl
    {
        private readonly ReportRepository _reportRepository;
        private readonly JobRepository _jobRepository;
        private readonly CustomerRepository _customerRepository;

        // Controls
        private MaterialCard cardReportTypes;
        private MaterialCard cardReportData;
        private MaterialComboBox cmbReportType;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private MaterialButton btnGenerate;
        private DataGridView dgvReportData;
        private MaterialButton btnExport;

        public ReportsPanel()
        {
            _reportRepository = new ReportRepository();
            _jobRepository = new JobRepository();
            _customerRepository = new CustomerRepository();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1000, 700);

            var lblTitle = new MaterialLabel
            {
                Text = "Reports & Analytics",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            CreateReportTypesCard();
            CreateReportDataCard();
        }

        private void CreateReportTypesCard()
        {
            cardReportTypes = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(960, 120),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblReportType = new MaterialLabel
            {
                Text = "Report Configuration",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            cmbReportType = new MaterialComboBox
            {
                Location = new Point(20, 60),
                Size = new Size(200, 50),
                Hint = "Select Report Type",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbReportType.Items.AddRange(new object[] { 
                "Job Statistics", 
                "Customer Report", 
                "Revenue Report", 
                "Monthly Summary", 
                "Top Customers" 
            });

            dtpStartDate = new DateTimePicker
            {
                Location = new Point(240, 60),
                Size = new Size(150, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(-1)
            };

            dtpEndDate = new DateTimePicker
            {
                Location = new Point(410, 60),
                Size = new Size(150, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };

            btnGenerate = new MaterialButton
            {
                Location = new Point(580, 55),
                Size = new Size(100, 36),
                Text = "GENERATE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnGenerate.Click += btnGenerate_Click;

            btnExport = new MaterialButton
            {
                Location = new Point(690, 55),
                Size = new Size(100, 36),
                Text = "EXPORT",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnExport.Click += btnExport_Click;

            cardReportTypes.Controls.Add(lblReportType);
            cardReportTypes.Controls.Add(cmbReportType);
            cardReportTypes.Controls.Add(dtpStartDate);
            cardReportTypes.Controls.Add(dtpEndDate);
            cardReportTypes.Controls.Add(btnGenerate);
            cardReportTypes.Controls.Add(btnExport);

            this.Controls.Add(cardReportTypes);
        }

        private void CreateReportDataCard()
        {
            cardReportData = new MaterialCard
            {
                Location = new Point(20, 210),
                Size = new Size(960, 460),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblReportData = new MaterialLabel
            {
                Text = "Report Data",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            dgvReportData = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(920, 380),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            cardReportData.Controls.Add(lblReportData);
            cardReportData.Controls.Add(dgvReportData);

            this.Controls.Add(cardReportData);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedIndex == -1)
            {
                MaterialMessageBox.Show("Please select a report type.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var reportType = cmbReportType.SelectedItem.ToString();
                
                switch (reportType)
                {
                    case "Job Statistics":
                        GenerateJobStatistics();
                        break;
                    case "Customer Report":
                        GenerateCustomerReport();
                        break;
                    case "Revenue Report":
                        GenerateRevenueReport();
                        break;
                    case "Monthly Summary":
                        GenerateMonthlySummary();
                        break;
                    case "Top Customers":
                        GenerateTopCustomers();
                        break;
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateJobStatistics()
        {
            var statistics = _reportRepository.GetJobStatusStatistics();
            var data = statistics.Select(s => new { Status = s.Key, Count = s.Value }).ToList();
            dgvReportData.DataSource = data;
        }

        private void GenerateCustomerReport()
        {
            var customers = _reportRepository.GetCustomersByRegistrationDate(dtpStartDate.Value, dtpEndDate.Value);
            var data = customers.Select(c => new
            {
                Name = c.FullName,
                Email = c.User != null ? c.User.Email : "",
                Phone = c.Phone,
                City = c.City,
                RegistrationDate = c.RegistrationDate.ToString("dd/MM/yyyy")
            }).ToList();
            dgvReportData.DataSource = data;
        }

        private void GenerateRevenueReport()
        {
            var revenue = _reportRepository.GetRevenueByMonth(dtpStartDate.Value.Year);
            var data = revenue.Select(r => new { Month = r.Key, Revenue = r.Value }).ToList();
            dgvReportData.DataSource = data;
        }

        private void GenerateMonthlySummary()
        {
            var jobs = _reportRepository.GetJobsByDateRange(dtpStartDate.Value, dtpEndDate.Value);
            var summary = jobs.GroupBy(j => j.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalJobs = g.Count(),
                    CompletedJobs = g.Count(j => j.Status == "completed"),
                    PendingJobs = g.Count(j => j.Status == "pending")
                }).ToList();
            dgvReportData.DataSource = summary;
        }

        private void GenerateTopCustomers()
        {
            var topCustomers = _reportRepository.GetTopCustomers(10);
            dgvReportData.DataSource = topCustomers;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            MaterialMessageBox.Show("Export functionality will be implemented in future updates.", "Information", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}