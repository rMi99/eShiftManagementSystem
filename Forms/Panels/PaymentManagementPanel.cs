using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace eShiftManagementSystem.Forms.Panels
{
    public partial class PaymentManagementPanel : UserControl
    {
        private readonly PaymentService _paymentService = new PaymentService();
        private readonly JobService _jobService = new JobService();
        private Job _selectedJob;
        private Payment _selectedPayment;

        // --- UI Controls ---
        private DataGridView dgvJobs, dgvPayments;
        private MaterialTextBox txtAmount, txtTransactionId;
        private MaterialComboBox cmbPaymentMethod, cmbPaymentStatus;
        private DateTimePicker dtpPaymentDate;
        private MaterialButton btnAddPayment, btnUpdatePayment, btnDeletePayment, btnClearForm;
        private MaterialLabel lblJobInfo, lblTotalPaid;

        public PaymentManagementPanel()
        {
            InitializeComponent();
            LoadJobs();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.SuspendLayout();

            var lblTitle = new MaterialLabel { Text = "Payment Management", Location = new Point(20, 20), AutoSize = true, FontType = MaterialSkinManager.fontType.H4 };
            this.Controls.Add(lblTitle);

            // --- Jobs List ---
            var cardJobs = new MaterialCard { Location = new Point(20, 80), Size = new Size(960, 250), Padding = new Padding(10) };
            var lblJobs = new MaterialLabel { Text = "Select a Job", Location = new Point(10, 10), FontType = MaterialSkinManager.fontType.H6 };
            dgvJobs = new DataGridView { Location = new Point(10, 40), Size = new Size(940, 200), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, ReadOnly = true, RowHeadersVisible = false };
            dgvJobs.SelectionChanged += DgvJobs_SelectionChanged;
            cardJobs.Controls.AddRange(new Control[] { lblJobs, dgvJobs });
            this.Controls.Add(cardJobs);

            // --- Payments Section ---
            var cardPayments = new MaterialCard { Location = new Point(20, 340), Size = new Size(550, 340), Padding = new Padding(10) };
            var lblPayments = new MaterialLabel { Text = "Payments for Selected Job", Location = new Point(10, 10), FontType = MaterialSkinManager.fontType.H6 };
            lblJobInfo = new MaterialLabel { Text = "Job: -", Location = new Point(10, 40), FontType = MaterialSkinManager.fontType.Subtitle1, AutoSize = true };
            lblTotalPaid = new MaterialLabel { Text = "Total Paid: LKR 0.00", Location = new Point(350, 40), FontType = MaterialSkinManager.fontType.Subtitle1, AutoSize = true };
            dgvPayments = new DataGridView { Location = new Point(10, 70), Size = new Size(530, 260), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, ReadOnly = true, RowHeadersVisible = false };
            dgvPayments.SelectionChanged += DgvPayments_SelectionChanged;
            cardPayments.Controls.AddRange(new Control[] { lblPayments, lblJobInfo, lblTotalPaid, dgvPayments });
            this.Controls.Add(cardPayments);

            // --- Add/Edit Payment Form ---
            var cardForm = new MaterialCard { Location = new Point(580, 340), Size = new Size(400, 340), Padding = new Padding(10) };
            var lblFormTitle = new MaterialLabel { Text = "Add/Edit Payment", Location = new Point(10, 10), FontType = MaterialSkinManager.fontType.H6 };
            txtAmount = new MaterialTextBox { Hint = "Amount (LKR)", Location = new Point(10, 50), Size = new Size(180, 50) };
            dtpPaymentDate = new DateTimePicker { Location = new Point(200, 60), Size = new Size(180, 25) };
            cmbPaymentMethod = new MaterialComboBox { Hint = "Method", Location = new Point(10, 110), Size = new Size(180, 50) };
            cmbPaymentMethod.Items.AddRange(new[] { "Credit Card", "Bank Transfer", "Cash" });
            cmbPaymentStatus = new MaterialComboBox { Hint = "Status", Location = new Point(200, 110), Size = new Size(180, 50) };
            cmbPaymentStatus.Items.AddRange(new[] { "Paid", "Unpaid", "Advance" });
            txtTransactionId = new MaterialTextBox { Hint = "Transaction ID (Optional)", Location = new Point(10, 170), Size = new Size(370, 50) };

            btnAddPayment = new MaterialButton { Text = "ADD PAYMENT", Location = new Point(10, 240), Type = MaterialButton.MaterialButtonType.Contained };
            btnUpdatePayment = new MaterialButton { Text = "UPDATE", Location = new Point(140, 240) };
            btnDeletePayment = new MaterialButton { Text = "DELETE", Location = new Point(245, 240), Type = MaterialButton.MaterialButtonType.Outlined, UseAccentColor = true };
            btnClearForm = new MaterialButton { Text = "CLEAR", Location = new Point(10, 285), Type = MaterialButton.MaterialButtonType.Text };

            btnAddPayment.Click += BtnAddPayment_Click;
            btnUpdatePayment.Click += BtnUpdatePayment_Click;
            btnDeletePayment.Click += BtnDeletePayment_Click;
            btnClearForm.Click += (s, e) => ClearForm();

            cardForm.Controls.AddRange(new Control[] { lblFormTitle, txtAmount, dtpPaymentDate, cmbPaymentMethod, cmbPaymentStatus, txtTransactionId, btnAddPayment, btnUpdatePayment, btnDeletePayment, btnClearForm });
            this.Controls.Add(cardForm);

            this.ResumeLayout(false);
        }

        private void LoadJobs()
        {
            dgvJobs.DataSource = _jobService.GetAllJobs()
                .Select(j => new { j.JobId, j.JobNumber, Customer = j.Customer?.FullName, j.Status, j.RequestedPickupDate })
                .ToList();
        }

        private void DgvJobs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvJobs.SelectedRows.Count > 0)
            {
                int jobId = (int)dgvJobs.SelectedRows[0].Cells["JobId"].Value;
                _selectedJob = _jobService.GetJobById(jobId);
                if (_selectedJob != null)
                {
                    lblJobInfo.Text = $"Job: {_selectedJob.JobNumber}";
                    LoadPaymentsForJob();
                    ClearForm();
                }
            }
        }

        private void LoadPaymentsForJob()
        {
            if (_selectedJob == null) return;
            var payments = _paymentService.GetPaymentsByJobId(_selectedJob.JobId);
            dgvPayments.DataSource = payments.Select(p => new { p.PaymentId, p.Amount, p.PaymentDate, p.PaymentMethod, p.PaymentStatus }).ToList();
            decimal totalPaid = _paymentService.GetTotalPaymentsForJob(_selectedJob.JobId);
            lblTotalPaid.Text = $"Total Paid: LKR {totalPaid:N2}";
        }

        private void DgvPayments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPayments.SelectedRows.Count > 0)
            {
                int paymentId = (int)dgvPayments.SelectedRows[0].Cells["PaymentId"].Value;
                _selectedPayment = _paymentService.GetPaymentsByJobId(_selectedJob.JobId).FirstOrDefault(p => p.PaymentId == paymentId);
                if (_selectedPayment != null)
                {
                    txtAmount.Text = _selectedPayment.Amount.ToString("F2");
                    dtpPaymentDate.Value = _selectedPayment.PaymentDate;
                    cmbPaymentMethod.SelectedItem = _selectedPayment.PaymentMethod;
                    cmbPaymentStatus.SelectedItem = _selectedPayment.PaymentStatus;
                    txtTransactionId.Text = _selectedPayment.TransactionId;
                }
            }
        }

        private void BtnAddPayment_Click(object sender, EventArgs e)
        {
            if (_selectedJob == null)
            {
                MaterialMessageBox.Show("Please select a job first.", "Error");
                return;
            }
            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MaterialMessageBox.Show("Please enter a valid amount.", "Error");
                return;
            }

            var newPayment = new Payment
            {
                JobId = _selectedJob.JobId,
                Amount = amount,
                PaymentDate = dtpPaymentDate.Value,
                PaymentMethod = cmbPaymentMethod.SelectedItem?.ToString(),
                PaymentStatus = cmbPaymentStatus.SelectedItem?.ToString(),
                TransactionId = txtTransactionId.Text
            };
            _paymentService.AddPayment(newPayment);
            LoadPaymentsForJob();
            ClearForm();
        }

        private void BtnUpdatePayment_Click(object sender, EventArgs e)
        {
            if (_selectedPayment == null)
            {
                MaterialMessageBox.Show("Please select a payment to update.", "Error");
                return;
            }
            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MaterialMessageBox.Show("Please enter a valid amount.", "Error");
                return;
            }
            _selectedPayment.Amount = amount;
            _selectedPayment.PaymentDate = dtpPaymentDate.Value;
            _selectedPayment.PaymentMethod = cmbPaymentMethod.SelectedItem?.ToString();
            _selectedPayment.PaymentStatus = cmbPaymentStatus.SelectedItem?.ToString();
            _selectedPayment.TransactionId = txtTransactionId.Text;

            _paymentService.UpdatePayment(_selectedPayment, SessionManager.GetUserId());
            LoadPaymentsForJob();
            ClearForm();
        }

        private void BtnDeletePayment_Click(object sender, EventArgs e)
        {
            if (_selectedPayment == null)
            {
                MaterialMessageBox.Show("Please select a payment to delete.", "Error");
                return;
            }
            //var result = MaterialMessageBox.Show("Are you sure you want to delete this payment?", "Confirm Delete", MessageBoxButtons.YesNo);
            var result = MaterialMessageBox.Show(
     "Are you sure you want to delete this payment?",
     "Confirm Delete",
     MessageBoxButtons.YesNo,
     true
 );
            if (result == DialogResult.Yes)
            {
                _paymentService.DeletePayment(_selectedPayment.PaymentId, SessionManager.GetUserId());
                LoadPaymentsForJob();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            _selectedPayment = null;
            txtAmount.Clear();
            txtTransactionId.Clear();
            cmbPaymentMethod.SelectedIndex = -1;
            cmbPaymentStatus.SelectedIndex = -1;
            dtpPaymentDate.Value = DateTime.Now;
            dgvPayments.ClearSelection();
        }
    }
}
