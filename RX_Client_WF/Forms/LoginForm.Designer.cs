using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        // Khai báo Controls
        private Guna2TextBox txtUsername;
        private Guna2TextBox txtPassword;
        private Guna2Button btnLogin;
        private Label lblTitle;
        private Label lblSubTitle;
        private Label lblRegister;
        private Label lblForgotPassword;
        private Guna2ControlBox btnClose;
        private Guna2ControlBox btnMinimize;
        private Guna2DragControl dragControl;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // 1. Cấu hình Form (YouTube Music Style)
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 550);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; // Bỏ viền Windows cũ
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(3, 3, 3); // Nền đen sâu (Deep Black)

            // 2. Drag Control (Để kéo thả được form không viền)
            this.dragControl = new Guna2DragControl(this.components);
            this.dragControl.TargetControl = this;

            // 3. Control Box (Nút X và -)
            this.btnClose = new Guna2ControlBox();
            this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnClose.FillColor = Color.Transparent;
            this.btnClose.IconColor = Color.White;
            this.btnClose.Location = new Point(355, 0);
            this.btnClose.Size = new Size(45, 30);

            this.btnMinimize = new Guna2ControlBox();
            this.btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnMinimize.FillColor = Color.Transparent;
            this.btnMinimize.IconColor = Color.White;
            this.btnMinimize.Location = new Point(310, 0);
            this.btnMinimize.Size = new Size(45, 30);

            // 4. Logo / Tiêu đề
            this.lblTitle = new Label();
            this.lblTitle.Text = "Đăng Nhập";
            this.lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.AutoSize = true;
            // Căn giữa thủ công (400/2 - width/2)
            this.lblTitle.Location = new Point(105, 80);

            this.lblSubTitle = new Label();
            this.lblSubTitle.Text = "Tiếp tục với Cadenza";
            this.lblSubTitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.lblSubTitle.ForeColor = Color.FromArgb(170, 170, 170); // Xám nhạt
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Location = new Point(125, 125);

            // 5. Username Box
            this.txtUsername = new Guna2TextBox();
            this.txtUsername.PlaceholderText = "Email hoặc tên người dùng";
            this.txtUsername.FillColor = Color.FromArgb(33, 33, 33); // Xám YouTube
            this.txtUsername.ForeColor = Color.White;
            this.txtUsername.BorderThickness = 0; // Không viền
            this.txtUsername.BorderRadius = 4;
            this.txtUsername.Font = new Font("Segoe UI", 11);
            this.txtUsername.Location = new Point(40, 180);
            this.txtUsername.Size = new Size(320, 50);
            this.txtUsername.TextOffset = new Point(10, 0);

            // 6. Password Box
            this.txtPassword = new Guna2TextBox();
            this.txtPassword.PlaceholderText = "Mật khẩu";
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.FillColor = Color.FromArgb(33, 33, 33);
            this.txtPassword.ForeColor = Color.White;
            this.txtPassword.BorderThickness = 0;
            this.txtPassword.BorderRadius = 4;
            this.txtPassword.Font = new Font("Segoe UI", 11);
            this.txtPassword.Location = new Point(40, 250);
            this.txtPassword.Size = new Size(320, 50);
            this.txtPassword.TextOffset = new Point(10, 0);

            // 7. Login Button (Style nút trắng chữ đen đặc trưng của YTM)
            this.btnLogin = new Guna2Button();
            this.btnLogin.Text = "TIẾP TỤC";
            this.btnLogin.FillColor = Color.White;
            this.btnLogin.ForeColor = Color.Black;
            this.btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnLogin.BorderRadius = 25; // Bo tròn full
            this.btnLogin.Location = new Point(40, 340);
            this.btnLogin.Size = new Size(320, 50);
            this.btnLogin.Cursor = Cursors.Hand;
            // Hiệu ứng hover: hơi xám chút
            this.btnLogin.HoverState.FillColor = Color.FromArgb(230, 230, 230);

            // 8. Register Link
            this.lblRegister = new Label();
            this.lblRegister.Text = "Chưa có tài khoản? Đăng ký ngay";
            this.lblRegister.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.lblRegister.ForeColor = Color.FromArgb(170, 170, 170);
            this.lblRegister.AutoSize = true;
            this.lblRegister.Cursor = Cursors.Hand;
            this.lblRegister.Location = new Point(100, 410);

            // 9. Forgot Password Link
            this.lblForgotPassword = new Label();
            this.lblForgotPassword.Text = "Quên mật khẩu?";
            this.lblForgotPassword.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.lblForgotPassword.ForeColor = Color.FromArgb(170, 170, 170);
            this.lblForgotPassword.AutoSize = true;
            this.lblForgotPassword.Cursor = Cursors.Hand;
            this.lblForgotPassword.Location = new Point(145, 440);

            // Thêm Controls vào Form
            this.Controls.Add(btnClose);
            this.Controls.Add(btnMinimize);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubTitle);
            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblRegister);
            this.Controls.Add(lblForgotPassword);
        }
    }
}