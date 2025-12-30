using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2TextBox txtUsername;
        private Guna2TextBox txtEmail;
        private Guna2TextBox txtPassword;
        private Guna2TextBox txtConfirm;
        private Guna2ComboBox cbRole; // Mới thêm
        private Guna2Button btnRegister;
        private Label lblTitle;
        private Guna2ControlBox btnClose;
        private Guna2ControlBox btnMinimize;
        private Guna2DragControl dragControl;
        private Guna2Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 600);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(3, 3, 3);

            this.dragControl = new Guna2DragControl(this.components);
            this.dragControl.TargetControl = this;

            // Control Box
            this.btnClose = new Guna2ControlBox { Anchor = AnchorStyles.Top | AnchorStyles.Right, FillColor = Color.Transparent, Location = new Point(355, 0), Size = new Size(45, 30) };
            this.btnMinimize = new Guna2ControlBox { ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox, Anchor = AnchorStyles.Top | AnchorStyles.Right, FillColor = Color.Transparent, Location = new Point(310, 0), Size = new Size(45, 30) };

            // Title
            this.lblTitle = new Label();
            this.lblTitle.Text = "Tạo Tài Khoản";
            this.lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(100, 40);

            // Email (NEW)
            this.txtEmail = CreateTextBox("Email", 100);

            // Username
            this.txtUsername = CreateTextBox("Tên đăng nhập", 160);

            // Password
            this.txtPassword = CreateTextBox("Mật khẩu", 220, true);

            // Confirm Password
            this.txtConfirm = CreateTextBox("Nhập lại mật khẩu", 280, true);

            // Role Combobox (Quan trọng)
            this.cbRole = new Guna2ComboBox();
            this.cbRole.Text = "Bạn là ai?";
            this.cbRole.FillColor = Color.FromArgb(33, 33, 33);
            this.cbRole.ForeColor = Color.White;
            this.cbRole.BorderThickness = 0;
            this.cbRole.BorderRadius = 4;
            this.cbRole.Location = new Point(40, 340);
            this.cbRole.Size = new Size(320, 45);

            // Register Button
            this.btnRegister = new Guna2Button();
            this.btnRegister.Text = "ĐĂNG KÝ";
            this.btnRegister.FillColor = Color.White;
            this.btnRegister.ForeColor = Color.Black;
            this.btnRegister.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnRegister.BorderRadius = 22;
            this.btnRegister.Location = new Point(40, 410);
            this.btnRegister.Size = new Size(320, 45);
            this.btnRegister.Cursor = Cursors.Hand;

            // Back Button
            this.btnBack = new Guna2Button();
            this.btnBack.Text = "Quay lại Đăng nhập";
            this.btnBack.FillColor = Color.Transparent;
            this.btnBack.ForeColor = Color.FromArgb(170, 170, 170);
            this.btnBack.Font = new Font("Segoe UI", 9);
            this.btnBack.Location = new Point(40, 470);
            this.btnBack.Size = new Size(320, 30);
            this.btnBack.Cursor = Cursors.Hand;

            this.Controls.Add(btnClose);
            this.Controls.Add(btnMinimize);
            this.Controls.Add(lblTitle);
            this.Controls.Add(txtEmail);
            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(txtConfirm);
            this.Controls.Add(cbRole);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnBack);
        }

        // Helper tạo textbox cho gọn code
        private Guna2TextBox CreateTextBox(string placeholder, int y, bool isPass = false)
        {
            return new Guna2TextBox
            {
                PlaceholderText = placeholder,
                UseSystemPasswordChar = isPass,
                FillColor = Color.FromArgb(33, 33, 33),
                ForeColor = Color.White,
                BorderThickness = 0,
                BorderRadius = 4,
                Font = new Font("Segoe UI", 11),
                Location = new Point(40, y),
                Size = new Size(320, 45),
                TextOffset = new Point(10, 0)
            };
        }
    }
}