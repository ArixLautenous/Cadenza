using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RX_Client.Services;
using System.Windows;

namespace RX_Client.ViewModels
{
    public partial class UploadViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty] private string _title;
        [ObservableProperty] private int _selectedGenreId = 1; // Mặc định Pop (ID=1)
        [ObservableProperty] private bool _isExclusive;
        [ObservableProperty] private string _audioPath; // Đường dẫn file trên máy
        [ObservableProperty] private string _imagePath; // Đường dẫn file ảnh
        [ObservableProperty] private bool _isUploading;
        [ObservableProperty] private string _uploadStatus;

        public UploadViewModel()
        {
            _apiService = new ApiService();
        }

        [RelayCommand]
        public void BrowseAudio()
        {
            var dialog = new OpenFileDialog { Filter = "Audio Files|*.mp3;*.wav;*.flac" };
            if (dialog.ShowDialog() == true)
            {
                AudioPath = dialog.FileName;
            }
        }

        [RelayCommand]
        public void BrowseImage()
        {
            var dialog = new OpenFileDialog { Filter = "Image Files|*.jpg;*.png;*.jpeg" };
            if (dialog.ShowDialog() == true)
            {
                ImagePath = dialog.FileName;
            }
        }

        [RelayCommand]
        public async Task Upload()
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(AudioPath) || string.IsNullOrEmpty(ImagePath))
            {
                MessageBox.Show("Vui lòng nhập tên bài hát, chọn file nhạc và ảnh bìa.");
                return;
            }

            IsUploading = true;
            UploadStatus = "Đang xử lý và upload...";

            try
            {
                // Gọi hàm Upload đặc biệt trong ApiService
                bool success = await _apiService.UploadAsync(
                    "/api/artist/upload",
                    Title,
                    AudioPath,
                    ImagePath
                );

                if (success)
                {
                    MessageBox.Show("Upload thành công!");
                    // Reset form
                    Title = "";
                    AudioPath = "";
                    ImagePath = "";
                    UploadStatus = "";
                }
                else
                {
                    MessageBox.Show("Upload thất bại. Vui lòng thử lại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
            finally
            {
                IsUploading = false;
            }
        }
    }
}