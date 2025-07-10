using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace eShiftManagementSystem.Forms
{
    public partial class CustomerRegistrationForm : MaterialForm
    {
        private readonly IAuthenticationService _authService;
        private MaterialTextBox txtUsername;
        private MaterialTextBox txtEmail;
        private MaterialTextBox txtPassword;
        private MaterialTextBox txtConfirmPassword;
        private MaterialTextBox txtFirstName;
        private MaterialTextBox txtLastName;
        private MaterialTextBox txtPhone;
        private MaterialTextBox txtAddress;
        private MaterialTextBox txtCity;
        private MaterialTextBox txtPostalCode;
        private MaterialComboBox cmbCountry;
        private DateTimePicker dtpRegistrationDate;
        private MaterialButton btnRegister;
        private MaterialButton btnCancel;

        public CustomerRegistrationForm(IAuthenticationService authService)
        {
            _authService = authService;
            InitializeComponent();

            // Initialize Material Design
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        // Parameterless constructor for cases where you need to create the service internally
        public CustomerRegistrationForm()
        {
            // Create the authentication service with its dependencies
            var userRepo = new UserRepository();
            var customerRepo = new CustomerRepository();
            _authService = new AuthenticationService(userRepo, customerRepo);
            InitializeComponent();

            // Initialize Material Design
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void InitializeComponent()
        {
            this.Text = "Customer Registration";
            this.Size = new Size(500, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.Sizable = false;

            // Create title label
            var lblTitle = new MaterialLabel
            {
                Text = "",
                Location = new Point(50, 30),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H5,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            // Create controls
            txtUsername = new MaterialTextBox
            {
                Location = new Point(50, 80),
                Size = new Size(400, 50),
                Hint = "Username",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtEmail = new MaterialTextBox
            {
                Location = new Point(50, 140),
                Size = new Size(400, 50),
                Hint = "Email",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPassword = new MaterialTextBox
            {
                Location = new Point(50, 200),
                Size = new Size(400, 50),
                Hint = "Password",
                Password = true,
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtConfirmPassword = new MaterialTextBox
            {
                Location = new Point(50, 260),
                Size = new Size(400, 50),
                Hint = "Confirm Password",
                Password = true,
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtFirstName = new MaterialTextBox
            {
                Location = new Point(50, 320),
                Size = new Size(195, 50),
                Hint = "First Name",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtLastName = new MaterialTextBox
            {
                Location = new Point(255, 320),
                Size = new Size(195, 50),
                Hint = "Last Name",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPhone = new MaterialTextBox
            {
                Location = new Point(50, 380),
                Size = new Size(400, 50),
                Hint = "Phone Number",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtAddress = new MaterialTextBox
            {
                Location = new Point(50, 440),
                Size = new Size(400, 50),
                Hint = "Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtCity = new MaterialTextBox
            {
                Location = new Point(50, 500),
                Size = new Size(195, 50),
                Hint = "City",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPostalCode = new MaterialTextBox
            {
                Location = new Point(255, 500),
                Size = new Size(195, 50),
                Hint = "Postal Code",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbCountry = new MaterialComboBox
            {
                Location = new Point(50, 560),
                Size = new Size(195, 50),
                Hint = "Country",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbCountry.Items.AddRange(new object[] { "Sri Lanka", "United Kingdom", "United States", "Canada", "Australia", "Ireland" });
            cmbCountry.SelectedIndex = 0;

            // Use regular DateTimePicker
            dtpRegistrationDate = new DateTimePicker
            {
                Location = new Point(255, 560),
                Size = new Size(195, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };

            btnRegister = new MaterialButton
            {
                Location = new Point(270, 620),
                Size = new Size(100, 36),
                Text = "REGISTER",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };

            btnCancel = new MaterialButton
            {
                Location = new Point(380, 620),
                Size = new Size(70, 36),
                Text = "CANCEL",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                HighEmphasis = false
            };

            btnRegister.Click += btnRegister_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblTitle,
                txtUsername, txtEmail, txtPassword, txtConfirmPassword,
                txtFirstName, txtLastName, txtPhone, txtAddress,
                txtCity, txtPostalCode, cmbCountry, dtpRegistrationDate,
                btnRegister, btnCancel
            });
        }

    private void btnRegister_Click(object sender, EventArgs e)
{
    // Basic required field validation
    if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
        string.IsNullOrWhiteSpace(txtEmail.Text) ||
        string.IsNullOrWhiteSpace(txtPassword.Text) ||
        string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
        string.IsNullOrWhiteSpace(txtFirstName.Text) ||
        string.IsNullOrWhiteSpace(txtLastName.Text) ||
        string.IsNullOrWhiteSpace(txtPhone.Text))
    {
        MaterialMessageBox.Show("Please fill in all required fields.", "Validation Error",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    // Email validation
    if (!ValidationUtils.IsValidEmail(txtEmail.Text))
    {
        MaterialMessageBox.Show("Please enter a valid email address.", "Validation Error",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    // Phone number validation
    if (!ValidationUtils.IsValidPhoneNumber(txtPhone.Text))
    {
        MaterialMessageBox.Show("Phone number must contain 10–15 digits.", "Validation Error",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    // Password confirmation
    if (txtPassword.Text != txtConfirmPassword.Text)
    {
        MaterialMessageBox.Show("Passwords do not match.", "Validation Error",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    // Strong password validation
    var (isStrong, errorMessage) = ValidationUtils.IsStrongPassword(txtPassword.Text);
    if (!isStrong)
    {
        MaterialMessageBox.Show(errorMessage, "Password Error",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    var customer = new Customer
    {
        FirstName = txtFirstName.Text.Trim(),
        LastName = txtLastName.Text.Trim(),
        Phone = txtPhone.Text.Trim(),
        Address = txtAddress.Text.Trim(),
        City = txtCity.Text.Trim(),
        PostalCode = txtPostalCode.Text.Trim(),
        Country = cmbCountry.SelectedItem?.ToString() ?? "United Kingdom",
        RegistrationDate = dtpRegistrationDate.Value.Date
    };

    try
    {
        if (_authService.RegisterCustomer(txtUsername.Text.Trim(), txtEmail.Text.Trim(), txtPassword.Text, customer))
        {
            MaterialMessageBox.Show("Registration successful! You can now login.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else
        {
            MaterialMessageBox.Show("Registration failed. Username or email may already exist.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    catch (Exception ex)
    {
        MaterialMessageBox.Show($"Registration error: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

    }
}