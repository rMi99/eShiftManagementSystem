using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace eShiftManagementSystem.Forms
{
    public partial class ExportChoiceForm : MaterialForm
    {
        public enum ExportFormat
        {
            None,
            PDF,
            Excel
        }

        public ExportFormat SelectedFormat { get; private set; } = ExportFormat.None;

        public ExportChoiceForm()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Choose Export Format";
            this.Size = new Size(400, 220);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Sizable = false;

            var lblTitle = new MaterialLabel
            {
                Text = "Which format would you like to export?",
                Location = new Point(25, 80),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            var btnPdf = new MaterialButton
            {
                Text = "📄 PDF",
                Location = new Point(40, 140),
                Size = new Size(140, 40),
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true,
                Font = new Font("Segoe UI Emoji", 12F)
            };
            btnPdf.Click += (s, e) => {
                this.SelectedFormat = ExportFormat.PDF;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            var btnExcel = new MaterialButton
            {
                Text = "📈 Excel",
                Location = new Point(220, 140),
                Size = new Size(140, 40),
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = true,
                HighEmphasis = true,
                Font = new Font("Segoe UI Emoji", 12F)
            };
            btnExcel.Click += (s, e) => {
                this.SelectedFormat = ExportFormat.Excel;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnPdf);
            this.Controls.Add(btnExcel);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}