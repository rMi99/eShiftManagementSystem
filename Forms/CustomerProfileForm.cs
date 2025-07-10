using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace eShiftManagementSystem.Forms
{
    public partial class CustomerProfileForm : MaterialForm
    {
        private readonly CustomerService _customerService;
        private readonly MaterialSkinManager materialSkinManager;
        private Customer _customer;

        // Controls
        private MaterialTextBox txtFirstName;
        private MaterialTextBox txtLastName;
        private MaterialTextBox txtPhone;
        private MaterialTextBox txtAddress;
        private MaterialTextBox txtCity;
        private MaterialTextBox txtPostalCode;
        private MaterialComboBox cmbCountry;
        private MaterialTextBox txtEmail;
        private MaterialButton btnSave;
        private MaterialButton btnCancel;

        public CustomerProfileForm(Customer customer)
        {
            _customer = customer;
            _customerService = new CustomerService();
            
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue100, Accent.Orange200, TextShade.WHITE);
            
            InitializeComponent();
            LoadCustomerData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Customer Profile";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            var lblTitle = new MaterialLabel
            {
                Text = "Customer Profile",
                Location = new Point(30, 80),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H5,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtFirstName = new MaterialTextBox
            {
                Location = new Point(30, 130),
                Size = new Size(200, 50),
                Hint = "First Name",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtLastName = new MaterialTextBox
            {
                Location = new Point(250, 130),
                Size = new Size(200, 50),
                Hint = "Last Name",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtEmail = new MaterialTextBox
            {
                Location = new Point(30, 200),
                Size = new Size(420, 50),
                Hint = "Email",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                ReadOnly = true
            };

            txtPhone = new MaterialTextBox
            {
                Location = new Point(30, 270),
                Size = new Size(420, 50),
                Hint = "Phone Number",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtAddress = new MaterialTextBox
            {
                Location = new Point(30, 340),
                Size = new Size(420, 50),
                Hint = "Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtCity = new MaterialTextBox
            {
                Location = new Point(30, 410),
                Size = new Size(200, 50),
                Hint = "City",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPostalCode = new MaterialTextBox
            {
                Location = new Point(250, 410),
                Size = new Size(200, 50),
                Hint = "Postal Code",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbCountry = new MaterialComboBox
            {
                Location = new Point(30, 480),
                Size = new Size(200, 50),
                Hint = "Country",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbCountry.Items.AddRange(new object[] { "Sri Lanka", "United Kingdom", "United States", "Canada", "Australia", "Ireland" });

            btnSave = new MaterialButton
            {
                Location = new Point(270, 530),
                Size = new Size(80, 36),
                Text = "SAVE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSave.Click += btnSave_Click;

            btnCancel = new MaterialButton
            {
                Location = new Point(370, 530),
                Size = new Size(80, 36),
                Text = "CANCEL",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblTitle, txtFirstName, txtLastName, txtEmail, txtPhone,
                txtAddress, txtCity, txtPostalCode, cmbCountry,
                btnSave, btnCancel
            });

            this.ResumeLayout(false);
        }

        private void LoadCustomerData()
        {
            txtFirstName.Text = _customer.FirstName;
            txtLastName.Text = _customer.LastName;
            txtEmail.Text = _customer.User?.Email ?? "";
            txtPhone.Text = _customer.Phone;
            txtAddress.Text = _customer.Address;
            txtCity.Text = _customer.City;
            txtPostalCode.Text = _customer.PostalCode;
            
            if (!string.IsNullOrEmpty(_customer.Country))
            {
                for (int i = 0; i < cmbCountry.Items.Count; i++)
                {
                    if (cmbCountry.Items[i].ToString() == _customer.Country)
                    {
                        cmbCountry.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                _customer.FirstName = txtFirstName.Text.Trim();
                _customer.LastName = txtLastName.Text.Trim();
                _customer.Phone = txtPhone.Text.Trim();
                _customer.Address = txtAddress.Text.Trim();
                _customer.City = txtCity.Text.Trim();
                _customer.PostalCode = txtPostalCode.Text.Trim();
                _customer.Country = cmbCountry.SelectedItem?.ToString() ?? "";

                _customerService.UpdateCustomer(_customer);
                
                MaterialMessageBox.Show("Profile updated successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error updating profile: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MaterialMessageBox.Show("Please enter first and last name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}