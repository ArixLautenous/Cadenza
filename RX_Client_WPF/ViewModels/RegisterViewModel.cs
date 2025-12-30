using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RX_Client_WPF.Services;
using RX_Client_WPF.ViewModels;
using Shared.DTOs.Auth;
using Shared.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RX_Client_WPF.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private readonly MainViewModel _mainVM;

        [ObservableProperty] private string _username;
        [ObservableProperty] private string _password;
        [ObservableProperty] private string _confirmPassword;
        [ObservableProperty] private UserRole _selectedRole = UserRole.Listener;
        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private string _errorMessage;

        // List Role để hiển thị lên ComboBox
        public ObservableCollection<UserRole> Roles { get; } = new ObservableCollection<UserRole>
        {
            UserRole.Listener,
            UserRole.Singer,
            UserRole.Composer,
            UserRole.Producer
        };

        public RegisterViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
            _apiService = new ApiService();
        }

        [RelayCommand]
        public async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Mật khẩu nhập lại không khớp.";
                return;
            }

            IsLoading = true;
            ErrorMessage = "";

            try
            {
                var request = new RegisterRequest1
                {
                    Username = Username,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword,
                    Role = SelectedRole
                };

                // Gọi API đăng ký (server trả về string message hoặc 200 OK)
                var result = await _apiService.PostAsync<string>("/api/auth/register", request);

                // Nếu không văng lỗi exception là thành công
                MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.");
                _mainVM.CurrentView = new LoginViewModel(_mainVM);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Đăng ký thất bại: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public void BackToLogin()
        {
            _mainVM.CurrentView = new LoginViewModel(_mainVM);
        }
    }
}