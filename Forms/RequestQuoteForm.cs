using MaterialSkin;
using MaterialSkin.Controls;
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
        private readonly int _customerId;

        // Form Controls
        private MaterialTextBox txtPickupAddress, txtPickupCity, txtPickupPostalCode;
        private MaterialTextBox txtDestinationAddress, txtDestinationCity, txtDestinationPostalCode;
        private MaterialTextBox txtEstimatedWeight, txtEstimatedVolume, txtSpecialRequirements;
        private MaterialButton btnSubmit, btnCancel;

        public RequestQuoteForm(int customerId)
        {
            _customerId = customerId;
            _quoteRepository = new QuoteRepository();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Purple600, Primary.Purple700, Primary.Purple100, Accent.Orange200, TextShade.WHITE);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Request Quote";
            this.Size = new Size(600, 550);
            this.StartPosition = FormStartPosition.CenterParent;

            // Initialize and position all your controls here...
            txtPickupAddress = new MaterialTextBox { Hint = "Pickup Address", Location = new Point(30, 80), Size = new Size(540, 50) };
            txtDestinationAddress = new MaterialTextBox { Hint = "Destination Address", Location = new Point(30, 140), Size = new Size(540, 50) };
            txtEstimatedWeight = new MaterialTextBox { Hint = "Estimated Weight (kg)", Location = new Point(30, 200), Size = new Size(260, 50) };
            txtEstimatedVolume = new MaterialTextBox { Hint = "Estimated Volume (m³)", Location = new Point(310, 200), Size = new Size(260, 50) };
            txtSpecialRequirements = new MaterialTextBox { Hint = "Special Requirements", Location = new Point(30, 260), Size = new Size(540, 80), Multiline = true };
            
            btnSubmit = new MaterialButton { Text = "SUBMIT", Location = new Point(360, 450), Size = new Size(100, 36), Type = MaterialButton.MaterialButtonType.Contained };
            btnCancel = new MaterialButton { Text = "CANCEL", Location = new Point(470, 450), Size = new Size(100, 36), Type = MaterialButton.MaterialButtonType.Text };

            btnSubmit.Click += btnSubmit_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                txtPickupAddress, txtDestinationAddress, txtEstimatedWeight,
                txtEstimatedVolume, txtSpecialRequirements, btnSubmit, btnCancel
            });
            this.ResumeLayout(false);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var quote = new Quote
            {
                CustomerId = _customerId,
                PickupAddress = txtPickupAddress.Text,
                DestinationAddress = txtDestinationAddress.Text,
                EstimatedWeight = decimal.TryParse(txtEstimatedWeight.Text, out var w) ? w : 0,
                EstimatedVolume = decimal.TryParse(txtEstimatedVolume.Text, out var v) ? v : 0,
                SpecialRequirements = txtSpecialRequirements.Text,
                QuoteAmount = 0, // To be set by an admin later
                ValidUntil = DateTime.Now.AddDays(30),
                Status = "pending",
                CreatedAt = DateTime.Now,
                // These fields might be empty if you don't have controls for them
                PickupCity = "", 
                PickupPostalCode = "",
                DestinationCity = "",
                DestinationPostalCode = ""
            };

            try
            {
                int newQuoteId = _quoteRepository.AddQuote(quote);
                MaterialMessageBox.Show($"Quote request (ID: {newQuoteId}) submitted successfully.", "Success");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }
    }
}