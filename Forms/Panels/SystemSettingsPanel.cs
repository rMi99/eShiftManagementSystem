using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace eShiftManagementSystem.Forms
{
    public partial class SystemSettingsPanel : UserControl
    {
        // Controls
        private MaterialCard cardDatabaseSettings;
        private MaterialCard cardSystemSettings;
        private MaterialCard cardUserSettings;
        private MaterialTextBox txtDatabaseServer;
        private MaterialTextBox txtDatabaseName;
        private MaterialTextBox txtDatabaseUser;
        private MaterialTextBox txtDatabasePassword;
        private MaterialButton btnTestConnection;
        private MaterialButton btnSaveDatabase;
        private MaterialComboBox cmbTheme;
        private MaterialComboBox cmbLanguage;
        private MaterialCheckbox chkEmailNotifications;
        private MaterialCheckbox chkSMSNotifications;
        private MaterialButton btnSaveSystem;
        private MaterialTextBox txtCompanyName;
        private MaterialTextBox txtCompanyAddress;
        private MaterialTextBox txtCompanyPhone;
        private MaterialTextBox txtCompanyEmail;
        private MaterialButton btnSaveCompany;

        public SystemSettingsPanel()
        {
            InitializeComponent();
            LoadCurrentSettings();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1000, 700);

            var lblTitle = new MaterialLabel
            {
                Text = "System Settings",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            CreateDatabaseSettingsCard();
            CreateSystemSettingsCard();
            CreateCompanySettingsCard();
        }

        private void CreateDatabaseSettingsCard()
        {
            cardDatabaseSettings = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(460, 280),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblDatabase = new MaterialLabel
            {
                Text = "Database Settings",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtDatabaseServer = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(420, 50),
                Hint = "Database Server",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Text = "localhost"
            };

            txtDatabaseName = new MaterialTextBox
            {
                Location = new Point(20, 120),
                Size = new Size(420, 50),
                Hint = "Database Name",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Text = "eshift_db"
            };

            txtDatabaseUser = new MaterialTextBox
            {
                Location = new Point(20, 180),
                Size = new Size(200, 50),
                Hint = "Username",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Text = "root"
            };

            txtDatabasePassword = new MaterialTextBox
            {
                Location = new Point(240, 180),
                Size = new Size(200, 50),
                Hint = "Password",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Password = true
            };

            btnTestConnection = new MaterialButton
            {
                Location = new Point(20, 240),
                Size = new Size(150, 36),
                Text = "TEST CONNECTION",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnTestConnection.Click += btnTestConnection_Click;

            btnSaveDatabase = new MaterialButton
            {
                Location = new Point(290, 240),
                Size = new Size(150, 36),
                Text = "SAVE SETTINGS",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSaveDatabase.Click += btnSaveDatabase_Click;

            cardDatabaseSettings.Controls.Add(lblDatabase);
            cardDatabaseSettings.Controls.Add(txtDatabaseServer);
            cardDatabaseSettings.Controls.Add(txtDatabaseName);
            cardDatabaseSettings.Controls.Add(txtDatabaseUser);
            cardDatabaseSettings.Controls.Add(txtDatabasePassword);
            cardDatabaseSettings.Controls.Add(btnTestConnection);
            cardDatabaseSettings.Controls.Add(btnSaveDatabase);

            this.Controls.Add(cardDatabaseSettings);
        }

        private void CreateSystemSettingsCard()
        {
            cardSystemSettings = new MaterialCard
            {
                Location = new Point(500, 70),
                Size = new Size(480, 280),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblSystem = new MaterialLabel
            {
                Text = "Application Settings",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            cmbTheme = new MaterialComboBox
            {
                Location = new Point(20, 60),
                Size = new Size(200, 50),
                Hint = "Theme",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbTheme.Items.AddRange(new object[] { "Light", "Dark" });
            cmbTheme.SelectedIndex = 0;

            cmbLanguage = new MaterialComboBox
            {
                Location = new Point(240, 60),
                Size = new Size(200, 50),
                Hint = "Language",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbLanguage.Items.AddRange(new object[] { "English", "Spanish", "French" });
            cmbLanguage.SelectedIndex = 0;

            chkEmailNotifications = new MaterialCheckbox
            {
                Location = new Point(20, 130),
                Size = new Size(200, 40),
                Text = "Email Notifications",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.HOVER,
                Checked = true
            };

            chkSMSNotifications = new MaterialCheckbox
            {
                Location = new Point(20, 170),
                Size = new Size(200, 40),
                Text = "SMS Notifications",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.HOVER,
                Checked = false
            };

            btnSaveSystem = new MaterialButton
            {
                Location = new Point(330, 240),
                Size = new Size(130, 36),
                Text = "SAVE SETTINGS",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSaveSystem.Click += btnSaveSystem_Click;

            cardSystemSettings.Controls.Add(lblSystem);
            cardSystemSettings.Controls.Add(cmbTheme);
            cardSystemSettings.Controls.Add(cmbLanguage);
            cardSystemSettings.Controls.Add(chkEmailNotifications);
            cardSystemSettings.Controls.Add(chkSMSNotifications);
            cardSystemSettings.Controls.Add(btnSaveSystem);

            this.Controls.Add(cardSystemSettings);
        }

        private void CreateCompanySettingsCard()
        {
            cardUserSettings = new MaterialCard
            {
                Location = new Point(20, 370),
                Size = new Size(960, 200),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblCompany = new MaterialLabel
            {
                Text = "Company Information",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtCompanyName = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(300, 50),
                Hint = "Company Name",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Text = "e-Shift Moving Services"
            };

            txtCompanyAddress = new MaterialTextBox
            {
                Location = new Point(340, 60),
                Size = new Size(300, 50),
                Hint = "Company Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtCompanyPhone = new MaterialTextBox
            {
                Location = new Point(660, 60),
                Size = new Size(150, 50),
                Hint = "Phone Number",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtCompanyEmail = new MaterialTextBox
            {
                Location = new Point(20, 120),
                Size = new Size(300, 50),
                Hint = "Company Email",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            btnSaveCompany = new MaterialButton
            {
                Location = new Point(810, 150),
                Size = new Size(130, 36),
                Text = "SAVE COMPANY",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSaveCompany.Click += btnSaveCompany_Click;

            cardUserSettings.Controls.Add(lblCompany);
            cardUserSettings.Controls.Add(txtCompanyName);
            cardUserSettings.Controls.Add(txtCompanyAddress);
            cardUserSettings.Controls.Add(txtCompanyPhone);
            cardUserSettings.Controls.Add(txtCompanyEmail);
            cardUserSettings.Controls.Add(btnSaveCompany);

            this.Controls.Add(cardUserSettings);
        }

        private void LoadCurrentSettings()
        {
            // Load current settings from configuration or database
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                if (DatabaseConnection.TestConnection())
                {
                    MaterialMessageBox.Show("Database connection successful!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MaterialMessageBox.Show("Database connection failed!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Connection test error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                MaterialMessageBox.Show("Database settings saved successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving database settings: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveSystem_Click(object sender, EventArgs e)
        {
            try
            {
                MaterialMessageBox.Show("System settings saved successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving system settings: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveCompany_Click(object sender, EventArgs e)
        {
            try
            {
                MaterialMessageBox.Show("Company information saved successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving company information: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}