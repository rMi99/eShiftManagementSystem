using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace eShiftManagementSystem.Forms.Panels
{
    public partial class PriceRangeManagementPanel : UserControl
    {
        private readonly PriceRangeService _priceRangeService;
        private PriceRange _selectedPriceRange;

        private DataGridView dgvPriceRanges;
        private MaterialTextBox txtFromWeight;
        private MaterialTextBox txtToWeight;
        private MaterialTextBox txtPrice;
        private MaterialButton btnAdd;
        private MaterialButton btnUpdate;
        private MaterialButton btnDelete;
        private MaterialButton btnClear;

        public PriceRangeManagementPanel()
        {
            _priceRangeService = new PriceRangeService();
            InitializeComponent();
            LoadPriceRanges();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1000, 700);

            var lblTitle = new MaterialLabel
            {
                Text = "Transport Price Management",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4
            };
            this.Controls.Add(lblTitle);

            dgvPriceRanges = new DataGridView
            {
                Location = new Point(20, 80),
                Size = new Size(960, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvPriceRanges.SelectionChanged += dgvPriceRanges_SelectionChanged;
            this.Controls.Add(dgvPriceRanges);

            var cardInputs = new MaterialCard
            {
                Location = new Point(20, 400),
                Size = new Size(960, 150),
                Padding = new Padding(20)
            };
            this.Controls.Add(cardInputs);

            txtFromWeight = new MaterialTextBox { Hint = "From Weight (kg)", Location = new Point(20, 20), Size = new Size(150, 50) };
            txtToWeight = new MaterialTextBox { Hint = "To Weight (kg)", Location = new Point(190, 20), Size = new Size(150, 50) };
            txtPrice = new MaterialTextBox { Hint = "Price (LKR)", Location = new Point(360, 20), Size = new Size(150, 50) };

            btnAdd = new MaterialButton { Text = "ADD", Location = new Point(20, 90) };
            btnUpdate = new MaterialButton { Text = "UPDATE", Location = new Point(130, 90) };
            btnDelete = new MaterialButton { Text = "DELETE", Location = new Point(240, 90), Type = MaterialButton.MaterialButtonType.Outlined, UseAccentColor = true };
            btnClear = new MaterialButton { Text = "CLEAR", Location = new Point(350, 90), Type = MaterialButton.MaterialButtonType.Text };

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;

            cardInputs.Controls.AddRange(new Control[] { txtFromWeight, txtToWeight, txtPrice, btnAdd, btnUpdate, btnDelete, btnClear });
        }

        private void LoadPriceRanges()
        {
            dgvPriceRanges.DataSource = _priceRangeService.GetAllPriceRanges()
                .Select(p => new { p.Id, FromWeight = p.FromWeight.ToString("0.00"), ToWeight = p.ToWeight.ToString("0.00"), Price = p.Price.ToString("0.00") })
                .ToList();
            dgvPriceRanges.Columns["Id"].Visible = false;
        }

        private void dgvPriceRanges_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPriceRanges.SelectedRows.Count > 0)
            {
                var row = dgvPriceRanges.SelectedRows[0];
                _selectedPriceRange = new PriceRange
                {
                    Id = (int)row.Cells["Id"].Value,
                    FromWeight = decimal.Parse(row.Cells["FromWeight"].Value.ToString()),
                    ToWeight = decimal.Parse(row.Cells["ToWeight"].Value.ToString()),
                    Price = decimal.Parse(row.Cells["Price"].Value.ToString())
                };
                txtFromWeight.Text = _selectedPriceRange.FromWeight.ToString("0.00");
                txtToWeight.Text = _selectedPriceRange.ToWeight.ToString("0.00");
                txtPrice.Text = _selectedPriceRange.Price.ToString("0.00");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var newRange = new PriceRange
            {
                FromWeight = decimal.Parse(txtFromWeight.Text),
                ToWeight = decimal.Parse(txtToWeight.Text),
                Price = decimal.Parse(txtPrice.Text)
            };
            _priceRangeService.AddPriceRange(newRange);
            LoadPriceRanges();
            ClearForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedPriceRange != null)
            {
                _selectedPriceRange.FromWeight = decimal.Parse(txtFromWeight.Text);
                _selectedPriceRange.ToWeight = decimal.Parse(txtToWeight.Text);
                _selectedPriceRange.Price = decimal.Parse(txtPrice.Text);
                _priceRangeService.UpdatePriceRange(_selectedPriceRange);
                LoadPriceRanges();
                ClearForm();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedPriceRange != null)
            {
                _priceRangeService.DeletePriceRange(_selectedPriceRange.Id);
                LoadPriceRanges();
                ClearForm();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _selectedPriceRange = null;
            txtFromWeight.Clear();
            txtToWeight.Clear();
            txtPrice.Clear();
            dgvPriceRanges.ClearSelection();
        }
    }
}