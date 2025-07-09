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
    public partial class ContainerManagementPanel : UserControl
    {
        private readonly ContainerRepository _containerRepository;
        private readonly AuditService _auditService;
        private Container? _selectedContainer;

        // Controls
        private MaterialCard cardContainerList;
        private MaterialCard cardContainerDetails;
        private DataGridView dgvContainers;
        private MaterialTextBox txtSearch;
        private MaterialButton btnSearch;
        private MaterialButton btnRefresh;
        private MaterialButton btnAddNew;
        private MaterialTextBox txtContainerNumber;
        private MaterialComboBox cmbType;
        private MaterialTextBox txtMaxWeight;
        private MaterialTextBox txtMaxVolume;
        private MaterialCheckbox chkIsAvailable;
        private MaterialComboBox cmbCondition;
        private DateTimePicker dtpLastInspection;
        private MaterialButton btnSave;
        private MaterialButton btnEdit;
        private MaterialButton btnDelete;
        private MaterialButton btnClear;

        public ContainerManagementPanel()
        {
            _containerRepository = new ContainerRepository();
            _auditService = new AuditService();
            InitializeComponent();
            LoadContainers();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1200, 800);

            var lblTitle = new MaterialLabel
            {
                Text = "Container Management",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            CreateContainerListCard();
            CreateContainerDetailsCard();
        }

        private void CreateContainerListCard()
        {
            cardContainerList = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(700, 500),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblContainerList = new MaterialLabel
            {
                Text = "Container List",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtSearch = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(400, 50),
                Hint = "Search containers by number or type...",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            btnSearch = new MaterialButton
            {
                Location = new Point(440, 60),
                Size = new Size(100, 50),
                Text = "SEARCH",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnSearch.Click += btnSearch_Click;

            btnRefresh = new MaterialButton
            {
                Location = new Point(560, 60),
                Size = new Size(100, 50),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnRefresh.Click += btnRefresh_Click;

            dgvContainers = new DataGridView
            {
                Location = new Point(20, 130),
                Size = new Size(640, 300),
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
            dgvContainers.SelectionChanged += dgvContainers_SelectionChanged;

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

            cardContainerList.Controls.AddRange(new Control[] {
                lblContainerList, txtSearch, btnSearch, btnRefresh, dgvContainers, btnAddNew
            });

            this.Controls.Add(cardContainerList);
        }

        private void CreateContainerDetailsCard()
        {
            cardContainerDetails = new MaterialCard
            {
                Location = new Point(750, 70),
                Size = new Size(420, 500),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblContainerDetails = new MaterialLabel
            {
                Text = "Container Details",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtContainerNumber = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(360, 50),
                Hint = "Container Number",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbType = new MaterialComboBox
            {
                Location = new Point(20, 120),
                Size = new Size(180, 50),
                Hint = "Container Type",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbType.Items.AddRange(new object[] { "Standard", "Refrigerated", "Open Top", "Flat Rack", "Tank", "Special" });

            cmbCondition = new MaterialComboBox
            {
                Location = new Point(210, 120),
                Size = new Size(170, 50),
                Hint = "Condition",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbCondition.Items.AddRange(new object[] { "Excellent", "Good", "Fair", "Poor", "Needs Repair" });

            txtMaxWeight = new MaterialTextBox
            {
                Location = new Point(20, 180),
                Size = new Size(180, 50),
                Hint = "Max Weight (kg)",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtMaxVolume = new MaterialTextBox
            {
                Location = new Point(210, 180),
                Size = new Size(170, 50),
                Hint = "Max Volume (m³)",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            chkIsAvailable = new MaterialCheckbox
            {
                Location = new Point(20, 240),
                Size = new Size(150, 40),
                Text = "Available",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.HOVER,
                Checked = true
            };

            var lblLastInspection = new MaterialLabel
            {
                Text = "Last Inspection Date",
                Location = new Point(20, 280),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Body1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            dtpLastInspection = new DateTimePicker
            {
                Location = new Point(20, 310),
                Size = new Size(200, 30),
                Format = DateTimePickerFormat.Short
            };

            // Buttons
            btnSave = new MaterialButton
            {
                Location = new Point(20, 360),
                Size = new Size(80, 40),
                Text = "SAVE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = true,
                UseVisualStyleBackColor = true
            };
            btnSave.Click += btnSave_Click;

            btnEdit = new MaterialButton
            {
                Location = new Point(110, 360),
                Size = new Size(80, 40),
                Text = "EDIT",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnEdit.Click += btnEdit_Click;

            btnDelete = new MaterialButton
            {
                Location = new Point(200, 360),
                Size = new Size(80, 40),
                Text = "DELETE",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnDelete.Click += btnDelete_Click;

            btnClear = new MaterialButton
            {
                Location = new Point(290, 360),
                Size = new Size(80, 40),
                Text = "CLEAR",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnClear.Click += btnClear_Click;

            cardContainerDetails.Controls.AddRange(new Control[] {
                lblContainerDetails, txtContainerNumber, cmbType, cmbCondition, txtMaxWeight, txtMaxVolume,
                chkIsAvailable, lblLastInspection, dtpLastInspection, btnSave, btnEdit, btnDelete, btnClear
            });

            this.Controls.Add(cardContainerDetails);
        }

        private void LoadContainers()
        {
            try
            {
                var containers = _containerRepository.GetAllContainers();
                var containerData = containers.Select(c => new
                {
                    ContainerId = c.ContainerId,
                    ContainerNumber = c.ContainerNumber,
                    Type = c.Type,
                    MaxWeight = $"{c.MaxWeight:N0} kg",
                    MaxVolume = $"{c.MaxVolume:N1} m³",
                    Status = c.IsAvailable ? "Available" : "In Use"
                }).ToList();

                dgvContainers.DataSource = containerData;

                if (dgvContainers.Columns.Count > 0)
                {
                    dgvContainers.Columns["ContainerId"].Visible = false;
                    dgvContainers.Columns["ContainerNumber"].HeaderText = "Container Number";
                    dgvContainers.Columns["Type"].HeaderText = "Type";
                    dgvContainers.Columns["MaxWeight"].HeaderText = "Max Weight";
                    dgvContainers.Columns["MaxVolume"].HeaderText = "Max Volume";
                    dgvContainers.Columns["Status"].HeaderText = "Status";
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading containers: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var allContainers = _containerRepository.GetAllContainers();
                var filteredContainers = allContainers.Where(c =>
                    string.IsNullOrWhiteSpace(txtSearch.Text) ||
                    c.ContainerNumber.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                    c.Type.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                var containerData = filteredContainers.Select(c => new
                {
                    ContainerId = c.ContainerId,
                    ContainerNumber = c.ContainerNumber,
                    Type = c.Type,
                    MaxWeight = $"{c.MaxWeight:N0} kg",
                    MaxVolume = $"{c.MaxVolume:N1} m³",
                    Status = c.IsAvailable ? "Available" : "In Use"
                }).ToList();

                dgvContainers.DataSource = containerData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error searching containers: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadContainers();
            ClearForm();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtContainerNumber.Focus();
        }

        private void dgvContainers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvContainers.SelectedRows.Count > 0)
            {
                var selectedRow = dgvContainers.SelectedRows[0];
                var containerId = Convert.ToInt32(selectedRow.Cells["ContainerId"].Value);
                
                var container = _containerRepository.GetContainerById(containerId);
                if (container != null)
                {
                    PopulateForm(container);
                }
            }
        }

        private void PopulateForm(Container container)
        {
            _selectedContainer = container;
            txtContainerNumber.Text = container.ContainerNumber;
            cmbType.Text = container.Type;
            txtMaxWeight.Text = container.MaxWeight.ToString("F2");
            txtMaxVolume.Text = container.MaxVolume.ToString("F2");
            chkIsAvailable.Checked = container.IsAvailable;
            // dtpLastInspection would need to be set if we had that property
        }

        private void ClearForm()
        {
            _selectedContainer = null;
            txtContainerNumber.Clear();
            cmbType.SelectedIndex = -1;
            txtMaxWeight.Clear();
            txtMaxVolume.Clear();
            chkIsAvailable.Checked = true;
            cmbCondition.SelectedIndex = -1;
            dtpLastInspection.Value = DateTime.Now;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtContainerNumber.Text))
            {
                MaterialMessageBox.Show("Container number is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbType.SelectedIndex == -1)
            {
                MaterialMessageBox.Show("Container type is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtMaxWeight.Text, out _) || decimal.Parse(txtMaxWeight.Text) <= 0)
            {
                MaterialMessageBox.Show("Please enter a valid maximum weight.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtMaxVolume.Text, out _) || decimal.Parse(txtMaxVolume.Text) <= 0)
            {
                MaterialMessageBox.Show("Please enter a valid maximum volume.", "Validation Error", 
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
                if (_selectedContainer == null)
                {
                    // Check if container number already exists
                    if (_containerRepository.ContainerExists(txtContainerNumber.Text.Trim()))
                    {
                        MaterialMessageBox.Show("A container with this number already exists.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Create new container
                    var container = new Container
                    {
                        ContainerNumber = txtContainerNumber.Text.Trim(),
                        Type = cmbType.SelectedItem?.ToString() ?? string.Empty,
                        MaxWeight = decimal.Parse(txtMaxWeight.Text),
                        MaxVolume = decimal.Parse(txtMaxVolume.Text),
                        IsAvailable = chkIsAvailable.Checked
                    };

                    var containerId = _containerRepository.AddContainer(container);
                    container.ContainerId = containerId;
                    
                    _auditService.LogCreate("containers", containerId, container, SessionManager.CurrentUserId);
                    
                    MaterialMessageBox.Show("Container added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update existing container
                    var oldContainer = _selectedContainer;
                    _selectedContainer.ContainerNumber = txtContainerNumber.Text.Trim();
                    _selectedContainer.Type = cmbType.SelectedItem?.ToString() ?? string.Empty;
                    _selectedContainer.MaxWeight = decimal.Parse(txtMaxWeight.Text);
                    _selectedContainer.MaxVolume = decimal.Parse(txtMaxVolume.Text);
                    _selectedContainer.IsAvailable = chkIsAvailable.Checked;

                    _containerRepository.UpdateContainer(_selectedContainer);
                    _auditService.LogUpdate("containers", _selectedContainer.ContainerId, oldContainer, _selectedContainer, SessionManager.CurrentUserId);
                    
                    MaterialMessageBox.Show("Container updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadContainers();
                ClearForm();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving container: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedContainer == null)
            {
                MaterialMessageBox.Show("Please select a container to edit.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Form is already populated for editing
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedContainer == null)
            {
                MaterialMessageBox.Show("Please select a container to delete.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MaterialMessageBox.Show(
                $"Are you sure you want to delete container '{_selectedContainer.ContainerNumber}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _auditService.LogDelete("containers", _selectedContainer.ContainerId, _selectedContainer, SessionManager.CurrentUserId);
                    _containerRepository.DeleteContainer(_selectedContainer.ContainerId);
                    
                    MaterialMessageBox.Show("Container deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadContainers();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error deleting container: {ex.Message}", "Error",
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