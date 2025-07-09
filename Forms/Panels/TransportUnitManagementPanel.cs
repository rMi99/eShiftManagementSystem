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
    public partial class TransportUnitManagementPanel : UserControl
    {
        private readonly TransportUnitRepository _transportUnitRepository;
        private readonly VehicleRepository _vehicleRepository;
        private readonly DriverRepository _driverRepository;
        private readonly AssistantRepository _assistantRepository;
        private readonly ContainerRepository _containerRepository;
        private readonly AuditService _auditService;
        private TransportUnit? _selectedTransportUnit;

        // Controls
        private MaterialCard cardTransportUnitList;
        private MaterialCard cardTransportUnitDetails;
        private DataGridView dgvTransportUnits;
        private MaterialTextBox txtSearch;
        private MaterialComboBox cmbStatusFilter;
        private MaterialButton btnSearch;
        private MaterialButton btnRefresh;
        private MaterialButton btnAddNew;
        private MaterialTextBox txtUnitName;
        private MaterialComboBox cmbVehicle;
        private MaterialComboBox cmbDriver;
        private MaterialComboBox cmbAssistant;
        private MaterialComboBox cmbContainer;
        private MaterialCheckbox chkIsActive;
        private MaterialLabel lblTeamComposition;
        private MaterialLabel lblCapacityInfo;
        private MaterialButton btnSave;
        private MaterialButton btnEdit;
        private MaterialButton btnDelete;
        private MaterialButton btnClear;

        public TransportUnitManagementPanel()
        {
            _transportUnitRepository = new TransportUnitRepository();
            _vehicleRepository = new VehicleRepository();
            _driverRepository = new DriverRepository();
            _assistantRepository = new AssistantRepository();
            _containerRepository = new ContainerRepository();
            _auditService = new AuditService();
            InitializeComponent();
            LoadDropdownData();
            LoadTransportUnits();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1200, 800);

            var lblTitle = new MaterialLabel
            {
                Text = "Transport Unit Management",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            CreateTransportUnitListCard();
            CreateTransportUnitDetailsCard();
        }

        private void CreateTransportUnitListCard()
        {
            cardTransportUnitList = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(750, 500),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblTransportUnitList = new MaterialLabel
            {
                Text = "Transport Unit List",
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
                Hint = "Search transport units...",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbStatusFilter = new MaterialComboBox
            {
                Location = new Point(340, 60),
                Size = new Size(120, 50),
                Hint = "Status",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbStatusFilter.Items.AddRange(new object[] { "All", "Active", "Inactive" });
            cmbStatusFilter.SelectedIndex = 0;

            btnSearch = new MaterialButton
            {
                Location = new Point(480, 60),
                Size = new Size(100, 50),
                Text = "SEARCH",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnSearch.Click += btnSearch_Click;

            btnRefresh = new MaterialButton
            {
                Location = new Point(600, 60),
                Size = new Size(100, 50),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnRefresh.Click += btnRefresh_Click;

            dgvTransportUnits = new DataGridView
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
            dgvTransportUnits.SelectionChanged += dgvTransportUnits_SelectionChanged;

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

            cardTransportUnitList.Controls.AddRange(new Control[] {
                lblTransportUnitList, txtSearch, cmbStatusFilter, btnSearch, btnRefresh, dgvTransportUnits, btnAddNew
            });

            this.Controls.Add(cardTransportUnitList);
        }

        private void CreateTransportUnitDetailsCard()
        {
            cardTransportUnitDetails = new MaterialCard
            {
                Location = new Point(800, 70),
                Size = new Size(380, 500),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblTransportUnitDetails = new MaterialLabel
            {
                Text = "Transport Unit Details",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtUnitName = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(320, 50),
                Hint = "Unit Name",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbVehicle = new MaterialComboBox
            {
                Location = new Point(20, 120),
                Size = new Size(320, 50),
                Hint = "Vehicle",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbDriver = new MaterialComboBox
            {
                Location = new Point(20, 180),
                Size = new Size(320, 50),
                Hint = "Driver (Optional)",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbAssistant = new MaterialComboBox
            {
                Location = new Point(20, 240),
                Size = new Size(320, 50),
                Hint = "Assistant (Optional)",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbContainer = new MaterialComboBox
            {
                Location = new Point(20, 300),
                Size = new Size(320, 50),
                Hint = "Container (Optional)",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            chkIsActive = new MaterialCheckbox
            {
                Location = new Point(20, 360),
                Size = new Size(150, 40),
                Text = "Active",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.HOVER,
                Checked = true
            };

            lblTeamComposition = new MaterialLabel
            {
                Text = "Team: Not configured",
                Location = new Point(20, 400),
                Size = new Size(320, 20),
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Body2,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            lblCapacityInfo = new MaterialLabel
            {
                Text = "Capacity: Not available",
                Location = new Point(20, 425),
                Size = new Size(320, 20),
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Body2,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            // Buttons
            btnSave = new MaterialButton
            {
                Location = new Point(20, 460),
                Size = new Size(70, 35),
                Text = "SAVE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = true,
                UseVisualStyleBackColor = true
            };
            btnSave.Click += btnSave_Click;

            btnEdit = new MaterialButton
            {
                Location = new Point(100, 460),
                Size = new Size(70, 35),
                Text = "EDIT",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnEdit.Click += btnEdit_Click;

            btnDelete = new MaterialButton
            {
                Location = new Point(180, 460),
                Size = new Size(70, 35),
                Text = "DELETE",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnDelete.Click += btnDelete_Click;

            btnClear = new MaterialButton
            {
                Location = new Point(260, 460),
                Size = new Size(70, 35),
                Text = "CLEAR",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnClear.Click += btnClear_Click;

            cardTransportUnitDetails.Controls.AddRange(new Control[] {
                lblTransportUnitDetails, txtUnitName, cmbVehicle, cmbDriver, cmbAssistant, cmbContainer,
                chkIsActive, lblTeamComposition, lblCapacityInfo, btnSave, btnEdit, btnDelete, btnClear
            });

            this.Controls.Add(cardTransportUnitDetails);
        }

        private void LoadDropdownData()
        {
            try
            {
                // Load vehicles
                var vehicles = _vehicleRepository.GetAvailableVehicles();
                cmbVehicle.Items.Clear();
                foreach (var vehicle in vehicles)
                {
                    cmbVehicle.Items.Add(new { Text = vehicle.DisplayName, Value = vehicle.VehicleId });
                }
                if (cmbVehicle.Items.Count > 0)
                {
                    cmbVehicle.DisplayMember = "Text";
                    cmbVehicle.ValueMember = "Value";
                }

                // Load drivers
                var drivers = _driverRepository.GetAvailableDrivers();
                cmbDriver.Items.Clear();
                cmbDriver.Items.Add(new { Text = "No Driver", Value = (int?)null });
                foreach (var driver in drivers)
                {
                    cmbDriver.Items.Add(new { Text = driver.FullName, Value = (int?)driver.DriverId });
                }
                if (cmbDriver.Items.Count > 0)
                {
                    cmbDriver.DisplayMember = "Text";
                    cmbDriver.ValueMember = "Value";
                    cmbDriver.SelectedIndex = 0;
                }

                // Load assistants
                var assistants = _assistantRepository.GetAvailableAssistants();
                cmbAssistant.Items.Clear();
                cmbAssistant.Items.Add(new { Text = "No Assistant", Value = (int?)null });
                foreach (var assistant in assistants)
                {
                    cmbAssistant.Items.Add(new { Text = assistant.FullName, Value = (int?)assistant.AssistantId });
                }
                if (cmbAssistant.Items.Count > 0)
                {
                    cmbAssistant.DisplayMember = "Text";
                    cmbAssistant.ValueMember = "Value";
                    cmbAssistant.SelectedIndex = 0;
                }

                // Load containers
                var containers = _containerRepository.GetAvailableContainers();
                cmbContainer.Items.Clear();
                cmbContainer.Items.Add(new { Text = "No Container", Value = (int?)null });
                foreach (var container in containers)
                {
                    cmbContainer.Items.Add(new { Text = container.ContainerNumber, Value = (int?)container.ContainerId });
                }
                if (cmbContainer.Items.Count > 0)
                {
                    cmbContainer.DisplayMember = "Text";
                    cmbContainer.ValueMember = "Value";
                    cmbContainer.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading dropdown data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTransportUnits()
        {
            try
            {
                var transportUnits = _transportUnitRepository.GetAllTransportUnits();
                var unitData = transportUnits.Select(tu => new
                {
                    TransportUnitId = tu.TransportUnitId,
                    UnitName = tu.UnitName,
                    Vehicle = tu.Vehicle?.RegistrationNumber ?? "No Vehicle",
                    Driver = tu.Driver?.FullName ?? "No Driver",
                    Assistant = tu.Assistant?.FullName ?? "No Assistant",
                    Status = tu.Status
                }).ToList();

                dgvTransportUnits.DataSource = unitData;

                if (dgvTransportUnits.Columns.Count > 0)
                {
                    dgvTransportUnits.Columns["TransportUnitId"].Visible = false;
                    dgvTransportUnits.Columns["UnitName"].HeaderText = "Unit Name";
                    dgvTransportUnits.Columns["Vehicle"].HeaderText = "Vehicle";
                    dgvTransportUnits.Columns["Driver"].HeaderText = "Driver";
                    dgvTransportUnits.Columns["Assistant"].HeaderText = "Assistant";
                    dgvTransportUnits.Columns["Status"].HeaderText = "Status";
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading transport units: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var allUnits = _transportUnitRepository.GetAllTransportUnits();
                var filteredUnits = allUnits.Where(tu =>
                {
                    bool matchesSearch = string.IsNullOrWhiteSpace(txtSearch.Text) ||
                        tu.UnitName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                        (tu.Vehicle?.RegistrationNumber?.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase) ?? false);

                    bool matchesStatus = cmbStatusFilter.SelectedIndex == 0 || // "All"
                        (cmbStatusFilter.SelectedIndex == 1 && tu.IsActive) || // "Active"
                        (cmbStatusFilter.SelectedIndex == 2 && !tu.IsActive); // "Inactive"

                    return matchesSearch && matchesStatus;
                }).ToList();

                var unitData = filteredUnits.Select(tu => new
                {
                    TransportUnitId = tu.TransportUnitId,
                    UnitName = tu.UnitName,
                    Vehicle = tu.Vehicle?.RegistrationNumber ?? "No Vehicle",
                    Driver = tu.Driver?.FullName ?? "No Driver",
                    Assistant = tu.Assistant?.FullName ?? "No Assistant",
                    Status = tu.Status
                }).ToList();

                dgvTransportUnits.DataSource = unitData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error searching transport units: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDropdownData();
            LoadTransportUnits();
            ClearForm();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtUnitName.Focus();
        }

        private void dgvTransportUnits_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTransportUnits.SelectedRows.Count > 0)
            {
                var selectedRow = dgvTransportUnits.SelectedRows[0];
                var transportUnitId = Convert.ToInt32(selectedRow.Cells["TransportUnitId"].Value);
                
                var transportUnit = _transportUnitRepository.GetTransportUnitById(transportUnitId);
                if (transportUnit != null)
                {
                    PopulateForm(transportUnit);
                }
            }
        }

        private void PopulateForm(TransportUnit transportUnit)
        {
            _selectedTransportUnit = transportUnit;
            txtUnitName.Text = transportUnit.UnitName;
            chkIsActive.Checked = transportUnit.IsActive;

            // Set vehicle selection
            for (int i = 0; i < cmbVehicle.Items.Count; i++)
            {
                dynamic item = cmbVehicle.Items[i];
                if (item.Value == transportUnit.VehicleId)
                {
                    cmbVehicle.SelectedIndex = i;
                    break;
                }
            }

            // Set driver selection
            for (int i = 0; i < cmbDriver.Items.Count; i++)
            {
                dynamic item = cmbDriver.Items[i];
                if (item.Value == transportUnit.DriverId)
                {
                    cmbDriver.SelectedIndex = i;
                    break;
                }
            }

            // Set assistant selection
            for (int i = 0; i < cmbAssistant.Items.Count; i++)
            {
                dynamic item = cmbAssistant.Items[i];
                if (item.Value == transportUnit.AssistantId)
                {
                    cmbAssistant.SelectedIndex = i;
                    break;
                }
            }

            // Set container selection
            for (int i = 0; i < cmbContainer.Items.Count; i++)
            {
                dynamic item = cmbContainer.Items[i];
                if (item.Value == transportUnit.ContainerId)
                {
                    cmbContainer.SelectedIndex = i;
                    break;
                }
            }

            UpdateInfoLabels();
        }

        private void ClearForm()
        {
            _selectedTransportUnit = null;
            txtUnitName.Clear();
            cmbVehicle.SelectedIndex = -1;
            cmbDriver.SelectedIndex = 0; // "No Driver"
            cmbAssistant.SelectedIndex = 0; // "No Assistant"
            cmbContainer.SelectedIndex = 0; // "No Container"
            chkIsActive.Checked = true;
            UpdateInfoLabels();
        }

        private void UpdateInfoLabels()
        {
            var teamParts = new List<string>();
            
            if (cmbVehicle.SelectedIndex >= 0)
            {
                dynamic vehicle = cmbVehicle.SelectedItem;
                teamParts.Add($"Vehicle: {vehicle.Text}");
            }

            if (cmbDriver.SelectedIndex > 0) // Skip "No Driver"
            {
                dynamic driver = cmbDriver.SelectedItem;
                teamParts.Add($"Driver: {driver.Text}");
            }

            if (cmbAssistant.SelectedIndex > 0) // Skip "No Assistant"
            {
                dynamic assistant = cmbAssistant.SelectedItem;
                teamParts.Add($"Assistant: {assistant.Text}");
            }

            lblTeamComposition.Text = teamParts.Any() ? $"Team: {string.Join(", ", teamParts)}" : "Team: Not configured";
            lblCapacityInfo.Text = "Capacity: Select vehicle for details";
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUnitName.Text))
            {
                MaterialMessageBox.Show("Unit name is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbVehicle.SelectedIndex < 0)
            {
                MaterialMessageBox.Show("Vehicle selection is required.", "Validation Error", 
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
                if (_selectedTransportUnit == null)
                {
                    // Check if unit name already exists
                    if (_transportUnitRepository.UnitNameExists(txtUnitName.Text.Trim()))
                    {
                        MaterialMessageBox.Show("A transport unit with this name already exists.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Create new transport unit
                    var transportUnit = new TransportUnit
                    {
                        UnitName = txtUnitName.Text.Trim(),
                        VehicleId = (int)((dynamic)cmbVehicle.SelectedItem).Value,
                        DriverId = cmbDriver.SelectedIndex > 0 ? (int?)((dynamic)cmbDriver.SelectedItem).Value : null,
                        AssistantId = cmbAssistant.SelectedIndex > 0 ? (int?)((dynamic)cmbAssistant.SelectedItem).Value : null,
                        ContainerId = cmbContainer.SelectedIndex > 0 ? (int?)((dynamic)cmbContainer.SelectedItem).Value : null,
                        IsActive = chkIsActive.Checked
                    };

                    var transportUnitId = _transportUnitRepository.AddTransportUnit(transportUnit);
                    transportUnit.TransportUnitId = transportUnitId;
                    
                    _auditService.LogCreate("transport_units", transportUnitId, transportUnit, SessionManager.CurrentUserId);
                    
                    MaterialMessageBox.Show("Transport unit created successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update existing transport unit
                    var oldTransportUnit = _selectedTransportUnit;
                    _selectedTransportUnit.UnitName = txtUnitName.Text.Trim();
                    _selectedTransportUnit.VehicleId = (int)((dynamic)cmbVehicle.SelectedItem).Value;
                    _selectedTransportUnit.DriverId = cmbDriver.SelectedIndex > 0 ? (int?)((dynamic)cmbDriver.SelectedItem).Value : null;
                    _selectedTransportUnit.AssistantId = cmbAssistant.SelectedIndex > 0 ? (int?)((dynamic)cmbAssistant.SelectedItem).Value : null;
                    _selectedTransportUnit.ContainerId = cmbContainer.SelectedIndex > 0 ? (int?)((dynamic)cmbContainer.SelectedItem).Value : null;
                    _selectedTransportUnit.IsActive = chkIsActive.Checked;

                    _transportUnitRepository.UpdateTransportUnit(_selectedTransportUnit);
                    _auditService.LogUpdate("transport_units", _selectedTransportUnit.TransportUnitId, oldTransportUnit, _selectedTransportUnit, SessionManager.CurrentUserId);
                    
                    MaterialMessageBox.Show("Transport unit updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadDropdownData(); // Refresh availability
                LoadTransportUnits();
                ClearForm();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving transport unit: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedTransportUnit == null)
            {
                MaterialMessageBox.Show("Please select a transport unit to edit.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Form is already populated for editing
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedTransportUnit == null)
            {
                MaterialMessageBox.Show("Please select a transport unit to delete.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MaterialMessageBox.Show(
                $"Are you sure you want to delete transport unit '{_selectedTransportUnit.UnitName}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _auditService.LogDelete("transport_units", _selectedTransportUnit.TransportUnitId, _selectedTransportUnit, SessionManager.CurrentUserId);
                    _transportUnitRepository.DeleteTransportUnit(_selectedTransportUnit.TransportUnitId);
                    
                    MaterialMessageBox.Show("Transport unit deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDropdownData(); // Refresh availability
                    LoadTransportUnits();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error deleting transport unit: {ex.Message}", "Error",
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