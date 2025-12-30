using RX_Client_WF.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService();

            // Gắn sự kiện (Event Handlers)
            btnLogin.Click += BtnLogin_Click;
            lblRegister.Click += LblRegister_Click;
            lblRegister.MouseEnter += (s, e) => lblRegister.ForeColor = Color.White; // Hover sáng lên
            lblRegister.MouseLeave += (s, e) => lblRegister.ForeColor = Color.FromArgb(170, 170, 170);

            lblForgotPassword.Click += (s, e) => { new ForgotPasswordForm().ShowDialog(); };
            lblForgotPassword.MouseEnter += (s, e) => lblForgotPassword.ForeColor = Color.White;
            lblForgotPassword.MouseLeave += (s, e) => lblForgotPassword.ForeColor = Color.FromArgb(170, 170, 170);

            // Xử lý phím Enter để login
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(s, e); };
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // UI Feedback: Đổi trạng thái nút
            btnLogin.Text = "ĐANG XỬ LÝ...";
            btnLogin.Enabled = false;

            try
            {
                bool success = await _authService.LoginAsync(txtUsername.Text, txtPassword.Text);

                if (success)
                {
                    // Đăng nhập thành công -> Mở MainForm
                    MainForm main = new MainForm();
                    this.Hide();
                    main.ShowDialog(); // ShowDialog để block thread cũ
                    this.Close(); // Đóng Login khi Main tắt
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}");
            }
            finally
            {
                // Reset nút
                btnLogin.Text = "TIẾP TỤC";
                btnLogin.Enabled = true;
            }
        }

        private void LblRegister_Click(object sender, EventArgs e)
        {
            RegisterForm reg = new RegisterForm();
            this.Hide();
            reg.ShowDialog();
            this.Show(); // Hiện lại login khi đóng form đăng ký
        }
    }
}