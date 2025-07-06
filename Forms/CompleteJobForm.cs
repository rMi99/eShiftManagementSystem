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
    public partial class CompleteJobForm : MaterialForm
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
        private DateTimePicker dtpCompletionTime;
        private MaterialComboBox cmbCompletionStatus;
        private MaterialMultiLineTextBox txtCompletionNotes;
        private MaterialCheckbox chkCustomerSignature;
        private MaterialButton btnCompleteJob;
        private MaterialButton btnCancel;

        public CompleteJobForm(int jobId)
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
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(500, 600);
            this.Text = "Complete Job";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblTitle = new MaterialLabel
            {
                Text = "Complete Job",
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

            dtpCompletionTime = new DateTimePicker
            {
                Location = new Point(30, 340),
                Size = new Size(190, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                Value = DateTime.Now
            };

            var lblCompletionTime = new MaterialLabel
            {
                Text = "Completion Time",
                Location = new Point(30, 320),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Caption,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            cmbCompletionStatus = new MaterialComboBox
            {
                Location = new Point(280, 330),
                Size = new Size(190, 50),
                Hint = "Completion Status",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbCompletionStatus.Items.AddRange(new object[] { 
                "completed", "delivered", "partial_delivery", "failed_delivery" 
            });

            txtCompletionNotes = new MaterialMultiLineTextBox
            {
                Location = new Point(30, 390),
                Size = new Size(440, 100),
                Hint = "Completion Notes (Optional)",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                MaxLength = 1000
            };

            chkCustomerSignature = new MaterialCheckbox
            {
                Location = new Point(30, 500),
                Size = new Size(200, 24),
                Text = "Customer Signature Obtained",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            btnCompleteJob = new MaterialButton
            {
                Location = new Point(310, 540),
                Size = new Size(80, 36),
                Text = "COMPLETE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnCompleteJob.Click += btnCompleteJob_Click;

            btnCancel = new MaterialButton
            {
                Location = new Point(400, 540),
                Size = new Size(80, 36),
                Text = "CANCEL",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnCancel.Click += btnCancel_Click;

            this.Controls.AddRange(new Control[] {
                lblTitle, lblJobDetails, txtJobNumber, txtCustomerName, txtPickupAddress, 
                txtDestinationAddress, lblCompletionTime, dtpCompletionTime, cmbCompletionStatus,
                txtCompletionNotes, chkCustomerSignature, btnCompleteJob, btnCancel
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
                    
                    // Set default completion status based on current status
                    if (string.Equals(_job.Status, "in_transit", StringComparison.OrdinalIgnoreCase))
                    {
                        cmbCompletionStatus.SelectedItem = "delivered";
                    }
                    else
                    {
                        cmbCompletionStatus.SelectedItem = "completed";
                    }
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading job details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCompleteJob_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                string completionStatus = cmbCompletionStatus.SelectedItem?.ToString() ?? "completed";
                
                // Update job status to completed
                _jobRepository.UpdateJobStatus(_jobId, completionStatus);

                // If driver is assigned, update their status back to available
                if (_job?.DriverId.HasValue == true)
                {
                    _driverRepository.UpdateDriverStatus(_job.DriverId.Value, "available");
                }

                MaterialMessageBox.Show("Job completed successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error completing job: {ex.Message}", "Error", 
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
            if (cmbCompletionStatus.SelectedItem is null)
            {
                MaterialMessageBox.Show("Please select a completion status.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCompletionStatus.Focus();
                return false;
            }

            string status = cmbCompletionStatus.SelectedItem.ToString() ?? string.Empty;
            if (string.Equals(status, "delivered", StringComparison.OrdinalIgnoreCase) && !chkCustomerSignature.Checked)
            {
                var result = MaterialMessageBox.Show("Customer signature not confirmed. Continue anyway?", "Confirmation", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return result == DialogResult.Yes;
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