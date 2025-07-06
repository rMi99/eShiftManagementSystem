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
    public partial class UpdateLocationForm : MaterialForm
    {
        private readonly JobRepository _jobRepository;
        private readonly int _jobId;
        private readonly MaterialSkinManager materialSkinManager;

        // Controls
        private MaterialLabel lblTitle;
        private MaterialTextBox txtCurrentLocation;
        private MaterialTextBox txtProgress;
        private MaterialComboBox cmbStatus;
        private MaterialMultiLineTextBox txtNotes;
        private MaterialButton btnUpdateLocation;
        private MaterialButton btnCancel;

        public UpdateLocationForm(int jobId)
        {
            _jobId = jobId;
            _jobRepository = new JobRepository();
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
            this.ClientSize = new Size(450, 400);
            this.Text = "Update Location";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblTitle = new MaterialLabel
            {
                Text = "Update Driver/Job Location",
                Location = new Point(30, 80),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H5,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtCurrentLocation = new MaterialTextBox
            {
                Location = new Point(30, 130),
                Size = new Size(390, 50),
                Hint = "Current Location",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtProgress = new MaterialTextBox
            {
                Location = new Point(30, 190),
                Size = new Size(390, 50),
                Hint = "Progress Update",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbStatus = new MaterialComboBox
            {
                Location = new Point(30, 250),
                Size = new Size(390, 50),
                Hint = "Job Status",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbStatus.Items.AddRange(new object[] { "in_progress", "picked_up", "in_transit", "delivered", "delayed" });

            txtNotes = new MaterialMultiLineTextBox
            {
                Location = new Point(30, 310),
                Size = new Size(390, 60),
                Hint = "Additional Notes",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                MaxLength = 500
            };

            btnUpdateLocation = new MaterialButton
            {
                Location = new Point(250, 380),
                Size = new Size(80, 36),
                Text = "UPDATE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnUpdateLocation.Click += btnUpdateLocation_Click;

            btnCancel = new MaterialButton
            {
                Location = new Point(340, 380),
                Size = new Size(80, 36),
                Text = "CANCEL",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnCancel.Click += btnCancel_Click;

            this.Controls.AddRange(new Control[] {
                lblTitle, txtCurrentLocation, txtProgress, cmbStatus, txtNotes,
                btnUpdateLocation, btnCancel
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadJobDetails()
        {
            try
            {
                var job = _jobRepository.GetJobById(_jobId);
                if (job is not null)
                {
                    this.Text = $"Update Location - Job #{job.JobNumber}";
                    cmbStatus.SelectedItem = job.Status;
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading job details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateLocation_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                // Update job progress
                _jobRepository.UpdateJobProgress(_jobId, cmbStatus.SelectedItem?.ToString() ?? "in_progress");

                MaterialMessageBox.Show("Location updated successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error updating location: {ex.Message}", "Error", 
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
            if (string.IsNullOrWhiteSpace(txtCurrentLocation.Text))
            {
                MaterialMessageBox.Show("Please enter current location.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCurrentLocation.Focus();
                return false;
            }

            if (cmbStatus.SelectedItem is null)
            {
                MaterialMessageBox.Show("Please select a status.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
                return false;
            }

            return true;
        }
    }
}