using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace eShiftManagementSystem.Forms
{
    public partial class CreateJobForm : MaterialForm
    {
        private readonly JobRepository _jobRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly MaterialSkinManager materialSkinManager;

        // Controls
        private MaterialCard cardJobDetails;
        private MaterialComboBox cmbCustomer;
        private MaterialTextBox txtPickupAddress;
        private MaterialTextBox txtPickupCity;
        private MaterialTextBox txtPickupPostalCode;
        private MaterialTextBox txtDestinationAddress;
        private MaterialTextBox txtDestinationCity;
        private MaterialTextBox txtDestinationPostalCode;
        private DateTimePicker dtpPickupDate;
        private DateTimePicker dtpDeliveryDate;
        private MaterialTextBox txtSpecialInstructions;
        private MaterialTextBox txtEstimatedWeight;
        private MaterialTextBox txtEstimatedVolume;
        private MaterialTextBox txtJobDescription;
        private MaterialComboBox cmbPriority;
        private MaterialButton btnSave;
        private MaterialButton btnCancel;

        public CreateJobForm()
        {
            _jobRepository = new JobRepository();
            _customerRepository = new CustomerRepository();

            // Initialize Material Design
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue100, Accent.Orange200, TextShade.WHITE);

            InitializeComponent();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Create New Job";
            this.Size = new Size(700, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title
            var lblTitle = new MaterialLabel
            {
                Text = "Create New Job",
                Location = new Point(30, 80),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            // Main card container
            cardJobDetails = new MaterialCard
            {
                Location = new Point(20, 120),
                Size = new Size(660, 600),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            // Customer Selection
            var lblCustomer = new MaterialLabel
            {
                Text = "Customer",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Subtitle1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            cmbCustomer = new MaterialComboBox
            {
                Location = new Point(20, 50),
                Size = new Size(620, 50),
                Hint = "Select Customer",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            // Job Description
            var lblJobDescription = new MaterialLabel
            {
                Text = "Job Description",
                Location = new Point(20, 110),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Subtitle1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtJobDescription = new MaterialTextBox
            {
                Location = new Point(20, 140),
                Size = new Size(620, 60),
                Hint = "Brief description of the moving job",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Multiline = true
            };

            // Pickup Information
            var lblPickup = new MaterialLabel
            {
                Text = "Pickup Information",
                Location = new Point(20, 210),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Subtitle1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtPickupAddress = new MaterialTextBox
            {
                Location = new Point(20, 240),
                Size = new Size(400, 50),
                Hint = "Pickup Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPickupCity = new MaterialTextBox
            {
                Location = new Point(20, 300),
                Size = new Size(190, 50),
                Hint = "Pickup City",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPickupPostalCode = new MaterialTextBox
            {
                Location = new Point(230, 300),
                Size = new Size(190, 50),
                Hint = "Pickup Postal Code",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            // Pickup Date
            var lblPickupDate = new Label
            {
                Text = "Pickup Date:",
                Location = new Point(440, 280),
                AutoSize = true,
                Font = new Font("Roboto", 10),
                ForeColor = Color.Gray
            };

            dtpPickupDate = new DateTimePicker
            {
                Location = new Point(440, 305),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddDays(1)
            };

            // Destination Information
            var lblDestination = new MaterialLabel
            {
                Text = "Destination Information",
                Location = new Point(20, 360),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Subtitle1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtDestinationAddress = new MaterialTextBox
            {
                Location = new Point(20, 390),
                Size = new Size(400, 50),
                Hint = "Destination Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtDestinationCity = new MaterialTextBox
            {
                Location = new Point(20, 450),
                Size = new Size(190, 50),
                Hint = "Destination City",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtDestinationPostalCode = new MaterialTextBox
            {
                Location = new Point(230, 450),
                Size = new Size(190, 50),
                Hint = "Destination Postal Code",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            // Delivery Date
            var lblDeliveryDate = new Label
            {
                Text = "Delivery Date:",
                Location = new Point(440, 430),
                AutoSize = true,
                Font = new Font("Roboto", 10),
                ForeColor = Color.Gray
            };

            dtpDeliveryDate = new DateTimePicker
            {
                Location = new Point(440, 455),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddDays(2)
            };

            // Job Details
            var lblJobDetails = new MaterialLabel
            {
                Text = "Job Details",
                Location = new Point(20, 510),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Subtitle1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtEstimatedWeight = new MaterialTextBox
            {
                Location = new Point(20, 540),
                Size = new Size(150, 50),
                Hint = "Weight (kg)",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtEstimatedVolume = new MaterialTextBox
            {
                Location = new Point(190, 540),
                Size = new Size(150, 50),
                Hint = "Volume (m³)",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbPriority = new MaterialComboBox
            {
                Location = new Point(360, 540),
                Size = new Size(150, 50),
                Hint = "Priority",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbPriority.Items.AddRange(new object[] { "Normal", "High", "Urgent" });
            cmbPriority.SelectedIndex = 0;

            // Special Instructions
            txtSpecialInstructions = new MaterialTextBox
            {
                Location = new Point(20, 600),
                Size = new Size(620, 60),
                Hint = "Special instructions or requirements",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Multiline = true
            };

            // Add all controls to the card
            cardJobDetails.Controls.Add(lblCustomer);
            cardJobDetails.Controls.Add(cmbCustomer);
            cardJobDetails.Controls.Add(lblJobDescription);
            cardJobDetails.Controls.Add(txtJobDescription);
            cardJobDetails.Controls.Add(lblPickup);
            cardJobDetails.Controls.Add(txtPickupAddress);
            cardJobDetails.Controls.Add(txtPickupCity);
            cardJobDetails.Controls.Add(txtPickupPostalCode);
            cardJobDetails.Controls.Add(lblPickupDate);
            cardJobDetails.Controls.Add(dtpPickupDate);
            cardJobDetails.Controls.Add(lblDestination);
            cardJobDetails.Controls.Add(txtDestinationAddress);
            cardJobDetails.Controls.Add(txtDestinationCity);
            cardJobDetails.Controls.Add(txtDestinationPostalCode);
            cardJobDetails.Controls.Add(lblDeliveryDate);
            cardJobDetails.Controls.Add(dtpDeliveryDate);
            cardJobDetails.Controls.Add(lblJobDetails);
            cardJobDetails.Controls.Add(txtEstimatedWeight);
            cardJobDetails.Controls.Add(txtEstimatedVolume);
            cardJobDetails.Controls.Add(cmbPriority);
            cardJobDetails.Controls.Add(txtSpecialInstructions);

            // Action buttons
            btnSave = new MaterialButton
            {
                Location = new Point(510, 740),
                Size = new Size(80, 36),
                Text = "CREATE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSave.Click += btnSave_Click;

            btnCancel = new MaterialButton
            {
                Location = new Point(600, 740),
                Size = new Size(80, 36),
                Text = "CANCEL",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnCancel.Click += btnCancel_Click;

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(cardJobDetails);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            this.ResumeLayout(false);
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = _customerRepository.GetAllCustomers();
                cmbCustomer.Items.Clear();

                foreach (var customer in customers)
                {
                    cmbCustomer.Items.Add(new { Text = customer.FullName, Value = customer.CustomerId });
                }

                cmbCustomer.DisplayMember = "Text";
                cmbCustomer.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading customers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                var selectedCustomer = (dynamic)cmbCustomer.SelectedItem;

                var job = new Job
                {
                    CustomerId = selectedCustomer.Value,
                    JobNumber = GenerateJobNumber(),
                    PickupAddress = txtPickupAddress.Text.Trim(),
                    PickupCity = txtPickupCity.Text.Trim(),
                    PickupPostalCode = txtPickupPostalCode.Text.Trim(),
                    DestinationAddress = txtDestinationAddress.Text.Trim(),
                    DestinationCity = txtDestinationCity.Text.Trim(),
                    DestinationPostalCode = txtDestinationPostalCode.Text.Trim(),
                    RequestedPickupDate = dtpPickupDate.Value.Date,
                    RequestedDeliveryDate = dtpDeliveryDate.Value.Date,
                    SpecialInstructions = txtSpecialInstructions.Text.Trim(),
                    Status = "pending",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Set optional fields
                if (!string.IsNullOrWhiteSpace(txtEstimatedWeight.Text) &&
                    decimal.TryParse(txtEstimatedWeight.Text, out decimal weight))
                {
                    job.TotalEstimatedWeight = weight;
                }

                if (!string.IsNullOrWhiteSpace(txtEstimatedVolume.Text) &&
                    decimal.TryParse(txtEstimatedVolume.Text, out decimal volume))
                {
                    job.TotalEstimatedVolume = volume;
                }

                // Add job description to special instructions if provided
                if (!string.IsNullOrWhiteSpace(txtJobDescription.Text))
                {
                    job.SpecialInstructions = $"Description: {txtJobDescription.Text.Trim()}\n\n{job.SpecialInstructions}";
                }

                var jobId = _jobRepository.AddJob(job);

                MaterialMessageBox.Show($"Job created successfully!\nJob Number: {job.JobNumber}\nJob ID: {jobId}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error creating job: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MaterialMessageBox.Show("Are you sure you want to cancel? All unsaved changes will be lost.",
                "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private bool ValidateForm()
        {
            // Customer validation
            if (cmbCustomer.SelectedIndex == -1)
            {
                MaterialMessageBox.Show("Please select a customer.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCustomer.Focus();
                return false;
            }

            // Pickup address validation
            if (string.IsNullOrWhiteSpace(txtPickupAddress.Text))
            {
                MaterialMessageBox.Show("Please enter a pickup address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPickupAddress.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPickupCity.Text))
            {
                MaterialMessageBox.Show("Please enter a pickup city.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPickupCity.Focus();
                return false;
            }

            // Destination address validation
            if (string.IsNullOrWhiteSpace(txtDestinationAddress.Text))
            {
                MaterialMessageBox.Show("Please enter a destination address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDestinationAddress.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDestinationCity.Text))
            {
                MaterialMessageBox.Show("Please enter a destination city.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDestinationCity.Focus();
                return false;
            }

            // Date validation
            if (dtpPickupDate.Value.Date <= DateTime.Now.Date)
            {
                MaterialMessageBox.Show("Pickup date must be in the future.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpPickupDate.Focus();
                return false;
            }

            if (dtpDeliveryDate.Value.Date < dtpPickupDate.Value.Date)
            {
                MaterialMessageBox.Show("Delivery date must be on or after pickup date.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDeliveryDate.Focus();
                return false;
            }

            // Numeric validation for weight and volume (if provided)
            if (!string.IsNullOrWhiteSpace(txtEstimatedWeight.Text))
            {
                if (!decimal.TryParse(txtEstimatedWeight.Text, out decimal weight) || weight <= 0)
                {
                    MaterialMessageBox.Show("Please enter a valid weight (greater than 0).", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEstimatedWeight.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtEstimatedVolume.Text))
            {
                if (!decimal.TryParse(txtEstimatedVolume.Text, out decimal volume) || volume <= 0)
                {
                    MaterialMessageBox.Show("Please enter a valid volume (greater than 0).", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEstimatedVolume.Focus();
                    return false;
                }
            }

            return true;
        }

        private string GenerateJobNumber()
        {
            // Generate job number format: JOB-YYYYMMDD-HHMMSS
            return $"JOB-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
            {
                var result = MaterialMessageBox.Show("Are you sure you want to close? All unsaved changes will be lost.",
                    "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            base.OnFormClosing(e);
        }
    }
}