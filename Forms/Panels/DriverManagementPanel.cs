using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace eShiftManagementSystem.Forms.Panels
{
    public partial class DriverManagementPanel : UserControl
    {
        private readonly DriverRepository _driverRepository;
        private readonly AuditService _auditService;
        private Driver? _selectedDriver;

        // Controls
        private MaterialCard cardDriverList;
        private MaterialCard cardDriverDetails;
        private DataGridView dgvDrivers;
        private MaterialTextBox txtSearch;
        private MaterialComboBox cmbAvailabilityFilter;
        private MaterialButton btnSearch;
        private MaterialButton btnRefresh;
        private MaterialButton btnAddNew;
        private MaterialTextBox txtLicenseNumber;
        private MaterialComboBox cmbLicenseType;
        private DateTimePicker dtpLicenseExpiry;
        private MaterialTextBox txtExperienceYears;
        private MaterialTextBox txtVehiclePreference;
        private MaterialCheckbox chkIsAvailable;
        private MaterialTextBox txtRating;
        private MaterialButton btnSave;
        private MaterialButton btnEdit;
        private MaterialButton btnDelete;
        private MaterialButton btnClear;

        public DriverManagementPanel()
        {
            _driverRepository = new DriverRepository();
            _auditService = new AuditService();
            InitializeComponent();
            LoadDrivers();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1200, 800);

            var lblTitle = new MaterialLabel
            {
                Text = "Driver Management",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            CreateDriverListCard();
            CreateDriverDetailsCard();
        }

        private void CreateDriverListCard()
        {
            cardDriverList = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(750, 500),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblDriverList = new MaterialLabel
            {
                Text = "Driver List",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtSearch = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(300, 50),
                Hint = "Search drivers by name or license...",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbAvailabilityFilter = new MaterialComboBox
            {
                Location = new Point(340, 60),
                Size = new Size(150, 50),
                Hint = "Availability",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbAvailabilityFilter.Items.AddRange(new object[] { "All", "Available", "Busy" });
            cmbAvailabilityFilter.SelectedIndex = 0;

            btnSearch = new MaterialButton
            {
                Location = new Point(510, 60),
                Size = new Size(100, 50),
                Text = "SEARCH",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnSearch.Click += btnSearch_Click;

            btnRefresh = new MaterialButton
            {
                Location = new Point(630, 60),
                Size = new Size(100, 50),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnRefresh.Click += btnRefresh_Click;

            dgvDrivers = new DataGridView
            {
                Location = new Point(20, 130),
                Size = new Size(690, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                BackgroundColor = Color.White,
                GridColor = Color.FromArgb(230, 230, 230),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(222, 0, 0, 0),
                    SelectionBackColor = Color.FromArgb(63, 81, 181),
                    SelectionForeColor = Color.White,
                    WrapMode = DataGridViewTriState.False
                }
            };
            dgvDrivers.SelectionChanged += dgvDrivers_SelectionChanged;

            btnAddNew = new MaterialButton
            {
                Location = new Point(20, 450),
                Size = new Size(120, 40),
                Text = "ADD NEW",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = true,
                UseVisualStyleBackColor = true
            };
            btnAddNew.Click += btnAddNew_Click;

            cardDriverList.Controls.AddRange(new Control[] {
                lblDriverList, txtSearch, cmbAvailabilityFilter, btnSearch, btnRefresh, dgvDrivers, btnAddNew
            });

            this.Controls.Add(cardDriverList);
        }

        private void CreateDriverDetailsCard()
        {
            cardDriverDetails = new MaterialCard
            {
                Location = new Point(800, 70),
                Size = new Size(380, 500),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblDriverDetails = new MaterialLabel
            {
                Text = "Driver Details",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtLicenseNumber = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(320, 50),
                Hint = "License Number",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbLicenseType = new MaterialComboBox
            {
                Location = new Point(20, 120),
                Size = new Size(160, 50),
                Hint = "License Type",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbLicenseType.Items.AddRange(new object[] { "B", "C", "C1", "C+E", "D", "D1", "D+E" });

            txtExperienceYears = new MaterialTextBox
            {
                Location = new Point(190, 120),
                Size = new Size(150, 50),
                Hint = "Experience (Years)",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            var lblLicenseExpiry = new MaterialLabel
            {
                Text = "License Expiry Date",
                Location = new Point(20, 180),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Body1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            dtpLicenseExpiry = new DateTimePicker
            {
                Location = new Point(20, 210),
                Size = new Size(200, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddYears(5) // Default to 5 years from now
            };

            txtVehiclePreference = new MaterialTextBox
            {
                Location = new Point(20, 250),
                Size = new Size(320, 50),
                Hint = "Vehicle Type Preference",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtRating = new MaterialTextBox
            {
                Location = new Point(20, 310),
                Size = new Size(150, 50),
                Hint = "Rating (0-5)",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            chkIsAvailable = new MaterialCheckbox
            {
                Location = new Point(190, 310),
                Size = new Size(150, 40),
                Text = "Available",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.HOVER,
                Checked = true
            };

            // Buttons
            btnSave = new MaterialButton
            {
                Location = new Point(20, 370),
                Size = new Size(80, 40),
                Text = "SAVE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = true,
                UseVisualStyleBackColor = true
            };
            btnSave.Click += btnSave_Click;

            btnEdit = new MaterialButton
            {
                Location = new Point(110, 370),
                Size = new Size(80, 40),
                Text = "EDIT",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnEdit.Click += btnEdit_Click;

            btnDelete = new MaterialButton
            {
                Location = new Point(200, 370),
                Size = new Size(80, 40),
                Text = "DELETE",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnDelete.Click += btnDelete_Click;

            btnClear = new MaterialButton
            {
                Location = new Point(290, 370),
                Size = new Size(50, 40),
                Text = "CLEAR",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnClear.Click += btnClear_Click;

            cardDriverDetails.Controls.AddRange(new Control[] {
                lblDriverDetails, txtLicenseNumber, cmbLicenseType, txtExperienceYears,
                lblLicenseExpiry, dtpLicenseExpiry, txtVehiclePreference, txtRating,
                chkIsAvailable, btnSave, btnEdit, btnDelete, btnClear
            });

            this.Controls.Add(cardDriverDetails);
        }

        private void LoadDrivers()
        {
            try
            {
                var drivers = _driverRepository.GetAllDrivers();
                var driverData = drivers.Select(d => new
                {
                    DriverId = d.DriverId,
                    FullName = d.FullName,
                    LicenseNumber = d.LicenseNumber,
                    LicenseType = d.LicenseType,
                    Experience = $"{d.ExperienceYears} years",
                    Rating = d.FormattedRating,
                    Status = d.AvailabilityStatus,
                    LicenseValid = d.IsLicenseValid ? "Valid" : "Expired"
                }).ToList();

                dgvDrivers.DataSource = driverData;

                if (dgvDrivers.Columns.Count > 0)
                {
                    dgvDrivers.Columns["DriverId"].Visible = false;
                    dgvDrivers.Columns["FullName"].HeaderText = "Full Name";
                    dgvDrivers.Columns["LicenseNumber"].HeaderText = "License Number";
                    dgvDrivers.Columns["LicenseType"].HeaderText = "License Type";
                    dgvDrivers.Columns["Experience"].HeaderText = "Experience";
                    dgvDrivers.Columns["Rating"].HeaderText = "Rating";
                    dgvDrivers.Columns["Status"].HeaderText = "Status";
                    dgvDrivers.Columns["LicenseValid"].HeaderText = "License";
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading drivers: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var allDrivers = _driverRepository.GetAllDrivers();
                var filteredDrivers = allDrivers.Where(d =>
                {
                    bool matchesSearch = string.IsNullOrWhiteSpace(txtSearch.Text) ||
                        d.FullName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                        d.LicenseNumber.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase);

                    bool matchesAvailability = cmbAvailabilityFilter.SelectedIndex == 0 || // "All"
                        (cmbAvailabilityFilter.SelectedIndex == 1 && d.IsAvailable) || // "Available"
                        (cmbAvailabilityFilter.SelectedIndex == 2 && !d.IsAvailable); // "Busy"

                    return matchesSearch && matchesAvailability;
                }).ToList();

                var driverData = filteredDrivers.Select(d => new
                {
                    DriverId = d.DriverId,
                    FullName = d.FullName,
                    LicenseNumber = d.LicenseNumber,
                    LicenseType = d.LicenseType,
                    Experience = $"{d.ExperienceYears} years",
                    Rating = d.FormattedRating,
                    Status = d.AvailabilityStatus,
                    LicenseValid = d.IsLicenseValid ? "Valid" : "Expired"
                }).ToList();

                dgvDrivers.DataSource = driverData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error searching drivers: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDrivers();
            ClearForm();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtLicenseNumber.Focus();
        }

        private void dgvDrivers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDrivers.SelectedRows.Count > 0)
            {
                var selectedRow = dgvDrivers.SelectedRows[0];
                var driverId = Convert.ToInt32(selectedRow.Cells["DriverId"].Value);
                
                var driver = _driverRepository.GetDriverById(driverId);
                if (driver != null)
                {
                    PopulateForm(driver);
                }
            }
        }

        private void PopulateForm(Driver driver)
        {
            _selectedDriver = driver;
            txtLicenseNumber.Text = driver.LicenseNumber;
            cmbLicenseType.Text = driver.LicenseType;
            dtpLicenseExpiry.Value = driver.LicenseExpiryDate;
            txtExperienceYears.Text = driver.ExperienceYears.ToString();
            txtVehiclePreference.Text = driver.VehicleTypePreference ?? string.Empty;
            chkIsAvailable.Checked = driver.IsAvailable;
            txtRating.Text = driver.Rating.ToString("F1");
        }

        private void ClearForm()
        {
            _selectedDriver = null;
            txtLicenseNumber.Clear();
            cmbLicenseType.SelectedIndex = -1;
            dtpLicenseExpiry.Value = DateTime.Now.AddYears(5);
            txtExperienceYears.Clear();
            txtVehiclePreference.Clear();
            chkIsAvailable.Checked = true;
            txtRating.Clear();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtLicenseNumber.Text))
            {
                MaterialMessageBox.Show("License number is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbLicenseType.SelectedIndex == -1)
            {
                MaterialMessageBox.Show("License type is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtExperienceYears.Text, out int experience) || experience < 0)
            {
                MaterialMessageBox.Show("Please enter a valid experience years (0 or greater).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtRating.Text))
            {
                if (!decimal.TryParse(txtRating.Text, out decimal rating) || rating < 0 || rating > 5)
                {
                    MaterialMessageBox.Show("Rating must be between 0 and 5.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if (dtpLicenseExpiry.Value <= DateTime.Now)
            {
                MaterialMessageBox.Show("License expiry date must be in the future.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                if (_selectedDriver == null)
                {
                    // Check if license number already exists
                    if (_driverRepository.LicenseNumberExists(txtLicenseNumber.Text.Trim()))
                    {
                        MaterialMessageBox.Show("A driver with this license number already exists.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Note: In a real implementation, you would need to create or select a Staff record first
                    // For this example, we'll assume staff creation is handled separately
                    MaterialMessageBox.Show("Driver creation requires staff record creation first. Please use Staff Management to create staff records.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    // Update existing driver
                    var oldDriver = new Driver
                    {
                        DriverId = _selectedDriver.DriverId,
                        StaffId = _selectedDriver.StaffId,
                        LicenseNumber = _selectedDriver.LicenseNumber,
                        LicenseType = _selectedDriver.LicenseType,
                        LicenseExpiryDate = _selectedDriver.LicenseExpiryDate,
                        ExperienceYears = _selectedDriver.ExperienceYears,
                        VehicleTypePreference = _selectedDriver.VehicleTypePreference,
                        IsAvailable = _selectedDriver.IsAvailable,
                        CurrentVehicleId = _selectedDriver.CurrentVehicleId,
                        Rating = _selectedDriver.Rating
                    };

                    _selectedDriver.LicenseNumber = txtLicenseNumber.Text.Trim();
                    _selectedDriver.LicenseType = cmbLicenseType.SelectedItem?.ToString() ?? string.Empty;
                    _selectedDriver.LicenseExpiryDate = dtpLicenseExpiry.Value;
                    _selectedDriver.ExperienceYears = int.Parse(txtExperienceYears.Text);
                    _selectedDriver.VehicleTypePreference = string.IsNullOrWhiteSpace(txtVehiclePreference.Text) ? 
                        null : txtVehiclePreference.Text.Trim();
                    _selectedDriver.IsAvailable = chkIsAvailable.Checked;
                    _selectedDriver.Rating = string.IsNullOrWhiteSpace(txtRating.Text) ? 
                        0 : decimal.Parse(txtRating.Text);

                    _driverRepository.UpdateDriver(_selectedDriver);
                    _auditService.LogUpdate("drivers", _selectedDriver.DriverId, oldDriver, _selectedDriver, SessionManager.CurrentUserId);
                    
                    MaterialMessageBox.Show("Driver updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadDrivers();
                ClearForm();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving driver: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedDriver == null)
            {
                MaterialMessageBox.Show("Please select a driver to edit.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Form is already populated for editing
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedDriver == null)
            {
                MaterialMessageBox.Show("Please select a driver to delete.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MaterialMessageBox.Show(
                $"Are you sure you want to delete driver '{_selectedDriver.FullName}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _auditService.LogDelete("drivers", _selectedDriver.DriverId, _selectedDriver, SessionManager.CurrentUserId);
                    _driverRepository.DeleteDriver(_selectedDriver.DriverId);
                    
                    MaterialMessageBox.Show("Driver deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDrivers();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error deleting driver: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}