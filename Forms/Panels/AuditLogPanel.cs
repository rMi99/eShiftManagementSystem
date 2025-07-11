using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Business.Services;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using eShiftManagementSystem.Business.Interfaces;

namespace eShiftManagementSystem.Forms
{
    public partial class AuditLogPanel : UserControl
    {
        private readonly AuditLogService _auditLogService;
        private readonly IUserService _userService;

        private MaterialCard cardFilters;
        private DataGridView dgvAuditLogs;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private MaterialComboBox cmbUser;
        private MaterialComboBox cmbActionType;
        private MaterialButton btnFilter;
        private MaterialButton btnReset;

        public AuditLogPanel()
        {
            _auditLogService = new AuditLogService();
            _userService = new UserService();
            InitializeComponent();
            LoadUsers();
            LoadAuditLogs();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1000, 700);

            var lblTitle = new MaterialLabel
            {
                Text = "Audit Logs",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            CreateFilterCard();
            CreateLogList();
        }

        private void CreateFilterCard()
        {
            cardFilters = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(960, 120),
                Padding = new Padding(20)
            };

            dtpStartDate = new DateTimePicker { Location = new Point(20, 40), Size = new Size(150, 25), Format = DateTimePickerFormat.Short, Value = DateTime.Now.AddDays(-7) };
            dtpEndDate = new DateTimePicker { Location = new Point(180, 40), Size = new Size(150, 25), Format = DateTimePickerFormat.Short };
            cmbUser = new MaterialComboBox { Hint = "User", Location = new Point(340, 30), Size = new Size(200, 50) };
            cmbActionType = new MaterialComboBox { Hint = "Action Type", Location = new Point(550, 30), Size = new Size(150, 50) };
            btnFilter = new MaterialButton { Text = "Filter", Location = new Point(720, 35), Size = new Size(100, 36) };
            btnReset = new MaterialButton { Text = "Reset", Location = new Point(830, 35), Size = new Size(100, 36), Type = MaterialButton.MaterialButtonType.Outlined };

            cmbActionType.Items.AddRange(new[] { "All", "CREATE", "UPDATE", "DELETE", "LOGIN" });
            cmbActionType.SelectedIndex = 0;

            btnFilter.Click += (s, e) => LoadAuditLogs();
            btnReset.Click += (s, e) => ResetFilters();

            cardFilters.Controls.AddRange(new Control[] { dtpStartDate, dtpEndDate, cmbUser, cmbActionType, btnFilter, btnReset });
            this.Controls.Add(cardFilters);
        }

        private void CreateLogList()
        {
            dgvAuditLogs = new DataGridView
            {
                Location = new Point(20, 210),
                Size = new Size(960, 460),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false
            };
            this.Controls.Add(dgvAuditLogs);
        }

        private void LoadUsers()
        {
            var users = _userService.GetAllUsers();
            var userList = users.Select(u => new { Text = u.Username, Value = u.UserId }).ToList();
            userList.Insert(0, new { Text = "All Users", Value = 0 });
            cmbUser.DataSource = userList;
            cmbUser.DisplayMember = "Text";
            cmbUser.ValueMember = "Value";
        }

        private void LoadAuditLogs()
        {
            var startDate = dtpStartDate.Value;
            var endDate = dtpEndDate.Value;
            var userId = (int)cmbUser.SelectedValue == 0 ? (int?)null : (int)cmbUser.SelectedValue;
            var actionType = cmbActionType.SelectedItem.ToString() == "All" ? null : cmbActionType.SelectedItem.ToString();

            var logs = _auditLogService.GetLogsByCriteria(startDate, endDate, userId, actionType);
            dgvAuditLogs.DataSource = logs.Select(l => new
            {
                Timestamp = l.ActionTimestamp,
                User = l.User?.Username ?? "System",
                Action = l.Action,
                Table = l.TableAffected,
                RecordId = l.RecordId,
                l.IpAddress
            }).ToList();
        }

        private void ResetFilters()
        {
            dtpStartDate.Value = DateTime.Now.AddDays(-7);
            dtpEndDate.Value = DateTime.Now;
            cmbUser.SelectedIndex = 0;
            cmbActionType.SelectedIndex = 0;
            LoadAuditLogs();
        }
    }
}