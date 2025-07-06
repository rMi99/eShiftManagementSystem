using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace eShiftManagementSystem.Forms
{
    public partial class CustomerJobsForm : MaterialForm
    {
        private readonly JobService _jobService;
        private readonly MaterialSkinManager materialSkinManager;
        private int _customerId;

        // Controls
        private DataGridView dgvJobs;
        private MaterialButton btnRefresh;
        private MaterialButton btnNewJob;
        private MaterialButton btnViewDetails;
        private MaterialButton btnClose;

        public CustomerJobsForm(int customerId)
        {
            _customerId = customerId;
            _jobService = new JobService();
            
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green100, Accent.Orange200, TextShade.WHITE);
            
            InitializeComponent();
            LoadJobs();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "My Jobs";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            var lblTitle = new MaterialLabel
            {
                Text = "My Jobs",
                Location = new Point(30, 80),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            dgvJobs = new DataGridView
            {
                Location = new Point(30, 130),
                Size = new Size(840, 400),
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

            btnRefresh = new MaterialButton
            {
                Location = new Point(30, 550),
                Size = new Size(80, 36),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnRefresh.Click += btnRefresh_Click;

            btnNewJob = new MaterialButton
            {
                Location = new Point(120, 550),
                Size = new Size(100, 36),
                Text = "NEW JOB",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnNewJob.Click += btnNewJob_Click;

            btnViewDetails = new MaterialButton
            {
                Location = new Point(230, 550),
                Size = new Size(120, 36),
                Text = "VIEW DETAILS",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnViewDetails.Click += btnViewDetails_Click;

            btnClose = new MaterialButton
            {
                Location = new Point(790, 550),
                Size = new Size(80, 36),
                Text = "CLOSE",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblTitle, dgvJobs, btnRefresh, btnNewJob, btnViewDetails, btnClose
            });

            this.ResumeLayout(false);
        }

        private void LoadJobs()
        {
            try
            {
                var jobs = _jobService.GetJobsByCustomerId(_customerId);
                var jobData = jobs.Select(j => new
                {
                    JobId = j.JobId,
                    JobNumber = j.JobNumber,
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadJobs();
        }

        private void btnNewJob_Click(object sender, EventArgs e)
        {
            var createJobForm = new CreateJobForm();
            if (createJobForm.ShowDialog() == DialogResult.OK)
            {
                LoadJobs();
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvJobs.SelectedRows.Count > 0)
            {
                var selectedRow = dgvJobs.SelectedRows[0];
                var jobId = Convert.ToInt32(selectedRow.Cells["JobId"].Value);
                
                MaterialMessageBox.Show($"Job details view will be implemented for Job ID: {jobId}", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MaterialMessageBox.Show("Please select a job to view details.", "Selection Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}