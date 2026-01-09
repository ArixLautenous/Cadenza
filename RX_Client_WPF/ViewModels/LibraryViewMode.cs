using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RX_Client.Services;
using System.Collections.ObjectModel;

namespace RX_Client.ViewModels
{
    public partial class LibraryViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        // Dùng dynamic hoặc tạo class PlaylistDto trong Shared nếu muốn chặt chẽ
        // Ở đây demo dùng dynamic cho nhanh
        [ObservableProperty] private ObservableCollection<dynamic> _playlists;
        [ObservableProperty] private bool _isLoading;

        public LibraryViewModel()
        {
            _apiService = new ApiService();
            Playlists = new ObservableCollection<dynamic>();
            LoadLibraryCommand.Execute(null);
        }

        [RelayCommand]
        public async Task LoadLibrary()
        {
            IsLoading = true;
            try
            {
                // Gọi API lấy danh sách Playlist của User
                var data = await _apiService.GetAsync<List<dynamic>>("/api/users/playlists");
                Playlists.Clear();
                if (data != null)
                {
                    foreach (var p in data) Playlists.Add(p);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
