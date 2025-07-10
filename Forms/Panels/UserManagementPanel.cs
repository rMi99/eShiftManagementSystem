using MaterialSkin;
using MaterialSkin.Controls;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace eShiftManagementSystem.Forms
{
    public partial class UserManagementPanel : UserControl
    {
        private readonly UserRepository _userRepository;
        private User? _selectedUser;

        // Controls
        private MaterialCard cardUserList;
        private MaterialCard cardUserDetails;
        private DataGridView dgvUsers;
        private MaterialTextBox txtSearch;
        private MaterialComboBox cmbRoleFilter;
        private MaterialButton btnSearch;
        private MaterialButton btnRefresh;
        private MaterialButton btnAddNew;
        private MaterialTextBox txtUsername;
        private MaterialTextBox txtEmail;
        private MaterialTextBox txtPassword;
        private MaterialComboBox cmbRole;
        private MaterialCheckbox chkIsActive;
        private MaterialButton btnSave;
        private MaterialButton btnEdit;
        private MaterialButton btnResetPassword;
        private MaterialButton btnDelete;
        private MaterialLabel lblTitle;
        private MaterialButton btnClear;

        public UserManagementPanel()
        {
            _userRepository = new UserRepository();
            InitializeComponent();
            // LoadUsers() is now called at the end of InitializeComponent()
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Size = new Size(1000, 700);
            this.Name = "UserManagementPanel";

            SuspendLayout();

            // Create title label
            lblTitle = new MaterialLabel
            {
                Text = "User Management",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H4,
                MouseState = MaterialSkin.MouseState.HOVER
            };
            this.Controls.Add(lblTitle);

            // Create the cards and their controls
            CreateUserListCard();
            CreateUserDetailsCard();

            this.Load += UserManagementPanel_Load;
            ResumeLayout(false);

            // Now that all components are created, load the data
            LoadUsers();
        }

        private void CreateUserListCard()
        {
            cardUserList = new MaterialCard
            {
                Location = new Point(20, 70),
                Size = new Size(580, 600),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            // Search and filter section
            txtSearch = new MaterialTextBox
            {
                Location = new Point(20, 20),
                Size = new Size(200, 50),
                Hint = "Search users...",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            cmbRoleFilter = new MaterialComboBox
            {
                Location = new Point(230, 20),
                Size = new Size(120, 50),
                Hint = "Filter by Role",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbRoleFilter.Items.AddRange(new object[] { "All", "admin", "customer", "operations_manager", "driver", "customer_service", "finance" });
            cmbRoleFilter.SelectedIndex = 0;

            btnSearch = new MaterialButton
            {
                Location = new Point(360, 30),
                Size = new Size(80, 30),
                Text = "SEARCH",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSearch.Click += btnSearch_Click;

            btnRefresh = new MaterialButton
            {
                Location = new Point(450, 30),
                Size = new Size(80, 30),
                Text = "REFRESH",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnRefresh.Click += btnRefresh_Click;

            btnAddNew = new MaterialButton
            {
                Location = new Point(20, 80),
                Size = new Size(100, 30),
                Text = "ADD NEW",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnAddNew.Click += btnAddNew_Click;

            // Users grid
            dgvUsers = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(540, 450),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvUsers.SelectionChanged += dgvUsers_SelectionChanged;

            cardUserList.Controls.Add(txtSearch);
            cardUserList.Controls.Add(cmbRoleFilter);
            cardUserList.Controls.Add(btnSearch);
            cardUserList.Controls.Add(btnRefresh);
            cardUserList.Controls.Add(btnAddNew);
            cardUserList.Controls.Add(dgvUsers);
            this.Controls.Add(cardUserList);
        }

        private void CreateUserDetailsCard()
        {
            cardUserDetails = new MaterialCard
            {
                Location = new Point(620, 70),
                Size = new Size(350, 600),
                BackColor = Color.White,
                Depth = 0,
                ForeColor = Color.FromArgb(222, 0, 0, 0),
                MouseState = MaterialSkin.MouseState.HOVER,
                Padding = new Padding(20)
            };

            var lblDetails = new MaterialLabel
            {
                Text = "User Details",
                Location = new Point(20, 20),
                AutoSize = true,
                Depth = 0,
                FontType = MaterialSkinManager.fontType.H6,
                MouseState = MaterialSkin.MouseState.HOVER
            };

            // Form fields
            txtUsername = new MaterialTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(310, 50),
                Hint = "Username",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtEmail = new MaterialTextBox
            {
                Location = new Point(20, 120),
                Size = new Size(310, 50),
                Hint = "Email Address",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };

            txtPassword = new MaterialTextBox
            {
                Location = new Point(20, 180),
                Size = new Size(310, 50),
                Hint = "Password (for new users)",
                BorderStyle = BorderStyle.None,
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT,
                Password = true
            };

            cmbRole = new MaterialComboBox
            {
                Location = new Point(20, 240),
                Size = new Size(200, 50),
                Hint = "Role",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.OUT
            };
            cmbRole.Items.AddRange(new object[] { "admin", "customer", "operations_manager", "driver", "customer_service", "finance" });

            chkIsActive = new MaterialCheckbox
            {
                Location = new Point(20, 300),
                Size = new Size(150, 40),
                Text = "Active User",
                Depth = 0,
                MouseState = MaterialSkin.MouseState.HOVER,
                Checked = true
            };

            // Action buttons
            btnSave = new MaterialButton
            {
                Location = new Point(20, 360),
                Size = new Size(70, 36),
                Text = "SAVE",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnSave.Click += btnSave_Click;

            btnEdit = new MaterialButton
            {
                Location = new Point(100, 360),
                Size = new Size(70, 36),
                Text = "EDIT",
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false,
                HighEmphasis = true
            };
            btnEdit.Click += btnEdit_Click;

            btnResetPassword = new MaterialButton
            {
                Location = new Point(20, 410),
                Size = new Size(120, 36),
                Text = "RESET PASSWORD",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnResetPassword.Click += btnResetPassword_Click;

            btnDelete = new MaterialButton
            {
                Location = new Point(180, 360),
                Size = new Size(70, 36),
                Text = "DELETE",
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = true,
                HighEmphasis = false
            };
            btnDelete.Click += btnDelete_Click;

            btnClear = new MaterialButton
            {
                Location = new Point(260, 360),
                Size = new Size(70, 36),
                Text = "CLEAR",
                Type = MaterialButton.MaterialButtonType.Text,
                UseAccentColor = false,
                HighEmphasis = false
            };
            btnClear.Click += btnClear_Click;

            // Add all controls to details card
            cardUserDetails.Controls.Add(lblDetails);
            cardUserDetails.Controls.Add(txtUsername);
            cardUserDetails.Controls.Add(txtEmail);
            cardUserDetails.Controls.Add(txtPassword);
            cardUserDetails.Controls.Add(cmbRole);
            cardUserDetails.Controls.Add(chkIsActive);
            cardUserDetails.Controls.Add(btnSave);
            cardUserDetails.Controls.Add(btnEdit);
            cardUserDetails.Controls.Add(btnResetPassword);
            cardUserDetails.Controls.Add(btnDelete);
            cardUserDetails.Controls.Add(btnClear);

            this.Controls.Add(cardUserDetails);
        }

        private void LoadUsers()
        {
            try
            {
                // Check if dgvUsers is initialized
                if (dgvUsers == null)
                {
                    return; // Exit silently if not yet initialized
                }

                var users = _userRepository.GetAllUsers();

                if (users == null || !users.Any())
                {
                    // Show placeholder data in the grid
                    var noDataPlaceholder = new List<object>
                    {
                        new
                        {
                            UserId = (int?)null,
                            Username = "No data available",
                            Email = "No users found in the system",
                            Role = "-",
                            Status = "-",
                            CreatedAt = "-",
                            LastLogin = "-"
                        }
                    };

                    dgvUsers.DataSource = noDataPlaceholder;
                    return;
                }

                // Convert to list first, then select to avoid expression tree issues
                var usersList = users.ToList();
                var userData = usersList.Select(u => new
                {
                    UserId = u.UserId,
                    Username = u.Username ?? "Unknown",
                    Email = u.Email ?? "No Email",
                    Role = u.Role ?? "No Role",
                    Status = u.IsActive ? "Active" : "Inactive",
                    CreatedAt = u.CreatedAt.ToString("dd/MM/yyyy"),
                    LastLogin = u.LastLogin?.ToString("dd/MM/yyyy HH:mm") ?? "Never"
                }).ToList();

                dgvUsers.DataSource = userData;
            }
            catch (Exception ex)
            {
                if (dgvUsers != null)
                {
                    // Show error placeholder in the grid
                    var errorPlaceholder = new List<object>
                    {
                        new
                        {
                            UserId = (int?)null,
                            Username = "Error",
                            Email = $"Error loading data: {ex.Message}",
                            Role = "-",
                            Status = "-",
                            CreatedAt = "-",
                            LastLogin = "-"
                        }
                    };

                    dgvUsers.DataSource = errorPlaceholder;
                }

                MaterialMessageBox.Show($"Error loading users: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var allUsers = _userRepository.GetAllUsers();
                
                if (allUsers == null || !allUsers.Any())
                {
                    LoadUsers(); // This will show the "no data" placeholder
                    return;
                }

                // Work with List<T> directly to avoid expression tree issues
                var usersList = allUsers.ToList();
                var filteredUsers = new List<User>();

                foreach (var user in usersList)
                {
                    bool matchesSearch = true;
                    bool matchesRole = true;

                    // Apply search filter
                    if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        var searchTerm = txtSearch.Text.Trim().ToLower();
                        matchesSearch = (!string.IsNullOrEmpty(user.Username) && user.Username.ToLower().Contains(searchTerm)) ||
                                       (!string.IsNullOrEmpty(user.Email) && user.Email.ToLower().Contains(searchTerm));
                    }

                    // Apply role filter
                    if (cmbRoleFilter.SelectedIndex > 0)
                    {
                        var selectedRole = cmbRoleFilter.SelectedItem?.ToString();
                        matchesRole = !string.IsNullOrEmpty(selectedRole) && user.Role == selectedRole;
                    }

                    if (matchesSearch && matchesRole)
                    {
                        filteredUsers.Add(user);
                    }
                }

                if (!filteredUsers.Any())
                {
                    var noResultsPlaceholder = new List<object>
                    {
                        new
                        {
                            UserId = (int?)null,
                            Username = "No results found",
                            Email = "No users match your search criteria",
                            Role = "-",
                            Status = "-",
                            CreatedAt = "-",
                            LastLogin = "-"
                        }
                    };

                    dgvUsers.DataSource = noResultsPlaceholder;
                    return;
                }

                // Convert to display data
                var userData = filteredUsers.Select(u => new
                {
                    UserId = u.UserId,
                    Username = u.Username ?? "Unknown",
                    Email = u.Email ?? "No Email",
                    Role = u.Role ?? "No Role",
                    Status = u.IsActive ? "Active" : "Inactive",
                    CreatedAt = u.CreatedAt.ToString("dd/MM/yyyy"),
                    LastLogin = u.LastLogin?.ToString("dd/MM/yyyy HH:mm") ?? "Never"
                }).ToList();

                dgvUsers.DataSource = userData;
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error searching users: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cmbRoleFilter.SelectedIndex = 0;
            LoadUsers();
            ClearForm();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtUsername.Focus();
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedRow = dgvUsers.SelectedRows[0];
                    var userIdValue = selectedRow.Cells["UserId"].Value;

                    // Check if this is a placeholder row
                    if (userIdValue == null || userIdValue == DBNull.Value)
                    {
                        ClearForm();
                        return;
                    }

                    var userId = Convert.ToInt32(userIdValue);

                    _selectedUser = _userRepository.GetUserById(userId);
                    if (_selectedUser != null)
                    {
                        PopulateForm(_selectedUser);
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error loading user details: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PopulateForm(User user)
        {
            txtUsername.Text = user.Username ?? "";
            txtEmail.Text = user.Email ?? "";
            txtPassword.Text = ""; // Don't show password
            cmbRole.SelectedItem = user.Role;
            chkIsActive.Checked = user.IsActive;
        }

        private void ClearForm()
        {
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            cmbRole.SelectedIndex = -1;
            chkIsActive.Checked = true;
            _selectedUser = null;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cmbRole.SelectedIndex == -1)
            {
                MaterialMessageBox.Show("Please fill in all required fields.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (_selectedUser == null && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MaterialMessageBox.Show("Password is required for new users.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MaterialMessageBox.Show("Please enter a valid email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                if (_selectedUser == null)
                {
                    // Check if user already exists
                    if (_userRepository.UserExists(txtUsername.Text.Trim(), txtEmail.Text.Trim()))
                    {
                        MaterialMessageBox.Show("A user with this username or email already exists.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Create new user
                    var user = new User
                    {
                        Username = txtUsername.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        PasswordHash = PasswordHasher.HashPassword(txtPassword.Text),
                        Role = cmbRole.SelectedItem?.ToString() ?? "customer",
                        IsActive = chkIsActive.Checked,
                        CreatedAt = DateTime.Now
                    };

                    _userRepository.AddUser(user);
                    MaterialMessageBox.Show("User created successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update existing user
                    _selectedUser.Username = txtUsername.Text.Trim();
                    _selectedUser.Email = txtEmail.Text.Trim();
                    _selectedUser.Role = cmbRole.SelectedItem?.ToString() ?? "customer";
                    _selectedUser.IsActive = chkIsActive.Checked;

                    // Update password if provided
                    if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        _selectedUser.PasswordHash = PasswordHasher.HashPassword(txtPassword.Text);
                    }

                    _userRepository.UpdateUser(_selectedUser);
                    MaterialMessageBox.Show("User updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadUsers();
                ClearForm();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show($"Error saving user: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null)
            {
                MaterialMessageBox.Show("Please select a user to edit.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MaterialMessageBox.Show("You can now edit the user details and click Save.", "Edit Mode",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null)
            {
                MaterialMessageBox.Show("Please select a user to reset password.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MaterialMessageBox.Show(
                $"Are you sure you want to reset password for user '{_selectedUser.Username}'?",
                "Confirm Password Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var newPassword = PasswordHasher.GenerateRandomPassword();
                    _selectedUser.PasswordHash = PasswordHasher.HashPassword(newPassword);
                    _userRepository.UpdateUser(_selectedUser);

                    MaterialMessageBox.Show($"Password reset successfully!\nNew password: {newPassword}\n\nPlease provide this to the user and ask them to change it.",
                        "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error resetting password: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null)
            {
                MaterialMessageBox.Show("Please select a user to delete.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prevent deleting current user (if SessionManager exists)
            try
            {
                if (SessionManager.CurrentUser != null && _selectedUser.UserId == SessionManager.CurrentUser.UserId)
                {
                    MaterialMessageBox.Show("You cannot delete your own account.", "Operation Not Allowed",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch
            {
                // SessionManager might not be available, continue with deletion
            }

            var result = MaterialMessageBox.Show(
                $"Are you sure you want to delete user '{_selectedUser.Username}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _userRepository.DeleteUser(_selectedUser.UserId);
                    MaterialMessageBox.Show("User deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUsers();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show($"Error deleting user: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void UserManagementPanel_Load(object sender, EventArgs e)
        {
            // This event handler can be used for additional initialization if needed
        }
    }
}