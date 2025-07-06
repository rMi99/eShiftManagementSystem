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
    public partial class VehicleManagementPanel : UserControl
    {
        private readonly VehicleRepository _vehicleRepository;
        private Vehicle? _selectedVehicle;

        // Controls
        private MaterialCard cardVehicleList;
        private MaterialCard cardVehicleDetails;
        private DataGridView dgvVehicles;
        private MaterialTextBox txtSearch;
        private MaterialButton btnSearch;
        private MaterialButton btnRefresh;
        private MaterialButton btnAddNew;
        private MaterialTextBox txtRegistrationNumber;
        private MaterialTextBox txtMake;
        private MaterialTextBox txtModel;
        private MaterialTextBox txtYear;
        private MaterialTextBox txtMileage;
        private DateTimePicker dtpLastService;
        private MaterialCheckbox chkAvailable;
        private MaterialButton btnSave;
        private MaterialButton btnEdit;
        private MaterialButton btnDelete;
        private MaterialButton btnClear;

        public VehicleManagementPanel()
        {
            _vehicleRepository = new VehicleRepository();
            InitializeComponent();
            LoadVehicles();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1000, 700);

            // Title
            var lblTitle = new MaterialLabel
            {
                Text = "Vehicle Management",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            CreateVehicleListCard();
            CreateVehicleDetailsCard();
        }

        private void CreateVehicleListCard()
        {
            cardVehicleList = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(580, 600),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            // Search section
            txtSearch = new MaterialTextBox
            {
                Location = new Point(20, 20),
                Size = new Size(300, 50),
                Hint = "Search vehicles...",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            btnSearch = new MaterialButton
            {
                Location = new Point(330, 30),
                Size = new Size(80, 30),
                Text = "SEARCH",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSearch.Click += btnSearch_Click;

            btnRefresh = new MaterialButton
            {
                Location = new Point(420, 30),
                Size = new Size(80, 30),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnRefresh.Click += btnRefresh_Click;

            btnAddNew = new MaterialButton
            {
                Location = new Point(20, 80),
                Size = new Size(100, 30),
                Text = "ADD NEW",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnAddNew.Click += btnAddNew_Click;

            // Vehicles grid
            dgvVehicles = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(540, 450),
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
            dgvVehicles.SelectionChanged += dgvVehicles_SelectionChanged;

            cardVehicleList.Controls.Add(txtSearch);
            cardVehicleList.Controls.Add(btnSearch);
            cardVehicleList.Controls.Add(btnRefresh);
            cardVehicleList.Controls.Add(btnAddNew);
            cardVehicleList.Controls.Add(dgvVehicles);
            this.Controls.Add(cardVehicleList);
        }

        private void CreateVehicleDetailsCard()
        {
            cardVehicleDetails = new MaterialCard
            {
                Location = new Point(620, 70),
                Size = new Size(350, 600),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblDetails = new MaterialLabel
            {
                Text = "Vehicle Details",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            // Form fields
            txtRegistrationNumber = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(310, 50),
                Hint = "Registration Number",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtMake = new MaterialTextBox
            {
                Location = new Point(20, 120),
                Size = new Size(150, 50),
                Hint = "Make",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtModel = new MaterialTextBox
            {
                Location = new Point(180, 120),
                Size = new Size(150, 50),
                Hint = "Model",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtYear = new MaterialTextBox
            {
                Location = new Point(20, 180),
                Size = new Size(100, 50),
                Hint = "Year",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtMileage = new MaterialTextBox
            {
                Location = new Point(130, 180),
                Size = new Size(100, 50),
                Hint = "Mileage",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            chkAvailable = new MaterialCheckbox
            {
                Location = new Point(20, 240),
                Size = new Size(150, 40),
                Text = "Available",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.HOVER,
                Checked = true
            };

            // Action buttons
            btnSave = new MaterialButton
            {
                Location = new Point(20, 300),
                Size = new Size(70, 36),
                Text = "SAVE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSave.Click += btnSave_Click;

            btnEdit = new MaterialButton
            {
                Location = new Point(100, 300),
                Size = new Size(70, 36),
                Text = "EDIT",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnEdit.Click += btnEdit_Click;

            btnDelete = new MaterialButton
            {
                Location = new Point(180, 300),
                Size = new Size(70, 36),
                Text = "DELETE",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = true,
                HighEmphasis = false
            };
            btnDelete.Click += btnDelete_Click;

            btnClear = new MaterialButton
            {
                Location = new Point(260, 300),
                Size = new Size(70, 36),
                Text = "CLEAR",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnClear.Click += btnClear_Click;

            // Add all controls to details card
            cardVehicleDetails.Controls.Add(lblDetails);
            cardVehicleDetails.Controls.Add(txtRegistrationNumber);
            cardVehicleDetails.Controls.Add(txtMake);
            cardVehicleDetails.Controls.Add(txtModel);
            cardVehicleDetails.Controls.Add(txtYear);
            cardVehicleDetails.Controls.Add(txtMileage);
            cardVehicleDetails.Controls.Add(chkAvailable);
            cardVehicleDetails.Controls.Add(btnSave);
            cardVehicleDetails.Controls.Add(btnEdit);
            cardVehicleDetails.Controls.Add(btnDelete);
            cardVehicleDetails.Controls.Add(btnClear);

            this.Controls.Add(cardVehicleDetails);
        }

        private void LoadVehicles()
        {
            try
            {
                var vehicles = _vehicleRepository.GetAllVehicles();
                var vehicleData = vehicles.Select(v => new
                {
                    VehicleId = v.VehicleId,
                    RegistrationNumber = v.RegistrationNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    Mileage = v.Mileage?.ToString() ?? "N/A",
                    Status = v.IsAvailable ? "Available" : "Unavailable"
                }).ToList();

                dgvVehicles.DataSource = vehicleData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading vehicles: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadVehicles();
                    return;
                }

                var vehicles = _vehicleRepository.SearchVehicles(txtSearch.Text.Trim());
                var vehicleData = vehicles.Select(v => new
                {
                    VehicleId = v.VehicleId,
                    RegistrationNumber = v.RegistrationNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    Mileage = v.Mileage?.ToString() ?? "N/A",
                    Status = v.IsAvailable ? "Available" : "Unavailable"
                }).ToList();

                dgvVehicles.DataSource = vehicleData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error searching vehicles: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadVehicles();
            ClearForm();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtRegistrationNumber.Focus();
        }

        private void dgvVehicles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvVehicles.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedRow = dgvVehicles.SelectedRows[0];
                    var vehicleId = Convert.ToInt32(selectedRow.Cells["VehicleId"].Value);
                    
                    _selectedVehicle = _vehicleRepository.GetVehicleById(vehicleId);
                    if (_selectedVehicle != null)
                    {
                        PopulateForm(_selectedVehicle);
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error loading vehicle details: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PopulateForm(Vehicle vehicle)
        {
            txtRegistrationNumber.Text = vehicle.RegistrationNumber;
            txtMake.Text = vehicle.Make;
            txtModel.Text = vehicle.Model;
            txtYear.Text = vehicle.Year.ToString();
            txtMileage.Text = vehicle.Mileage?.ToString() ?? "";
            chkAvailable.Checked = vehicle.IsAvailable;
        }

        private void ClearForm()
        {
            txtRegistrationNumber.Text = "";
            txtMake.Text = "";
            txtModel.Text = "";
            txtYear.Text = "";
            txtMileage.Text = "";
            chkAvailable.Checked = true;
            _selectedVehicle = null;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtRegistrationNumber.Text) ||
                string.IsNullOrWhiteSpace(txtMake.Text) ||
                string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text))
            {
                MaterialMessageBox.Show("Please fill in all required fields.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year + 1)
            {
                MaterialMessageBox.Show("Please enter a valid year.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtMileage.Text))
            {
                if (!int.TryParse(txtMileage.Text, out _))
                {
                    MaterialMessageBox.Show("Please enter a valid mileage value.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                if (_selectedVehicle == null)
                {
                    // Create new vehicle
                    var vehicle = new Vehicle
                    {
                        RegistrationNumber = txtRegistrationNumber.Text.Trim(),
                        TypeId = 1, // Default type
                        Make = txtMake.Text.Trim(),
                        Model = txtModel.Text.Trim(),
                        Year = int.Parse(txtYear.Text),
                        Mileage = string.IsNullOrWhiteSpace(txtMileage.Text) ? null : int.Parse(txtMileage.Text),
                        IsAvailable = chkAvailable.Checked
                    };

                    _vehicleRepository.AddVehicle(vehicle);
                    MaterialMessageBox.Show("Vehicle created successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update existing vehicle
                    _selectedVehicle.RegistrationNumber = txtRegistrationNumber.Text.Trim();
                    _selectedVehicle.Make = txtMake.Text.Trim();
                    _selectedVehicle.Model = txtModel.Text.Trim();
                    _selectedVehicle.Year = int.Parse(txtYear.Text);
                    _selectedVehicle.Mileage = string.IsNullOrWhiteSpace(txtMileage.Text) ? null : int.Parse(txtMileage.Text);
                    _selectedVehicle.IsAvailable = chkAvailable.Checked;

                    _vehicleRepository.UpdateVehicle(_selectedVehicle);
                    MaterialMessageBox.Show("Vehicle updated successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadVehicles();
                ClearForm();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving vehicle: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedVehicle == null)
            {
                MaterialMessageBox.Show("Please select a vehicle to edit.", "Selection Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MaterialMessageBox.Show("You can now edit the vehicle details and click Save.", "Edit Mode", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedVehicle == null)
            {
                MaterialMessageBox.Show("Please select a vehicle to delete.", "Selection Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MaterialMessageBox.Show(
                $"Are you sure you want to delete vehicle '{_selectedVehicle.RegistrationNumber}'?", 
                "Confirm Delete", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _vehicleRepository.DeleteVehicle(_selectedVehicle.VehicleId);
                    MaterialMessageBox.Show("Vehicle deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadVehicles();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error deleting vehicle: {ex.Message}", "Error", 
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