using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Windows.Forms;

namespace eShift.Presentation
{
    public partial class LoginForm : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;

        public LoginForm()
        {
            InitializeComponent();

            // Initialize MaterialSkinManager
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true; // Or false
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Indigo500, Primary.Indigo700,
                Primary.Indigo100, Accent.Pink200,
                TextShade.WHITE
            );
        }

        private void InitializeComponent()
        {
            this.txtUsername = new MaterialSkin.Controls.MaterialTextBox();
            this.txtPassword = new MaterialSkin.Controls.MaterialTextBox();
            this.btnLogin = new MaterialSkin.Controls.MaterialButton();
            this.chkRememberMe = new MaterialSkin.Controls.MaterialCheckbox();
            this.lblStatus = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            //
            // txtUsername
            //
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsername.Depth = 0;
            this.txtUsername.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtUsername.Hint = "Username";
            this.txtUsername.Location = new System.Drawing.Point(40, 100); // Adjusted Y for MaterialForm
            this.txtUsername.MaxLength = 50;
            this.txtUsername.MouseState = MaterialSkin.MouseState.OUT;
            this.txtUsername.Multiline = false;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(320, 50);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.Text = "";
            this.txtUsername.UseTallSize = false; // Use standard size
            //
            // txtPassword
            //
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Depth = 0;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtPassword.Hint = "Password";
            this.txtPassword.Location = new System.Drawing.Point(40, 170);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.MouseState = MaterialSkin.MouseState.OUT;
            this.txtPassword.Multiline = false;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Password = true;
            this.txtPassword.Size = new System.Drawing.Size(320, 50);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "";
            this.txtPassword.UseTallSize = false;
            //
            // btnLogin
            //
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLogin.Depth = 0;
            this.btnLogin.DrawShadows = true;
            this.btnLogin.HighEmphasis = true;
            this.btnLogin.Icon = null;
            this.btnLogin.Location = new System.Drawing.Point(150, 280);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnLogin.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(98, 36);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Login";
            this.btnLogin.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnLogin.UseAccentColor = false;
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            //
            // chkRememberMe
            //
            this.chkRememberMe.AutoSize = true;
            this.chkRememberMe.Depth = 0;
            this.chkRememberMe.Location = new System.Drawing.Point(40, 230);
            this.chkRememberMe.Margin = new System.Windows.Forms.Padding(0);
            this.chkRememberMe.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkRememberMe.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkRememberMe.Name = "chkRememberMe";
            this.chkRememberMe.Ripple = true;
            this.chkRememberMe.Size = new System.Drawing.Size(139, 37);
            this.chkRememberMe.TabIndex = 2;
            this.chkRememberMe.Text = "Remember Me";
            this.chkRememberMe.UseVisualStyleBackColor = true;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Depth = 0;
            this.lblStatus.Font = new System.Drawing.Font("Roboto", 10F);
            this.lblStatus.Location = new System.Drawing.Point(40, 330);
            this.lblStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = ""; // Will show login errors
            //
            // LoginForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 400); // Adjusted size
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.chkRememberMe);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblStatus);
            this.Name = "LoginForm";
            this.Padding = new System.Windows.Forms.Padding(3, 64, 3, 3); // MaterialForm padding
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "eShift - Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private MaterialSkin.Controls.MaterialTextBox txtUsername;
        private MaterialSkin.Controls.MaterialTextBox txtPassword;
        private MaterialSkin.Controls.MaterialButton btnLogin;
        private MaterialSkin.Controls.MaterialCheckbox chkRememberMe;
        private MaterialSkin.Controls.MaterialLabel lblStatus;


        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Placeholder for login logic
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Basic validation (more will be in Application layer)
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblStatus.Text = "Username and password are required.";
                // MessageBox.Show("Username and password are required.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblStatus.Text = "Attempting login...";

            // Here, you would typically call an AuthService to validate credentials.
            // For example:
            // var authService = new AuthService(new UserRepository(new DbConnectionFactory()));
            // var user = await authService.LoginAsync(username, password);
            //
            // if (user != null)
            // {
            //    lblStatus.Text = "Login Successful!";
            //    // Proceed to open the main dashboard based on user.Role
            //    // Example:
            //    // if (user.Role == UserRole.Admin) { /* Open Admin Dashboard */ }
            //    // else { /* Open Customer Dashboard */ }
            //    // this.Hide();
            // }
            // else
            // {
            //    lblStatus.Text = "Invalid username or password.";
            //    // MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // }

            // Dummy logic for now:
            if (username == "admin" && password == "admin")
            {
                lblStatus.Text = "Login Successful (Admin)!";
                // Open Admin Dashboard (placeholder)
                // MessageBox.Show("Admin Login Successful!", "Success");
                // this.Hide();
                // var adminDashboard = new AdminDashboardForm(); // Assuming this form exists
                // adminDashboard.ShowDialog();
                // this.Close();
            }
            else if (username == "customer" && password == "customer")
            {
                 lblStatus.Text = "Login Successful (Customer)!";
                // Open Customer Dashboard (placeholder)
                // MessageBox.Show("Customer Login Successful!", "Success");
                // this.Hide();
                // var customerDashboard = new CustomerDashboardForm(); // Assuming this form exists
                // customerDashboard.ShowDialog();
                // this.Close();
            }
            else
            {
                lblStatus.Text = "Invalid credentials (dummy check).";
            }
        }
    }
}
