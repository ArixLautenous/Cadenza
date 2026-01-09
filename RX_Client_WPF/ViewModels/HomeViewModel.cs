using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RX_Client.Services;
using RX_Client.ViewModels;
using Shared.DTOs;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace RX_Client.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private readonly MainViewModel _mainVM; // Để gọi Player

        // Dùng ObservableCollection để giao diện tự cập nhật khi list thay đổi
        [ObservableProperty]
        private ObservableCollection<SongDto> _songs;

        [ObservableProperty]
        private bool _isLoading;

        public HomeViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
            _apiService = new ApiService();
            Songs = new ObservableCollection<SongDto>();

            // Tự động tải nhạc khi khởi tạo
            LoadDataCommand.Execute(null);
        }

        [RelayCommand]
        public async Task LoadData()
        {
            IsLoading = true;
            try
            {
                // Gọi API lấy danh sách bài hát
                var list = await _apiService.GetAsync<List<SongDto>>("/api/songs");

                Songs.Clear();
                if (list != null)
                {
                    foreach (var song in list) Songs.Add(song);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task PlaySong(SongDto song)
        {
            // Gọi lệnh Play bên MainViewModel -> PlayerViewModel
            if (song != null)
            {
                await _mainVM.PlayerVM.PlaySong(song);
            }
        }
    }
}
