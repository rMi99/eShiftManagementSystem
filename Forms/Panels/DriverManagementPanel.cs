using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Business.Services;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace eShiftManagementSystem.Forms
{
    public partial class DriverManagementPanel : UserControl
    {
        private readonly StaffService _staffService;
        private readonly JobService _jobService;
        private readonly VehicleService _vehicleService;
        private Staff _selectedDriver;

        // Controls
        private MaterialCard cardDriverList;
        private MaterialCard cardDriverDetails;
        private DataGridView dgvDrivers;
        private MaterialTextBox txtSearch;
        private MaterialButton btnSearch;
        private MaterialButton btnRefresh;
        private MaterialButton btnAddNew;
        private MaterialTextBox txtFirstName;
        private MaterialTextBox txtLastName;
        private MaterialTextBox txtPhone;
        private MaterialComboBox cmbAssignedVehicle;
        private MaterialComboBox cmbAssignedJob;
        private MaterialButton btnSave;
        private MaterialButton btnDelete;

        public DriverManagementPanel()
        {
            _staffService = new StaffService();
            _jobService = new JobService();
            _vehicleService = new VehicleService();
            InitializeComponent();
            LoadDrivers();
            LoadVehicles();
            LoadJobs();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1000, 700);

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
                Size = new Size(580, 600),
                Padding = new Padding(20)
            };

            txtSearch = new MaterialTextBox { Location = new Point(20, 20), Size = new Size(300, 50), Hint = "Search drivers..." };
            btnSearch = new MaterialButton { Location = new Point(330, 30), Size = new Size(80, 30), Text = "SEARCH" };
            btnRefresh = new MaterialButton { Location = new Point(420, 30), Size = new Size(80, 30), Text = "REFRESH", Type = MaterialButton.MaterialButtonType.Outlined };
            btnAddNew = new MaterialButton { Location = new Point(20, 80), Size = new Size(100, 30), Text = "ADD NEW" };

            dgvDrivers = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(540, 450),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
            };

            btnSearch.Click += (s, e) => LoadDrivers(txtSearch.Text);
            btnRefresh.Click += (s, e) => LoadDrivers();
            btnAddNew.Click += (s, e) => ClearForm();
            dgvDrivers.SelectionChanged += dgvDrivers_SelectionChanged;

            cardDriverList.Controls.AddRange(new Control[] { txtSearch, btnSearch, btnRefresh, btnAddNew, dgvDrivers });
            this.Controls.Add(cardDriverList);
        }

        private void CreateDriverDetailsCard()
        {
            cardDriverDetails = new MaterialCard
            {
                Location = new Point(620, 70),
                Size = new Size(350, 600),
                Padding = new Padding(20)
            };

            var lblDetails = new MaterialLabel { Text = "Driver Details", Location = new Point(20, 20), AutoSize = true, FontType = MaterialSkinManager.fontType.H6 };
            txtFirstName = new MaterialTextBox { Hint = "First Name", Location = new Point(20, 60), Size = new Size(310, 50) };
            txtLastName = new MaterialTextBox { Hint = "Last Name", Location = new Point(20, 120), Size = new Size(310, 50) };
            txtPhone = new MaterialTextBox { Hint = "Phone", Location = new Point(20, 180), Size = new Size(310, 50) };
            cmbAssignedVehicle = new MaterialComboBox { Hint = "Assign Vehicle", Location = new Point(20, 240), Size = new Size(310, 50) };
            cmbAssignedJob = new MaterialComboBox { Hint = "Assign Job", Location = new Point(20, 300), Size = new Size(310, 50) };

            btnSave = new MaterialButton { Text = "SAVE", Location = new Point(20, 370), Size = new Size(150, 36) };
            btnDelete = new MaterialButton { Text = "DELETE", Location = new Point(180, 370), Size = new Size(150, 36), Type = MaterialButton.MaterialButtonType.Outlined };

            btnSave.Click += btnSave_Click;
            btnDelete.Click += btnDelete_Click;

            cardDriverDetails.Controls.AddRange(new Control[] { lblDetails, txtFirstName, txtLastName, txtPhone, cmbAssignedVehicle, cmbAssignedJob, btnSave, btnDelete });
            this.Controls.Add(cardDriverDetails);
        }

        private void LoadDrivers(string searchTerm = null)
        {
            var drivers = string.IsNullOrEmpty(searchTerm) ? _staffService.GetAllDrivers() : _staffService.GetAllDrivers().Where(d => d.FullName.ToLower().Contains(searchTerm.ToLower())).ToList();
            dgvDrivers.DataSource = drivers.Select(d => new { d.StaffId, d.FullName, d.Phone, d.Position }).ToList();
        }

        private void LoadVehicles()
        {
            var vehicles = _vehicleService.GetAllVehicles();
            cmbAssignedVehicle.DataSource = vehicles;
            cmbAssignedVehicle.DisplayMember = "RegistrationNumber";
            cmbAssignedVehicle.ValueMember = "VehicleId";
        }

        private void LoadJobs()
        {
            var jobs = _jobService.GetAllJobs();
            cmbAssignedJob.DataSource = jobs;
            cmbAssignedJob.DisplayMember = "JobNumber";
            cmbAssignedJob.ValueMember = "JobId";
        }

        private void dgvDrivers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDrivers.SelectedRows.Count > 0)
            {
                var staffId = (int)dgvDrivers.SelectedRows[0].Cells["StaffId"].Value;
                _selectedDriver = _staffService.GetStaffById(staffId);
                PopulateForm();
            }
        }

        private void PopulateForm()
        {
            if (_selectedDriver != null)
            {
                txtFirstName.Text = _selectedDriver.FirstName;
                txtLastName.Text = _selectedDriver.LastName;
                txtPhone.Text = _selectedDriver.Phone;
            }
        }

        private void ClearForm()
        {
            _selectedDriver = null;
            txtFirstName.Clear();
            txtLastName.Clear();
            txtPhone.Clear();
            cmbAssignedVehicle.SelectedIndex = -1;
            cmbAssignedJob.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_selectedDriver == null) // Add new driver
            {
                // In a real application, you would create a new User first, then the Staff
                MessageBox.Show("Adding new drivers is not fully implemented in this example.");
            }
            else // Update existing driver
            {
                _selectedDriver.FirstName = txtFirstName.Text;
                _selectedDriver.LastName = txtLastName.Text;
                _selectedDriver.Phone = txtPhone.Text;
                _staffService.UpdateStaff(_selectedDriver);

                // Update Job-Driver-Vehicle assignment
                var jobId = (int)cmbAssignedJob.SelectedValue;
                var vehicleId = (int)cmbAssignedVehicle.SelectedValue;
                _jobService.AssignJobToDriver(jobId, _selectedDriver.StaffId);
                // You would also have a method to assign a vehicle to a driver
                // _vehicleService.AssignVehicleToDriver(vehicleId, _selectedDriver.StaffId);


                MessageBox.Show("Driver details updated successfully.");
                LoadDrivers();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedDriver != null)
            {
                _staffService.DeleteStaff(_selectedDriver.StaffId);
                MessageBox.Show("Driver deleted successfully.");
                LoadDrivers();
                ClearForm();
            }
        }
    }
}