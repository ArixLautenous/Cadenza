using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RX_Client_WPF.Services;
using RX_Client_WPF.Utils;
using RX_Client_WPF.ViewModels;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RX_Client_WPF.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        private readonly MainViewModel _mainViewModel; // Tham chiếu để điều hướng

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _isLoading;

        public LoginViewModel(MainViewModel mainViewModel)
        {
            _authService = new AuthService();
            _mainViewModel = mainViewModel;
        }

        [RelayCommand]
        public async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                // Gọi API Login
                var success = await _authService.LoginAsync(Username, Password);

                if (success)
                {
                    // Đăng nhập thành công -> Báo cho MainViewModel chuyển sang Home
                    _mainViewModel.NavigateToHome();
                }
                else
                {
                    ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi kết nối: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public void GoToRegister()
        {
            // Chuyển sang màn hình đăng ký
            // _mainViewModel.CurrentView = new RegisterViewModel(_mainViewModel);
            MessageBox.Show("Chức năng đang phát triển!");
        }
    }
}