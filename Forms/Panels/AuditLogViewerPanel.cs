using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace eShiftManagementSystem.Forms.Panels
{
    public partial class AuditLogViewerPanel : UserControl
    {
        private readonly AuditService _auditService;
        private int _currentPage = 1;
        private const int PageSize = 50;

        // Controls
        private MaterialCard cardFilters;
        private MaterialCard cardAuditLogs;
        private DataGridView dgvAuditLogs;
        private MaterialComboBox cmbTableFilter;
        private MaterialComboBox cmbActionFilter;
        private MaterialComboBox cmbUserFilter;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private MaterialButton btnSearch;
        private MaterialButton btnRefresh;
        private MaterialButton btnClearFilters;
        private MaterialButton btnExport;
        private MaterialButton btnPrevious;
        private MaterialButton btnNext;
        private MaterialLabel lblPageInfo;
        private MaterialLabel lblTotalRecords;

        public AuditLogViewerPanel()
        {
            _auditService = new AuditService();
            InitializeComponent();
            LoadFilters();
            LoadAuditLogs();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1200, 800);

            var lblTitle = new MaterialLabel
            {
                Text = "Audit Log Viewer",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            CreateFiltersCard();
            CreateAuditLogsCard();
        }

        private void CreateFiltersCard()
        {
            cardFilters = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(1150, 120),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblFilters = new MaterialLabel
            {
                Text = "Filters",
                Location = new Point(20, 15),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            cmbTableFilter = new MaterialComboBox
            {
                Location = new Point(20, 50),
                Size = new Size(150, 50),
                Hint = "Table",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbActionFilter = new MaterialComboBox
            {
                Location = new Point(190, 50),
                Size = new Size(120, 50),
                Hint = "Action",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbActionFilter.Items.AddRange(new object[] { "All", "INSERT", "UPDATE", "DELETE", "LOGIN_SUCCESS", "LOGIN_FAILED", "STATUS_CHANGE" });
            cmbActionFilter.SelectedIndex = 0;

            cmbUserFilter = new MaterialComboBox
            {
                Location = new Point(330, 50),
                Size = new Size(150, 50),
                Hint = "User",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            var lblFromDate = new MaterialLabel
            {
                Text = "From:",
                Location = new Point(500, 35),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Body1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(500, 60),
                Size = new Size(120, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddDays(-30) // Default to last 30 days
            };

            var lblToDate = new MaterialLabel
            {
                Text = "To:",
                Location = new Point(640, 35),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Body1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            dtpToDate = new DateTimePicker
            {
                Location = new Point(640, 60),
                Size = new Size(120, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };

            btnSearch = new MaterialButton
            {
                Location = new Point(780, 50),
                Size = new Size(80, 40),
                Text = "SEARCH",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnSearch.Click += btnSearch_Click;

            btnRefresh = new MaterialButton
            {
                Location = new Point(870, 50),
                Size = new Size(80, 40),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnRefresh.Click += btnRefresh_Click;

            btnClearFilters = new MaterialButton
            {
                Location = new Point(960, 50),
                Size = new Size(80, 40),
                Text = "CLEAR",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnClearFilters.Click += btnClearFilters_Click;

            btnExport = new MaterialButton
            {
                Location = new Point(1050, 50),
                Size = new Size(80, 40),
                Text = "EXPORT",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = true,
                UseVisualStyleBackColor = true
            };
            btnExport.Click += btnExport_Click;

            cardFilters.Controls.AddRange(new Control[] {
                lblFilters, cmbTableFilter, cmbActionFilter, cmbUserFilter,
                lblFromDate, dtpFromDate, lblToDate, dtpToDate,
                btnSearch, btnRefresh, btnClearFilters, btnExport
            });

            this.Controls.Add(cardFilters);
        }

        private void CreateAuditLogsCard()
        {
            cardAuditLogs = new MaterialCard
            {
                Location = new Point(20, 210),
                Size = new Size(1150, 550),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblAuditLogs = new MaterialLabel
            {
                Text = "Audit Logs",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            lblTotalRecords = new MaterialLabel
            {
                Text = "Total Records: 0",
                Location = new Point(900, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Body1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            dgvAuditLogs = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(1100, 420),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                BackgroundColor = Color.White,
                GridColor = Color.FromArgb(230, 230, 230),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(222, 0, 0, 0),
                    SelectionBackColor = Color.FromArgb(63, 81, 181),
                    SelectionForeColor = Color.White,
                    WrapMode = DataGridViewTriState.False
                }
            };
            dgvAuditLogs.CellDoubleClick += dgvAuditLogs_CellDoubleClick;

            // Pagination controls
            btnPrevious = new MaterialButton
            {
                Location = new Point(20, 500),
                Size = new Size(100, 35),
                Text = "PREVIOUS",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                UseVisualStyleBackColor = true,
                Enabled = false
            };
            btnPrevious.Click += btnPrevious_Click;

            lblPageInfo = new MaterialLabel
            {
                Text = "Page 1",
                Location = new Point(130, 505),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.Body1,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            btnNext = new MaterialButton
            {
                Location = new Point(250, 500),
                Size = new Size(100, 35),
                Text = "NEXT",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                UseVisualStyleBackColor = true
            };
            btnNext.Click += btnNext_Click;

            cardAuditLogs.Controls.AddRange(new Control[] {
                lblAuditLogs, lblTotalRecords, dgvAuditLogs, btnPrevious, lblPageInfo, btnNext
            });

            this.Controls.Add(cardAuditLogs);
        }

        private void LoadFilters()
        {
            try
            {
                // Load table names
                var tables = _auditService.GetAuditedTables();
                cmbTableFilter.Items.Clear();
                cmbTableFilter.Items.Add("All");
                foreach (var table in tables)
                {
                    cmbTableFilter.Items.Add(table);
                }
                cmbTableFilter.SelectedIndex = 0;

                // Note: In a real implementation, you would load users from UserRepository
                cmbUserFilter.Items.Clear();
                cmbUserFilter.Items.Add("All");
                cmbUserFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading filters: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAuditLogs()
        {
            try
            {
                string? tableName = cmbTableFilter.SelectedIndex > 0 ? cmbTableFilter.SelectedItem?.ToString() : null;
                string? actionType = cmbActionFilter.SelectedIndex > 0 ? cmbActionFilter.SelectedItem?.ToString() : null;
                int? changedBy = null; // Would need to implement user filtering
                DateTime? fromDate = dtpFromDate.Value.Date;
                DateTime? toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1); // End of day

                var auditLogs = _auditService.GetAuditLogs(_currentPage, PageSize, tableName, actionType, changedBy, fromDate, toDate);
                var totalCount = _auditService.GetAuditLogsCount(tableName, actionType, changedBy, fromDate, toDate);

                var auditData = auditLogs.Select(a => new
                {
                    AuditId = a.AuditId,
                    DateTime = a.FormattedDate,
                    Table = a.TableDisplayName,
                    Action = a.DisplayAction,
                    RecordId = a.RecordId,
                    User = a.UserInfo,
                    IpAddress = a.ShortIpAddress
                }).ToList();

                dgvAuditLogs.DataSource = auditData;

                if (dgvAuditLogs.Columns.Count > 0)
                {
                    dgvAuditLogs.Columns["AuditId"].Visible = false;
                    dgvAuditLogs.Columns["DateTime"].HeaderText = "Date & Time";
                    dgvAuditLogs.Columns["Table"].HeaderText = "Table";
                    dgvAuditLogs.Columns["Action"].HeaderText = "Action";
                    dgvAuditLogs.Columns["RecordId"].HeaderText = "Record ID";
                    dgvAuditLogs.Columns["User"].HeaderText = "User";
                    dgvAuditLogs.Columns["IpAddress"].HeaderText = "IP Address";

                    // Set column widths
                    dgvAuditLogs.Columns["DateTime"].Width = 150;
                    dgvAuditLogs.Columns["Table"].Width = 120;
                    dgvAuditLogs.Columns["Action"].Width = 100;
                    dgvAuditLogs.Columns["RecordId"].Width = 80;
                    dgvAuditLogs.Columns["User"].Width = 150;
                    dgvAuditLogs.Columns["IpAddress"].Width = 120;
                }

                lblTotalRecords.Text = $"Total Records: {totalCount:N0}";
                lblPageInfo.Text = $"Page {_currentPage} of {Math.Ceiling((double)totalCount / PageSize):N0}";
                
                btnPrevious.Enabled = _currentPage > 1;
                btnNext.Enabled = auditLogs.Count == PageSize; // Assume more pages if we got full page
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error loading audit logs: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _currentPage = 1;
            LoadAuditLogs();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _currentPage = 1;
            LoadAuditLogs();
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cmbTableFilter.SelectedIndex = 0;
            cmbActionFilter.SelectedIndex = 0;
            cmbUserFilter.SelectedIndex = 0;
            dtpFromDate.Value = DateTime.Now.AddDays(-30);
            dtpToDate.Value = DateTime.Now;
            _currentPage = 1;
            LoadAuditLogs();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                MaterialMessageBox.Show("Export functionality would be implemented here to export audit logs to CSV or Excel format.", 
                    "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error exporting audit logs: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadAuditLogs();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _currentPage++;
            LoadAuditLogs();
        }

        private void dgvAuditLogs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dgvAuditLogs.Rows[e.RowIndex];
                var auditId = Convert.ToInt32(selectedRow.Cells["AuditId"].Value);
                
                // Show detailed audit log information
                ShowAuditLogDetails(auditId);
            }
        }

        private void ShowAuditLogDetails(int auditId)
        {
            try
            {
                // In a real implementation, you would create a detail form
                // For now, just show a message box
                MaterialMessageBox.Show($"Detailed audit log view for Audit ID: {auditId}\n\nThis would show:\n- Complete old and new values\n- Full user agent string\n- Complete IP address\n- Related changes", 
                    "Audit Log Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error showing audit log details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}