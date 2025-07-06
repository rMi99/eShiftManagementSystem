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
    public partial class DriverJobHistoryForm : MaterialForm
    {
        private readonly JobRepository _jobRepository;
        private readonly DriverRepository _driverRepository;
        private readonly int _driverId;
        private readonly MaterialSkinManager materialSkinManager;

        // Controls
        private MaterialLabel lblTitle;
        private MaterialLabel lblDriverInfo;
        private DataGridView dgvJobHistory;
        private MaterialComboBox cmbStatusFilter;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private MaterialButton btnFilter;
        private MaterialButton btnRefresh;
        private MaterialButton btnViewDetails;
        private MaterialButton btnClose;

        public DriverJobHistoryForm(int driverId)
        {
            _driverId = driverId;
            _jobRepository = new JobRepository();
            _driverRepository = new DriverRepository();
            InitializeComponent();

            // Initialize Material Design
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

            LoadDriverInfo();
            LoadJobHistory();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 600);
            this.Text = "Driver Job History";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(900, 600);

            lblTitle = new MaterialLabel
            {
                Text = "Driver Job History",
                Location = new Point(30, 80),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H5,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            lblDriverInfo = new MaterialLabel
            {
                Text = "Driver: Loading...",
                Location = new Point(30, 110),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Body1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            var lblFromDate = new MaterialLabel
            {
                Text = "From Date:",
                Location = new Point(30, 140),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Caption,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(100, 140),
                Size = new Size(120, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(-3)
            };

            var lblToDate = new MaterialLabel
            {
                Text = "To Date:",
                Location = new Point(240, 140),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Caption,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            dtpToDate = new DateTimePicker
            {
                Location = new Point(300, 140),
                Size = new Size(120, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };

            cmbStatusFilter = new MaterialComboBox
            {
                Location = new Point(440, 130),
                Size = new Size(150, 50),
                Hint = "Filter by Status",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbStatusFilter.Items.AddRange(new object[] { 
                "All", "completed", "delivered", "in_progress", "cancelled" 
            });
            cmbStatusFilter.SelectedItem = "All";

            btnFilter = new MaterialButton
            {
                Location = new Point(610, 140),
                Size = new Size(70, 30),
                Text = "FILTER",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnFilter.Click += btnFilter_Click;

            btnRefresh = new MaterialButton
            {
                Location = new Point(690, 140),
                Size = new Size(70, 30),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnRefresh.Click += btnRefresh_Click;

            dgvJobHistory = new DataGridView
            {
                Location = new Point(30, 180),
                Size = new Size(840, 350),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvJobHistory.SelectionChanged += DgvJobHistory_SelectionChanged;

            btnViewDetails = new MaterialButton
            {
                Location = new Point(700, 540),
                Size = new Size(90, 36),
                Text = "VIEW DETAILS",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true,
                Enabled = false
            };
            btnViewDetails.Click += btnViewDetails_Click;

            btnClose = new MaterialButton
            {
                Location = new Point(800, 540),
                Size = new Size(70, 36),
                Text = "CLOSE",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnClose.Click += btnClose_Click;

            this.Controls.AddRange(new Control[] {
                lblTitle, lblDriverInfo, lblFromDate, dtpFromDate, lblToDate, dtpToDate,
                cmbStatusFilter, btnFilter, btnRefresh, dgvJobHistory, btnViewDetails, btnClose
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadDriverInfo()
        {
            try
            {
                var driver = _driverRepository.GetDriverById(_driverId);
                if (driver is not null)
                {
                    lblDriverInfo.Text = $"Driver: {driver.FullName} | License: {driver.LicenseNumber} | Status: {driver.Status}";
                    this.Text = $"Job History - {driver.FullName}";
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading driver information: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadJobHistory()
        {
            try
            {
                var jobs = _jobRepository.GetJobsByDriverId(_driverId);
                
                // Apply date filter
                jobs = jobs.Where(j => j.CreatedAt >= dtpFromDate.Value.Date && 
                                      j.CreatedAt <= dtpToDate.Value.Date.AddDays(1)).ToList();

                // Apply status filter
                string statusFilter = cmbStatusFilter.SelectedItem?.ToString() ?? "All";
                if (!string.Equals(statusFilter, "All", StringComparison.OrdinalIgnoreCase))
                {
                    jobs = jobs.Where(j => string.Equals(j.Status, statusFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                var jobData = jobs.Select(j => new
                {
                    JobId = j.JobId,
                    JobNumber = j.JobNumber,
                    CustomerName = j.Customer?.FullName ?? j.CustomerName,
                    PickupCity = j.PickupCity,
                    DestinationCity = j.DestinationCity,
                    Status = j.Status,
                    Priority = j.Priority,
                    RequestedDate = j.RequestedPickupDate.ToString("dd/MM/yyyy"),
                    CompletedDate = j.UpdatedAt.ToString("dd/MM/yyyy"),
                    SpecialInstructions = string.IsNullOrEmpty(j.SpecialInstructions) ? "None" : "Yes"
                }).OrderByDescending(j => j.RequestedDate).ToList();

                dgvJobHistory.DataSource = jobData;

                // Customize column headers
                if (dgvJobHistory.Columns["JobId"] is not null)
                    dgvJobHistory.Columns["JobId"].Visible = false;
                if (dgvJobHistory.Columns["JobNumber"] is not null)
                    dgvJobHistory.Columns["JobNumber"].HeaderText = "Job #";
                if (dgvJobHistory.Columns["CustomerName"] is not null)
                    dgvJobHistory.Columns["CustomerName"].HeaderText = "Customer";
                if (dgvJobHistory.Columns["PickupCity"] is not null)
                    dgvJobHistory.Columns["PickupCity"].HeaderText = "From";
                if (dgvJobHistory.Columns["DestinationCity"] is not null)
                    dgvJobHistory.Columns["DestinationCity"].HeaderText = "To";
                if (dgvJobHistory.Columns["RequestedDate"] is not null)
                    dgvJobHistory.Columns["RequestedDate"].HeaderText = "Requested";
                if (dgvJobHistory.Columns["CompletedDate"] is not null)
                    dgvJobHistory.Columns["CompletedDate"].HeaderText = "Last Updated";
                if (dgvJobHistory.Columns["SpecialInstructions"] is not null)
                    dgvJobHistory.Columns["SpecialInstructions"].HeaderText = "Special Notes";
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading job history: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadJobHistory();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadJobHistory();
        }

        private void DgvJobHistory_SelectionChanged(object sender, EventArgs e)
        {
            btnViewDetails.Enabled = dgvJobHistory.SelectedRows.Count > 0;
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvJobHistory.SelectedRows.Count == 0) return;

            try
            {
                var selectedRow = dgvJobHistory.SelectedRows[0];
                int jobId = Convert.ToInt32(selectedRow.Cells["JobId"].Value);

                // Here you could open a job details form
                MaterialMessageBox.Show($"Job details for Job ID: {jobId}", "Job Details", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error viewing job details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}