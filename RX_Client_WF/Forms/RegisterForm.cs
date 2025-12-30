using RX_Client_WF.Services;
using Shared.DTOs.Auth;
using Shared.Enums;
using NAudio.CoreAudioApi;
using System;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly ApiService _apiService;

        public RegisterForm()
        {
            InitializeComponent();
            _apiService = new ApiService();

            // Đổ dữ liệu vào ComboBox Role
            cbRole.DataSource = Enum.GetValues(typeof(UserRole));
            cbRole.SelectedItem = UserRole.Listener; // Mặc định là người nghe

            btnRegister.Click += BtnRegister_Click;
            btnBack.Click += (s, e) => this.Close();
        }

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            // 1. Validate
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp.");
                return;
            }

            btnRegister.Text = "ĐANG TẠO TÀI KHOẢN...";
            btnRegister.Enabled = false;

            try
            {
                var request = new RegisterRequest1
                {
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    ConfirmPassword = txtConfirm.Text,
                    Email = txtEmail.Text,
                    Role = (UserRole)cbRole.SelectedItem
                };

                // Gọi API Register (trả về chuỗi message hoặc 200 OK)
                var result = await _apiService.PostAsync<string>("/api/auth/register", request);

                MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.");
                this.Close(); // Đóng form đăng ký để quay về Login
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đăng ký thất bại: {ex.Message}");
                btnRegister.Text = "ĐĂNG KÝ";
                btnRegister.Enabled = true;
            }
        }
    }
}