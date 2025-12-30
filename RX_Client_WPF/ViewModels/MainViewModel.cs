using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RX_Client_WPF.Services;
using RX_Client_WPF.Utils;

namespace RX_Client_WPF.ViewModels
{
    // ObservableObject: Class cơ sở của CommunityToolkit giúp tự động cập nhật UI
    public partial class MainViewModel : ObservableObject
    {
        // AuthService để kiểm tra đăng nhập
        private readonly AuthService _authService;

        // --- CÁC BIẾN TRẠNG THÁI (ObservableProperty) ---
        // [ObservableProperty] sẽ tự động sinh ra property public CurrentViewModel
        // và hàm OnCurrentViewModelChanged để UI biết mà vẽ lại.
        [ObservableProperty]
        private object _currentView;

        [ObservableProperty]
        private bool _isLoggedIn;

        [ObservableProperty]
        private string _userName;

        // ViewModel con cho thanh Player (luôn hiển thị khi đã login)
        [ObservableProperty]
        private PlayerViewModel _playerVM;

        public MainViewModel()
        {
            _authService = new AuthService();
            _playerVM = new PlayerViewModel();

            // Mặc định hiển thị màn hình Login
            CurrentView = new LoginViewModel(this);

            // Kiểm tra session cũ (nếu có tính năng nhớ đăng nhập)
            CheckLoginStatus();
        }

        private void CheckLoginStatus()
        {
            if (Session.IsLoggedIn)
            {
                IsLoggedIn = true;
                UserName = Session.CurrentUser.Username;
                NavigateToHome();
            }
            else
            {
                IsLoggedIn = false;
                CurrentView = new LoginViewModel(this);
            }
        }

        // --- CÁC LỆNH ĐIỀU HƯỚNG (RelayCommand) ---

        [RelayCommand]
        public void NavigateToHome()
        {
            // Chuyển sang View Home (Bạn sẽ tạo HomeViewModel sau)
            // CurrentView = new HomeViewModel(); 
            IsLoggedIn = true; // Hiện thanh điều hướng và player
        }

        [RelayCommand]
        public void NavigateToLibrary()
        {
            // CurrentView = new LibraryViewModel();
        }

        [RelayCommand]
        public void Logout()
        {
            _authService.Logout();
            IsLoggedIn = false;
            CurrentView = new LoginViewModel(this); // Quay về Login
            PlayerVM.StopMusic(); // Tắt nhạc
        }
    }
}