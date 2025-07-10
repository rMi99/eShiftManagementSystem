using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Services;
using eShiftManagementSystem.Business.Interfaces;

namespace eShiftManagementSystem.Forms
{
    public partial class JobManagementPanel : UserControl
    {
        private readonly JobService _jobService;
        private readonly CustomerService _customerService;
        private Job? _selectedJob;

        // Controls
        private MaterialCard cardJobList;
        private MaterialCard cardJobDetails;
        private DataGridView dgvJobs;
        private MaterialTextBox txtSearch;
        private MaterialComboBox cmbStatusFilter;
        private MaterialButton btnSearch;
        private MaterialButton btnRefresh;
        private MaterialTextBox txtJobNumber;
        private MaterialComboBox cmbCustomer;
        private MaterialTextBox txtPickupAddress;
        private MaterialTextBox txtPickupCity;
        private MaterialTextBox txtDestinationAddress;
        private MaterialTextBox txtDestinationCity;
        private DateTimePicker dtpPickupDate;
        private DateTimePicker dtpDeliveryDate;
        private MaterialComboBox cmbStatus;
        private MaterialTextBox txtSpecialInstructions;
        private MaterialButton btnSave;
        private MaterialButton btnApprove;
        private MaterialButton btnDecline;
        private MaterialButton btnDelete;
        private MaterialLabel lblTitle;
        private MaterialButton btnClear;

        public JobManagementPanel()
        {
            _jobService = new JobService(new JobRepository(), new EmailService(Program.Configuration));
            _customerService = new CustomerService();
            InitializeComponent();
            LoadJobs();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            lblTitle = new MaterialLabel();
            SuspendLayout();
            
            // Title
            lblTitle.Text = "Job Management";
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            lblTitle.Depth = 0;
            lblTitle.FontType = MaterialSkinManager.fontType.H4;
            lblTitle.MouseState = MouseState.HOVER;
            Controls.Add(lblTitle);

            CreateJobListCard();
            CreateJobDetailsCard();
            
            // 
            // JobManagementPanel
            // 
            BackColor = Color.FromArgb(250, 250, 250);
            Name = "JobManagementPanel";
            Size = new Size(1000, 700);
            Load += JobManagementPanel_Load;
            ResumeLayout(false);
        }

        private void CreateJobListCard()
        {
            cardJobList = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(600, 600),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            // Search and filter section
            txtSearch = new MaterialTextBox
            {
                Location = new Point(20, 20),
                Size = new Size(200, 50),
                Hint = "Search jobs...",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbStatusFilter = new MaterialComboBox
            {
                Location = new Point(230, 20),
                Size = new Size(120, 50),
                Hint = "Filter by Status",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbStatusFilter.Items.AddRange(new object[] { "All", "pending", "accepted", "declined", "in_progress", "completed", "cancelled" });
            cmbStatusFilter.SelectedIndex = 0;

            btnSearch = new MaterialButton
            {
                Location = new Point(360, 30),
                Size = new Size(80, 30),
                Text = "SEARCH",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSearch.Click += btnSearch_Click;

            btnRefresh = new MaterialButton
            {
                Location = new Point(450, 30),
                Size = new Size(80, 30),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnRefresh.Click += btnRefresh_Click;

            // Jobs grid
            dgvJobs = new DataGridView
            {
                Location = new Point(20, 90),
                Size = new Size(560, 480),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvJobs.SelectionChanged += dgvJobs_SelectionChanged;

            cardJobList.Controls.Add(txtSearch);
            cardJobList.Controls.Add(cmbStatusFilter);
            cardJobList.Controls.Add(btnSearch);
            cardJobList.Controls.Add(btnRefresh);
            cardJobList.Controls.Add(dgvJobs);
            this.Controls.Add(cardJobList);
        }

        private void CreateJobDetailsCard()
        {
            cardJobDetails = new MaterialCard
            {
                Location = new Point(640, 70),
                Size = new Size(330, 600),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblDetails = new MaterialLabel
            {
                Text = "Job Details",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            // Job details form
            txtJobNumber = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(290, 50),
                Hint = "Job Number",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                ReadOnly = true
            };

            cmbCustomer = new MaterialComboBox
            {
                Location = new Point(20, 120),
                Size = new Size(290, 50),
                Hint = "Customer",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPickupAddress = new MaterialTextBox
            {
                Location = new Point(20, 180),
                Size = new Size(290, 50),
                Hint = "Pickup Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Multiline = true
            };

            txtPickupCity = new MaterialTextBox
            {
                Location = new Point(20, 240),
                Size = new Size(140, 50),
                Hint = "Pickup City",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtDestinationAddress = new MaterialTextBox
            {
                Location = new Point(20, 300),
                Size = new Size(290, 50),
                Hint = "Destination Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Multiline = true
            };

            txtDestinationCity = new MaterialTextBox
            {
                Location = new Point(20, 360),
                Size = new Size(140, 50),
                Hint = "Destination City",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            // Date pickers
            dtpPickupDate = new DateTimePicker
            {
                Location = new Point(170, 240),
                Size = new Size(140, 23),
                Format = DateTimePickerFormat.Short
            };

            dtpDeliveryDate = new DateTimePicker
            {
                Location = new Point(170, 270),
                Size = new Size(140, 23),
                Format = DateTimePickerFormat.Short
            };

            cmbStatus = new MaterialComboBox
            {
                Location = new Point(170, 360),
                Size = new Size(140, 50),
                Hint = "Status",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbStatus.Items.AddRange(new object[] { "pending", "accepted", "declined", "in_progress", "completed", "cancelled" });

            txtSpecialInstructions = new MaterialTextBox
            {
                Location = new Point(20, 420),
                Size = new Size(290, 50),
                Hint = "Special Instructions",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Multiline = true
            };

            // Action buttons
            btnSave = new MaterialButton
            {
                Location = new Point(20, 490),
                Size = new Size(60, 36),
                Text = "SAVE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSave.Click += btnSave_Click;

            btnApprove = new MaterialButton
            {
                Location = new Point(90, 490),
                Size = new Size(80, 36),
                Text = "APPROVE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnApprove.Click += btnApprove_Click;

            btnDecline = new MaterialButton
            {
                Location = new Point(180, 490),
                Size = new Size(70, 36),
                Text = "DECLINE",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = true,
                HighEmphasis = false
            };
            btnDecline.Click += btnDecline_Click;

            btnDelete = new MaterialButton
            {
                Location = new Point(20, 540),
                Size = new Size(70, 36),
                Text = "DELETE",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = true,
                HighEmphasis = false
            };
            btnDelete.Click += btnDelete_Click;

            btnClear = new MaterialButton
            {
                Location = new Point(100, 540),
                Size = new Size(60, 36),
                Text = "CLEAR",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnClear.Click += btnClear_Click;

            // Add all controls to details card
            cardJobDetails.Controls.Add(lblDetails);
            cardJobDetails.Controls.Add(txtJobNumber);
            cardJobDetails.Controls.Add(cmbCustomer);
            cardJobDetails.Controls.Add(txtPickupAddress);
            cardJobDetails.Controls.Add(txtPickupCity);
            cardJobDetails.Controls.Add(txtDestinationAddress);
            cardJobDetails.Controls.Add(txtDestinationCity);
            cardJobDetails.Controls.Add(dtpPickupDate);
            cardJobDetails.Controls.Add(dtpDeliveryDate);
            cardJobDetails.Controls.Add(cmbStatus);
            cardJobDetails.Controls.Add(txtSpecialInstructions);
            cardJobDetails.Controls.Add(btnSave);
            cardJobDetails.Controls.Add(btnApprove);
            cardJobDetails.Controls.Add(btnDecline);
            cardJobDetails.Controls.Add(btnDelete);
            cardJobDetails.Controls.Add(btnClear);

            this.Controls.Add(cardJobDetails);
        }

        private void LoadJobs()
        {
            try
            {
                var jobs = _jobService.GetAllJobs();
                var jobData = jobs.Select(j => new
                {
                    JobId = j.JobId,
                    JobNumber = j.JobNumber,
                    CustomerName = j.Customer != null ? j.Customer.FullName : "Unknown",
                    PickupCity = j.PickupCity,
                    DestinationCity = j.DestinationCity,
                    Status = j.Status,
                    RequestedDate = j.RequestedPickupDate.ToString("dd/MM/yyyy"),
                    CreatedAt = j.CreatedAt.ToString("dd/MM/yyyy")
                }).ToList();

                dgvJobs.DataSource = jobData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading jobs: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = _customerService.GetAllCustomers();
                cmbCustomer.DataSource = customers.Select(c => new { Text = c.FullName, Value = c.CustomerId }).ToList();
                cmbCustomer.DisplayMember = "Text";
                cmbCustomer.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading customers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var allJobs = _jobService.GetAllJobs();
                var filteredJobs = new List<Job>(allJobs);

                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    var searchTerm = txtSearch.Text.Trim().ToLower();
                    filteredJobs = filteredJobs.Where(j =>
                        (j.JobNumber?.ToLower().Contains(searchTerm) ?? false) ||
                        (j.Customer?.FullName?.ToLower().Contains(searchTerm) ?? false) ||
                        (j.PickupCity?.ToLower().Contains(searchTerm) ?? false) ||
                        (j.DestinationCity?.ToLower().Contains(searchTerm) ?? false)
                    ).ToList();
                }

                if (cmbStatusFilter.SelectedIndex > 0)
                {
                    var selectedStatus = cmbStatusFilter.SelectedItem.ToString();
                    filteredJobs = filteredJobs.Where(j => j.Status == selectedStatus).ToList();
                }

                var jobData = filteredJobs.Select(j => new
                {
                    JobId = j.JobId,
                    JobNumber = j.JobNumber,
                    CustomerName = j.Customer != null ? j.Customer.FullName : "Unknown",
                    PickupCity = j.PickupCity,
                    DestinationCity = j.DestinationCity,
                    Status = j.Status,
                    RequestedDate = j.RequestedPickupDate.ToString("dd/MM/yyyy"),
                    CreatedAt = j.CreatedAt.ToString("dd/MM/yyyy")
                }).ToList();

                dgvJobs.DataSource = jobData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error searching jobs: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cmbStatusFilter.SelectedIndex = 0;
            LoadJobs();
            ClearForm();
        }

        private void dgvJobs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJobs.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedRow = dgvJobs.SelectedRows[0];
                    if (selectedRow.DataBoundItem != null)
                    {
                        var jobId = (int)selectedRow.Cells["JobId"].Value;
                        _selectedJob = _jobService.GetJobById(jobId);
                        if (_selectedJob != null)
                        {
                            PopulateForm(_selectedJob);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error loading job details: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PopulateForm(Job job)
        {
            txtJobNumber.Text = job.JobNumber;

            cmbCustomer.SelectedValue = job.CustomerId;

            txtPickupAddress.Text = job.PickupAddress;
            txtPickupCity.Text = job.PickupCity;
            txtDestinationAddress.Text = job.DestinationAddress;
            txtDestinationCity.Text = job.DestinationCity;
            dtpPickupDate.Value = job.RequestedPickupDate;
            if (job.RequestedDeliveryDate.HasValue)
            {
                dtpDeliveryDate.Value = job.RequestedDeliveryDate.Value;
            }
            cmbStatus.SelectedItem = job.Status;
            txtSpecialInstructions.Text = job.SpecialInstructions;
        }

        private void ClearForm()
        {
            txtJobNumber.Text = "";
            cmbCustomer.SelectedIndex = -1;
            txtPickupAddress.Text = "";
            txtPickupCity.Text = "";
            txtDestinationAddress.Text = "";
            txtDestinationCity.Text = "";
            dtpPickupDate.Value = DateTime.Now;
            dtpDeliveryDate.Value = DateTime.Now;
            cmbStatus.SelectedIndex = -1;
            txtSpecialInstructions.Text = "";
            _selectedJob = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_selectedJob == null) return;

            try
            {
                _selectedJob.PickupAddress = txtPickupAddress.Text.Trim();
                _selectedJob.PickupCity = txtPickupCity.Text.Trim();
                _selectedJob.DestinationAddress = txtDestinationAddress.Text.Trim();
                _selectedJob.DestinationCity = txtDestinationCity.Text.Trim();
                _selectedJob.RequestedPickupDate = dtpPickupDate.Value;
                _selectedJob.RequestedDeliveryDate = dtpDeliveryDate.Value;
                _selectedJob.Status = cmbStatus.SelectedItem?.ToString() ?? "pending";
                _selectedJob.SpecialInstructions = txtSpecialInstructions.Text.Trim();

                _jobService.UpdateJob(_selectedJob);
                MaterialMessageBox.Show("Job updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadJobs();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving job: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (_selectedJob == null) return;

            try
            {
                _jobService.UpdateJobStatus(_selectedJob.JobId, "accepted");
                MaterialMessageBox.Show("Job approved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadJobs();
                ClearForm();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error approving job: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecline_Click(object sender, EventArgs e)
        {
            if (_selectedJob == null) return;

            var result = MaterialMessageBox.Show("Are you sure you want to decline this job?", "Confirm Decline",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _jobService.UpdateJobStatus(_selectedJob.JobId, "declined");
                    MaterialMessageBox.Show("Job declined successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadJobs();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error declining job: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedJob == null) return;

            var result = MaterialMessageBox.Show($"Are you sure you want to delete job '{_selectedJob.JobNumber}'?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _jobService.DeleteJob(_selectedJob.JobId);
                    MaterialMessageBox.Show("Job deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadJobs();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error deleting job: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void JobManagementPanel_Load(object sender, EventArgs e)
        {

        }
    }
}