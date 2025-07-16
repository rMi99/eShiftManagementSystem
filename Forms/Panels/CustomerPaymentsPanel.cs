// eShiftManagementSystem/Forms/Panels/CustomerPaymentsPanel.cs

using eShiftManagementSystem.Models;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace eShiftManagementSystem.Forms.Panels
{
    public partial class CustomerPaymentsPanel : UserControl
    {
        private readonly int _customerId;

        public CustomerPaymentsPanel(int customerId)
        {
            _customerId = customerId;
            InitializeComponent();
            LoadCustomerPayments();
        }

        private void InitializeComponent()
        {
            // Initialize a DataGridView to display payment records in a view-only mode.
        }

        private void LoadCustomerPayments()
        {
            // Implement the logic to load and display payment records for the given customerId.
        }
    }
}