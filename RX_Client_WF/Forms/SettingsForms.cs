using Guna.UI2.WinForms;
using RX_Client_WF.Services;
using RX_Client_WF.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    public class ChangePasswordForm : Form
    {
        private Guna2TextBox txtOldPass;
        private Guna2TextBox txtNewPass;
        private Guna2TextBox txtConfirmPass;
        private Guna2Button btnSave;
        private ApiService _apiService;

        public ChangePasswordForm()
        {
            _apiService = new ApiService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(400, 350);
            this.Text = "Đổi Mật Khẩu";
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.ForeColor = Color.White;

            var lblOld = new Label { Text = "Mật khẩu cũ:", Location = new Point(20, 20), AutoSize = true, ForeColor = Color.Gray };
            txtOldPass = CreatePassBox(50);
            
            var lblNew = new Label { Text = "Mật khẩu mới:", Location = new Point(20, 100), AutoSize = true, ForeColor = Color.Gray };
            txtNewPass = CreatePassBox(130);

            var lblConfirm = new Label { Text = "Xác nhận mật khẩu:", Location = new Point(20, 180), AutoSize = true, ForeColor = Color.Gray };
            txtConfirmPass = CreatePassBox(210);

            btnSave = new Guna2Button
            {
                Text = "XÁC NHẬN",
                Location = new Point(100, 270),
                Size = new Size(200, 45),
                BorderRadius = 20,
                FillColor = Color.FromArgb(29, 185, 84),
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lblOld);
            this.Controls.Add(txtOldPass);
            this.Controls.Add(lblNew);
            this.Controls.Add(txtNewPass);
            this.Controls.Add(lblConfirm);
            this.Controls.Add(txtConfirmPass);
            this.Controls.Add(btnSave);
        }

        private Guna2TextBox CreatePassBox(int y)
        {
            return new Guna2TextBox
            {
                Location = new Point(20, y),
                Size = new Size(340, 36),
                BorderRadius = 4,
                FillColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                PasswordChar = '•'
            };
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOldPass.Text) || string.IsNullOrEmpty(txtNewPass.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            if (txtNewPass.Text != txtConfirmPass.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.");
                return;
            }

            var request = new { OldPassword = txtOldPass.Text, NewPassword = txtNewPass.Text };
            
            // Can sua ApiService them PutAsync generic hoac dung truc tiep
            // _apiService.PutAsync co san tu buoc truoc
            bool success = await _apiService.PutAsync("/api/users/change-password", request);

            if (success)
            {
                MessageBox.Show("Đổi mật khẩu thành công!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Đổi mật khẩu thất bại. Kiểm tra lại mật khẩu cũ.");
            }
        }
    }

    public class ChangeUsernameForm : Form
    {
        private Guna2TextBox txtNewUsername;
        private Guna2Button btnSave;
        private ApiService _apiService;

        public ChangeUsernameForm()
        {
            _apiService = new ApiService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(400, 200);
            this.Text = "Đổi Tên Đăng Nhập";
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.ForeColor = Color.White;

            var lbl = new Label { Text = "Tên đăng nhập mới:", Location = new Point(20, 20), AutoSize = true, ForeColor = Color.Gray };
            txtNewUsername = new Guna2TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(340, 36),
                BorderRadius = 4,
                FillColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White
            };

            btnSave = new Guna2Button
            {
                Text = "LƯU",
                Location = new Point(100, 110),
                Size = new Size(200, 40),
                BorderRadius = 20,
                FillColor = Color.FromArgb(29, 185, 84),
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lbl);
            this.Controls.Add(txtNewUsername);
            this.Controls.Add(btnSave);
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên mới.");
                return;
            }

            // PutAsync string body can boc trong quotes neu backend nhan [FromBody] string
            // Hoac backend nhan object. Controller nhan [FromBody] string newUsername
            // RestSharp AddJsonBody se serialize string -> "string". Dungs.
            
            bool success = await _apiService.PutAsync("/api/users/change-username", txtNewUsername.Text);

            if (success)
            {
                MessageBox.Show("Đổi tên thành công! Vui lòng đăng nhập lại.");
                // Session.ClearSession(); -> Logic o form cha
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại hoặc lỗi server.");
            }
        }
    }
    public class UpdateEmailForm : Form
    {
        private Guna2TextBox txtEmail;
        private Guna2Button btnSave;
        private Guna2ControlBox btnClose;
        private Guna2DragControl dragControl;
        private ApiService _apiService;

        public UpdateEmailForm()
        {
            _apiService = new ApiService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(400, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;
            
            // Drag Control
            this.dragControl = new Guna2DragControl();
            this.dragControl.TargetControl = this;

            // Close Button
            this.btnClose = new Guna2ControlBox { Anchor = AnchorStyles.Top | AnchorStyles.Right, FillColor = Color.Transparent, Location = new Point(355, 0), Size = new Size(45, 30) };

            var lblTitle = new Label { 
                Text = "Liên kết / Đổi Email", 
                Location = new Point(20, 20), 
                AutoSize = true, 
                Font = new Font("Segoe UI", 14, FontStyle.Bold), 
                ForeColor = Color.White 
            };

            var lblDesc = new Label { 
                Text = "Nhập địa chỉ Email của bạn:", 
                Location = new Point(20, 60), 
                AutoSize = true, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray 
            };

            txtEmail = new Guna2TextBox
            {
                Location = new Point(20, 90),
                Size = new Size(360, 40),
                BorderRadius = 4,
                FillColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "name@example.com"
            };

            btnSave = new Guna2Button
            {
                Text = "LƯU THAY ĐỔI",
                Location = new Point(100, 160),
                Size = new Size(200, 45),
                BorderRadius = 22,
                FillColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(btnClose);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblDesc);
            this.Controls.Add(txtEmail);
            this.Controls.Add(btnSave);
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập Email.");
                return;
            }
            
            btnSave.Enabled = false;
            btnSave.Text = "ĐANG XỬ LÝ...";

            bool success = await _apiService.PutAsync("/api/users/update-email", txtEmail.Text);

            if (success)
            {
                MessageBox.Show("Cập nhật Email thành công!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Email không hợp lệ hoặc đã được sử dụng.");
                btnSave.Enabled = true;
                btnSave.Text = "LƯU THAY ĐỔI";
            }
        }
    }
}
