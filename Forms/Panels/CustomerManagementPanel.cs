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
    public partial class CustomerManagementPanel : UserControl
    {
        private readonly CustomerRepository _customerRepository;
        private readonly UserRepository _userRepository;
        private Customer? _selectedCustomer;

        // Controls
        private MaterialCard cardCustomerList;
        private MaterialCard cardCustomerDetails;
        private DataGridView dgvCustomers;
        private MaterialTextBox txtSearch;
        private MaterialButton btnSearch;
        private MaterialButton btnRefresh;
        private MaterialTextBox txtFirstName;
        private MaterialTextBox txtLastName;
        private MaterialTextBox txtEmail;
        private MaterialTextBox txtPhone;
        private MaterialTextBox txtAddress;
        private MaterialTextBox txtCity;
        private MaterialTextBox txtPostalCode;
        private MaterialComboBox cmbCountry;
        private MaterialButton btnSave;
        private MaterialButton btnEdit;
        private MaterialButton btnDelete;
        private MaterialButton btnClear;

        public CustomerManagementPanel()
        {
            _customerRepository = new CustomerRepository();
            _userRepository = new UserRepository();
            InitializeComponent();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1000, 700);

            // Title
            var lblTitle = new MaterialLabel
            {
                Text = "Customer Management",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            CreateCustomerListCard();
            CreateCustomerDetailsCard();
        }

        private void CreateCustomerListCard()
        {
            cardCustomerList = new MaterialCard
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
                Size = new Size(350, 50),
                Hint = "Search customers...",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            btnSearch = new MaterialButton
            {
                Location = new Point(380, 30),
                Size = new Size(80, 30),
                Text = "SEARCH",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true,
                Density = MaterialButton.MaterialButtonDensity.Default,
                Depth = 0
            };
            btnSearch.Click += btnSearch_Click;

            btnRefresh = new MaterialButton
            {
                Location = new Point(470, 30),
                Size = new Size(80, 30),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false,
                Density = MaterialButton.MaterialButtonDensity.Default,
                Depth = 0
            };
            btnRefresh.Click += btnRefresh_Click;

            // Customer grid
            dgvCustomers = new DataGridView
            {
                Location = new Point(20, 90),
                Size = new Size(540, 480),
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
            dgvCustomers.SelectionChanged += dgvCustomers_SelectionChanged;

            cardCustomerList.Controls.Add(txtSearch);
            cardCustomerList.Controls.Add(btnSearch);
            cardCustomerList.Controls.Add(btnRefresh);
            cardCustomerList.Controls.Add(dgvCustomers);
            this.Controls.Add(cardCustomerList);
        }

        private void CreateCustomerDetailsCard()
        {
            cardCustomerDetails = new MaterialCard
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
                Text = "Customer Details",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            // Form fields
            txtFirstName = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(150, 50),
                Hint = "First Name",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtLastName = new MaterialTextBox
            {
                Location = new Point(180, 60),
                Size = new Size(150, 50),
                Hint = "Last Name",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtEmail = new MaterialTextBox
            {
                Location = new Point(20, 120),
                Size = new Size(310, 50),
                Hint = "Email Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPhone = new MaterialTextBox
            {
                Location = new Point(20, 180),
                Size = new Size(310, 50),
                Hint = "Phone Number",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtAddress = new MaterialTextBox
            {
                Location = new Point(20, 240),
                Size = new Size(310, 50),
                Hint = "Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Multiline = true
            };

            txtCity = new MaterialTextBox
            {
                Location = new Point(20, 300),
                Size = new Size(150, 50),
                Hint = "City",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPostalCode = new MaterialTextBox
            {
                Location = new Point(180, 300),
                Size = new Size(150, 50),
                Hint = "Postal Code",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbCountry = new MaterialComboBox
            {
                Location = new Point(20, 360),
                Size = new Size(150, 50),
                Hint = "Country",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbCountry.Items.AddRange(new object[] { "Sri Lanka", "USA", "Canada", "Australia", "Germany", "France" });
            cmbCountry.SelectedIndex = 0;

            // Action buttons
            btnSave = new MaterialButton
            {
                Location = new Point(20, 430),
                Size = new Size(70, 36),
                Text = "SAVE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSave.Click += btnSave_Click;

            btnEdit = new MaterialButton
            {
                Location = new Point(100, 430),
                Size = new Size(70, 36),
                Text = "EDIT",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnEdit.Click += btnEdit_Click;

            btnDelete = new MaterialButton
            {
                Location = new Point(180, 430),
                Size = new Size(70, 36),
                Text = "DELETE",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = true,
                HighEmphasis = false
            };
            btnDelete.Click += btnDelete_Click;

            btnClear = new MaterialButton
            {
                Location = new Point(260, 430),
                Size = new Size(70, 36),
                Text = "CLEAR",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnClear.Click += btnClear_Click;

            // Add all controls to details card
            cardCustomerDetails.Controls.Add(lblDetails);
            cardCustomerDetails.Controls.Add(txtFirstName);
            cardCustomerDetails.Controls.Add(txtLastName);
            cardCustomerDetails.Controls.Add(txtEmail);
            cardCustomerDetails.Controls.Add(txtPhone);
            cardCustomerDetails.Controls.Add(txtAddress);
            cardCustomerDetails.Controls.Add(txtCity);
            cardCustomerDetails.Controls.Add(txtPostalCode);
            cardCustomerDetails.Controls.Add(cmbCountry);
            cardCustomerDetails.Controls.Add(btnSave);
            cardCustomerDetails.Controls.Add(btnEdit);
            cardCustomerDetails.Controls.Add(btnDelete);
            cardCustomerDetails.Controls.Add(btnClear);

            this.Controls.Add(cardCustomerDetails);
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = _customerRepository.GetAllCustomers();
                var customerData = customers.Select(c => new
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    Email = c.User?.Email ?? "",
                    Phone = c.Phone,
                    City = c.City,
                    Country = c.Country,
                    RegistrationDate = c.RegistrationDate.ToString("dd/MM/yyyy")
                }).ToList();

                dgvCustomers.DataSource = customerData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading customers: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadCustomers();
                    return;
                }

                var customers = _customerRepository.SearchCustomers(txtSearch.Text.Trim());
                var customerData = customers.Select(c => new
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    Email = c.User?.Email ?? "",
                    Phone = c.Phone,
                    City = c.City,
                    Country = c.Country,
                    RegistrationDate = c.RegistrationDate.ToString("dd/MM/yyyy")
                }).ToList();

                dgvCustomers.DataSource = customerData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error searching customers: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadCustomers();
            ClearForm();
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedRow = dgvCustomers.SelectedRows[0];
                    var customerId = Convert.ToInt32(selectedRow.Cells["CustomerId"].Value);
                    
                    _selectedCustomer = _customerRepository.GetCustomerById(customerId);
                    if (_selectedCustomer != null)
                    {
                        PopulateForm(_selectedCustomer);
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error loading customer details: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PopulateForm(Customer customer)
        {
            txtFirstName.Text = customer.FirstName;
            txtLastName.Text = customer.LastName;
            txtEmail.Text = customer.User?.Email ?? "";
            txtPhone.Text = customer.Phone;
            txtAddress.Text = customer.Address;
            txtCity.Text = customer.City;
            txtPostalCode.Text = customer.PostalCode;
            cmbCountry.SelectedItem = customer.Country;
        }

        private void ClearForm()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtPostalCode.Text = "";
            cmbCountry.SelectedIndex = 0;
            _selectedCustomer = null;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtPostalCode.Text) ||
                cmbCountry.SelectedIndex == -1)
            {
                MaterialMessageBox.Show("Please fill in all required fields.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MaterialMessageBox.Show("Please enter a valid email address.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                if (_selectedCustomer == null)
                {
                    // Create new customer
                    var user = new User
                    {
                        Username = txtEmail.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        PasswordHash = PasswordHasher.HashPassword("Password123!"),
                        Role = "customer",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    var userId = _userRepository.AddUser(user);

                    var customer = new Customer
                    {
                        UserId = userId,
                        FirstName = txtFirstName.Text.Trim(),
                        LastName = txtLastName.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        City = txtCity.Text.Trim(),
                        PostalCode = txtPostalCode.Text.Trim(),
                        Country = cmbCountry.SelectedItem?.ToString() ?? "Sri Lanka",
                        RegistrationDate = DateTime.Now.Date
                    };

                    _customerRepository.AddCustomer(customer);
                    MaterialMessageBox.Show("Customer created successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update existing customer
                    _selectedCustomer.FirstName = txtFirstName.Text.Trim();
                    _selectedCustomer.LastName = txtLastName.Text.Trim();
                    _selectedCustomer.Phone = txtPhone.Text.Trim();
                    _selectedCustomer.Address = txtAddress.Text.Trim();
                    _selectedCustomer.City = txtCity.Text.Trim();
                    _selectedCustomer.PostalCode = txtPostalCode.Text.Trim();
                    _selectedCustomer.Country = cmbCountry.SelectedItem?.ToString() ?? "Sri Lanka";

                    _customerRepository.UpdateCustomer(_selectedCustomer);

                    if (_selectedCustomer.User != null && _selectedCustomer.User.Email != txtEmail.Text.Trim())
                    {
                        _selectedCustomer.User.Email = txtEmail.Text.Trim();
                        _selectedCustomer.User.Username = txtEmail.Text.Trim();
                        _userRepository.UpdateUser(_selectedCustomer.User);
                    }

                    MaterialMessageBox.Show("Customer updated successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadCustomers();
                ClearForm();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving customer: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedCustomer == null)
            {
                MaterialMessageBox.Show("Please select a customer to edit.", "Selection Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MaterialMessageBox.Show("You can now edit the customer details and click Save.", "Edit Mode", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCustomer == null)
            {
                MaterialMessageBox.Show("Please select a customer to delete.", "Selection Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MaterialMessageBox.Show(
                $"Are you sure you want to delete customer '{_selectedCustomer.FullName}'?", 
                "Confirm Delete", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _customerRepository.DeleteCustomer(_selectedCustomer.CustomerId);
                    MaterialMessageBox.Show("Customer deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadCustomers();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error deleting customer: {ex.Message}", "Error", 
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