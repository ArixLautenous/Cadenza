using Guna.UI2.WinForms;
using RX_Client_WF.Forms;
using RX_Client_WF.Services;
using RX_Client_WF.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCSettings : UserControl
    {
        private ApiService _apiService;

        public event EventHandler LogoutClicked;

        public UCSettings()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(3, 3, 3);
            this.Dock = DockStyle.Fill;

            var lblHeader = new Label
            {
                Text = "Cài Đặt",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(40, 30),
                AutoSize = true
            };

            // Section Account
            var lblAccount = new Label
            {
                Text = "Tài khoản",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Gray,
                Location = new Point(40, 100),
                AutoSize = true
            };
            
            // Controls
            var btnChangeUsername = CreateSettingButton("Đổi tên đăng nhập", 140);
            btnChangeUsername.Click += (s, e) => 
            {
                var frm = new ChangeUsernameForm();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LogoutClicked?.Invoke(this, EventArgs.Empty);
                }
            };

            var btnChangePass = CreateSettingButton("Đổi mật khẩu", 200);
            btnChangePass.Click += (s, e) => new ChangePasswordForm().ShowDialog();

            var btnUpdateEmail = CreateSettingButton("Cập nhật / Liên kết Email", 260);
            btnUpdateEmail.Click += (s, e) => new UpdateEmailForm().ShowDialog();

            var btnLogout = new Guna2Button
            {
                Text = "Đăng xuất tài khoản",
                Location = new Point(40, 340),

                Size = new Size(200, 45),
                FillColor = Color.Transparent,
                BorderColor = Color.IndianRed,
                BorderThickness = 1,
                ForeColor = Color.IndianRed,
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogout.Click += (s, e) => LogoutClicked?.Invoke(this, EventArgs.Empty);

            this.Controls.Add(lblHeader);
            this.Controls.Add(lblAccount);
            this.Controls.Add(btnChangeUsername);
            this.Controls.Add(btnChangePass);
            this.Controls.Add(btnUpdateEmail);
            this.Controls.Add(btnLogout);
        }

        private Guna2Button CreateSettingButton(string text, int y)
        {
            return new Guna2Button
            {
                Text = text,
                Location = new Point(40, y),
                Size = new Size(300, 45),
                FillColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                TextAlign = HorizontalAlignment.Left,
                BorderRadius = 5,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
        }
    }
}
