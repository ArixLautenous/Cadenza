using Guna.UI2.WinForms;
using RX_Client_WF.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    public class ForgotPasswordForm : Form
    {
        private Guna2TextBox txtEmail;
        private Guna2Button btnSendCode;
        private Guna2TextBox txtCode;
        private Guna2TextBox txtNewPassword;
        private Guna2Button btnReset;
        private Label lblStatus;
        
        private ApiService _apiService;
        private string _email;

        // Panel Steps
        private Panel pnStep1;
        private Panel pnStep2;

        public ForgotPasswordForm()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(3, 3, 3);

            // Drag Control
            new Guna2DragControl(this.components) { TargetControl = this };

            // Control Box
            var btnClose = new Guna2ControlBox { Anchor = AnchorStyles.Top | AnchorStyles.Right, FillColor = Color.Transparent, IconColor = Color.White, Location = new Point(355, 0), Size = new Size(45, 30) };
            var btnMin = new Guna2ControlBox { Anchor = AnchorStyles.Top | AnchorStyles.Right, ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox, FillColor = Color.Transparent, IconColor = Color.White, Location = new Point(310, 0), Size = new Size(45, 30) };
            this.Controls.Add(btnClose);
            this.Controls.Add(btnMin);

            var lblTitle = new Label
            {
                Text = "Khôi Phục Tài Khoản",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(60, 50)
            };
            this.Controls.Add(lblTitle);

            // --- STEP 1: NHAP EMAIL ---
            pnStep1 = new Panel { Size = new Size(400, 300), Location = new Point(0, 100), BackColor = Color.Transparent };
            
            txtEmail = CreateTextBox("Email hoặc tên tài khoản", 20);
            btnSendCode = CreateButton("GỬI MÃ XÁC NHẬN", 90);
            
            btnSendCode.Click += async (s, e) =>
            {
                if (string.IsNullOrEmpty(txtEmail.Text)) { MessageBox.Show("Vui lòng nhập Email."); return; }
                
                btnSendCode.Enabled = false;
                btnSendCode.Text = "ĐANG GỬI...";
                
                bool result = await _apiService.ForgotPasswordAsync(txtEmail.Text);
                if (result)
                {
                    _email = txtEmail.Text;
                    pnStep1.Visible = false;
                    pnStep2.Visible = true;
                    MessageBox.Show("Mã xác nhận đã gửi đến Email của bạn!");
                }
                else
                {
                    btnSendCode.Enabled = true;
                    btnSendCode.Text = "GỬI MÃ XÁC NHẬN";
                }
            };

            pnStep1.Controls.Add(txtEmail);
            pnStep1.Controls.Add(btnSendCode);
            this.Controls.Add(pnStep1);


            // --- STEP 2: NHAP CODE & RESET ---
            pnStep2 = new Panel { Size = new Size(400, 350), Location = new Point(0, 100), Visible = false, BackColor = Color.Transparent };

            txtCode = CreateTextBox("Mã xác nhận (6 số)", 20);
            txtNewPassword = CreateTextBox("Mật khẩu mới", 80);
            txtNewPassword.UseSystemPasswordChar = true;
            
            btnReset = CreateButton("ĐỔI MẬT KHẨU", 150);
            btnReset.Click += async (s, e) =>
            {
                if (string.IsNullOrEmpty(txtCode.Text) || string.IsNullOrEmpty(txtNewPassword.Text))
                { MessageBox.Show("Vui lòng nhập đủ thông tin."); return; }

                bool result = await _apiService.ResetPasswordAsync(_email, txtCode.Text, txtNewPassword.Text);
                if (result)
                {
                    MessageBox.Show("Đổi mật khẩu thành công! Hãy đăng nhập lại.");
                    this.Close();
                }
            };

            Guna2Button btnBack = new Guna2Button 
            { 
                Text = "Quay lại", 
                FillColor = Color.Transparent, 
                ForeColor = Color.FromArgb(170,170,170), 
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(140, 210),
                Cursor = Cursors.Hand
            };
            btnBack.Click += (s, e) => { pnStep2.Visible = false; pnStep1.Visible = true; };

            pnStep2.Controls.Add(txtCode);
            pnStep2.Controls.Add(txtNewPassword);
            pnStep2.Controls.Add(btnReset);
            pnStep2.Controls.Add(btnBack);
            this.Controls.Add(pnStep2);
        }

        private Guna2TextBox CreateTextBox(string placeholder, int y)
        {
            return new Guna2TextBox
            {
                PlaceholderText = placeholder,
                Size = new Size(320, 50),
                Location = new Point(40, y),
                BorderRadius = 4,
                FillColor = Color.FromArgb(33, 33, 33),
                ForeColor = Color.White,
                BorderThickness = 0,
                Font = new Font("Segoe UI", 11),
                TextOffset = new Point(10, 0)
            };
        }

        private Guna2Button CreateButton(string text, int y)
        {
            return new Guna2Button
            {
                Text = text,
                Size = new Size(320, 50),
                Location = new Point(40, y),
                BorderRadius = 25,
                FillColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                HoverState = { FillColor = Color.FromArgb(230,230,230) }
            };
        }
    }
}
