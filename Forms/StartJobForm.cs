using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace eShiftManagementSystem.Forms
{
    public partial class StartJobForm : MaterialForm
    {
        private readonly JobRepository _jobRepository;
        private readonly DriverRepository _driverRepository;
        private readonly int _jobId;
        private readonly MaterialSkinManager materialSkinManager;
        private Job? _job;

        // Controls
        private MaterialLabel lblTitle;
        private MaterialLabel lblJobDetails;
        private MaterialTextBox txtJobNumber;
        private MaterialTextBox txtCustomerName;
        private MaterialTextBox txtPickupAddress;
        private MaterialTextBox txtDestinationAddress;
        private MaterialComboBox cmbAssignedDriver;
        private DateTimePicker dtpStartTime;
        private MaterialMultiLineTextBox txtStartNotes;
        private MaterialButton btnStartJob;
        private MaterialButton btnCancel;

        public StartJobForm(int jobId)
        {
            _jobId = jobId;
            _jobRepository = new JobRepository();
            _driverRepository = new DriverRepository();
            InitializeComponent();

            // Initialize Material Design
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

            LoadJobDetails();
            LoadDrivers();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(500, 550);
            this.Text = "Start Job";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblTitle = new MaterialLabel
            {
                Text = "Start Job",
                Location = new Point(30, 80),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H5,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            lblJobDetails = new MaterialLabel
            {
                Text = "Job Details",
                Location = new Point(30, 120),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtJobNumber = new MaterialTextBox
            {
                Location = new Point(30, 150),
                Size = new Size(190, 50),
                Hint = "Job Number",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                ReadOnly = true
            };

            txtCustomerName = new MaterialTextBox
            {
                Location = new Point(280, 150),
                Size = new Size(190, 50),
                Hint = "Customer Name",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                ReadOnly = true
            };

            txtPickupAddress = new MaterialTextBox
            {
                Location = new Point(30, 210),
                Size = new Size(440, 50),
                Hint = "Pickup Address",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                ReadOnly = true
            };

            txtDestinationAddress = new MaterialTextBox
            {
                Location = new Point(30, 270),
                Size = new Size(440, 50),
                Hint = "Destination Address",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                ReadOnly = true
            };

            cmbAssignedDriver = new MaterialComboBox
            {
                Location = new Point(30, 330),
                Size = new Size(190, 50),
                Hint = "Assigned Driver",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            dtpStartTime = new DateTimePicker
            {
                Location = new Point(280, 340),
                Size = new Size(190, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                Value = DateTime.Now
            };

            var lblStartTime = new MaterialLabel
            {
                Text = "Start Time",
                Location = new Point(280, 320),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Caption,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtStartNotes = new MaterialMultiLineTextBox
            {
                Location = new Point(30, 390),
                Size = new Size(440, 80),
                Hint = "Start Notes",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                MaxLength = 500
            };

            btnStartJob = new MaterialButton
            {
                Location = new Point(310, 490),
                Size = new Size(80, 36),
                Text = "START",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnStartJob.Click += btnStartJob_Click;

            btnCancel = new MaterialButton
            {
                Location = new Point(400, 490),
                Size = new Size(80, 36),
                Text = "CANCEL",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnCancel.Click += btnCancel_Click;

            this.Controls.AddRange(new Control[] {
                lblTitle, lblJobDetails, txtJobNumber, txtCustomerName, txtPickupAddress, 
                txtDestinationAddress, cmbAssignedDriver, lblStartTime, dtpStartTime, 
                txtStartNotes, btnStartJob, btnCancel
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadJobDetails()
        {
            try
            {
                _job = _jobRepository.GetJobById(_jobId);
                if (_job is not null)
                {
                    txtJobNumber.Text = _job.JobNumber;
                    txtCustomerName.Text = _job.Customer?.FullName ?? _job.CustomerName;
                    txtPickupAddress.Text = $"{_job.PickupAddress}, {_job.PickupCity} {_job.PickupPostalCode}";
                    txtDestinationAddress.Text = $"{_job.DestinationAddress}, {_job.DestinationCity} {_job.DestinationPostalCode}";
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading job details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDrivers()
        {
            try
            {
                var availableDrivers = _driverRepository.GetDriversByStatus("available");
                cmbAssignedDriver.Items.Clear();
                
                foreach (var driver in availableDrivers)
                {
                    cmbAssignedDriver.Items.Add(new { 
                        Text = driver.FullName, 
                        Value = driver.DriverId 
                    });
                }
                
                cmbAssignedDriver.DisplayMember = "Text";
                cmbAssignedDriver.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading drivers: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStartJob_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var selectedDriver = (dynamic)cmbAssignedDriver.SelectedItem;
                int driverId = selectedDriver.Value;

                // Assign driver to job
                _jobRepository.AssignJobToDriver(_jobId, driverId);
                
                // Update job status to started
                _jobRepository.UpdateJobStatus(_jobId, "in_progress");
                
                // Update driver status to on_job
                _driverRepository.UpdateDriverStatus(driverId, "on_job");

                MaterialMessageBox.Show("Job started successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error starting job: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (cmbAssignedDriver.SelectedItem is null)
            {
                MaterialMessageBox.Show("Please select a driver to assign to this job.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbAssignedDriver.Focus();
                return false;
            }

            if (_job is null)
            {
                MaterialMessageBox.Show("Job details not loaded properly.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}