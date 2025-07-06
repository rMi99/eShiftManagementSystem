using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace eShiftManagementSystem.Forms
{
    public partial class RequestQuoteForm : MaterialForm
    {
        private readonly QuoteRepository _quoteRepository;
        private readonly MaterialSkinManager materialSkinManager;
        private int _customerId;

        // Controls
        private MaterialTextBox txtDescription;
        private MaterialTextBox txtPickupAddress;
        private MaterialTextBox txtDestinationAddress;
        private DateTimePicker dtpRequestedDate;
        private MaterialTextBox txtSpecialRequirements;
        private MaterialButton btnSubmit;
        private MaterialButton btnCancel;

        public RequestQuoteForm(int customerId)
        {
            _customerId = customerId;
            _quoteRepository = new QuoteRepository();
            
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Purple600, Primary.Purple700, Primary.Purple100, Accent.Orange200, TextShade.WHITE);
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Request Quote";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            var lblTitle = new MaterialLabel
            {
                Text = "Request Quote",
                Location = new Point(30, 80),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H5,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            txtDescription = new MaterialTextBox
            {
                Location = new Point(30, 130),
                Size = new Size(540, 80),
                Hint = "Description of items to move",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Multiline = true
            };

            txtPickupAddress = new MaterialTextBox
            {
                Location = new Point(30, 230),
                Size = new Size(540, 50),
                Hint = "Pickup Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtDestinationAddress = new MaterialTextBox
            {
                Location = new Point(30, 300),
                Size = new Size(540, 50),
                Hint = "Destination Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            var lblRequestedDate = new Label
            {
                Text = "Preferred Date:",
                Location = new Point(30, 360),
                AutoSize = true,
                Font = new Font("Roboto", 10),
                ForeColor = Color.Gray
            };

            dtpRequestedDate = new DateTimePicker
            {
                Location = new Point(30, 380),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddDays(7)
            };

            txtSpecialRequirements = new MaterialTextBox
            {
                Location = new Point(30, 420),
                Size = new Size(540, 80),
                Hint = "Special requirements or instructions",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Multiline = true
            };

            btnSubmit = new MaterialButton
            {
                Location = new Point(400, 520),
                Size = new Size(80, 36),
                Text = "SUBMIT",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSubmit.Click += btnSubmit_Click;

            btnCancel = new MaterialButton
            {
                Location = new Point(490, 520),
                Size = new Size(80, 36),
                Text = "CANCEL",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblTitle, txtDescription, txtPickupAddress, txtDestinationAddress,
                lblRequestedDate, dtpRequestedDate, txtSpecialRequirements,
                btnSubmit, btnCancel
            });

            this.ResumeLayout(false);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                var quote = new Quote
                {
                    CustomerId = _customerId,
                    QuoteNumber = GenerateQuoteNumber(),
                    Description = $"{txtDescription.Text.Trim()}\n\nPickup: {txtPickupAddress.Text.Trim()}\nDestination: {txtDestinationAddress.Text.Trim()}\nSpecial Requirements: {txtSpecialRequirements.Text.Trim()}",
                    EstimatedCost = 0, // Will be filled by admin
                    ValidUntil = DateTime.Now.AddDays(30),
                    Status = "pending",
                    CreatedAt = DateTime.Now
                };

                var quoteId = _quoteRepository.AddQuote(quote);
                
                MaterialMessageBox.Show($"Quote request submitted successfully! Quote ID: {quoteId}\nYou will receive an estimate within 24 hours.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error submitting quote request: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MaterialMessageBox.Show("Please enter a description of items to move.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPickupAddress.Text) || string.IsNullOrWhiteSpace(txtDestinationAddress.Text))
            {
                MaterialMessageBox.Show("Please enter pickup and destination addresses.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private string GenerateQuoteNumber()
        {
            return $"QT{DateTime.Now:yyyyMMdd}{DateTime.Now.Millisecond:000}";
        }
    }
}