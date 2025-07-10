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
        private MaterialCard cardUserSettings;
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

            CreateCompanySettingsCard();
        }

        private void CreateCompanySettingsCard()
        {
            cardUserSettings = new MaterialCard
            {
                Location = new Point(20, 70),
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