using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RX_Client_WPF.Services;
using Shared.DTOs;
using System.Windows.Threading;

namespace RX_Client_WPF.ViewModels
{
    public partial class PlayerViewModel : ObservableObject
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly DispatcherTimer _timer; // Timer để cập nhật thanh chạy nhạc

        [ObservableProperty]
        private SongDto _currentSong;

        [ObservableProperty]
        private bool _isPlaying;

        [ObservableProperty]
        private double _currentPositionSeconds; // Vị trí hiện tại (giây)

        [ObservableProperty]
        private double _totalDurationSeconds; // Tổng thời lượng (giây)

        [ObservableProperty]
        private double _volume = 0.8; // Mặc định 80%

        [ObservableProperty]
        private string _currentTimeString = "00:00";

        [ObservableProperty]
        private string _totalTimeString = "00:00";

        public PlayerViewModel()
        {
            _audioPlayer = new AudioPlayer();

            // Thiết lập Timer cập nhật UI mỗi giây
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Cập nhật thanh seekbar theo thời gian thực từ AudioPlayer
            if (_audioPlayer.IsPlaying)
            {
                CurrentPositionSeconds = _audioPlayer.CurrentTime.TotalSeconds;
                CurrentTimeString = _audioPlayer.CurrentTime.ToString(@"mm\:ss");
            }
        }

        // Hàm này được gọi từ HomeViewModel khi user click vào một bài hát
        public async Task PlaySong(SongDto song)
        {
            CurrentSong = song;

            // 1. Gọi AudioPlayer phát nhạc từ URL
            // (Giả sử bạn có hàm GetStreamUrl trong Config hoặc ApiService)
            string streamUrl = $"http://localhost:5000/api/songs/{song.Id}/stream";

            await _audioPlayer.PlayUrlAsync(streamUrl);

            // 2. Cập nhật trạng thái UI
            IsPlaying = true;
            TotalDurationSeconds = song.Duration; // Hoặc lấy từ AudioPlayer sau khi load xong
            TotalTimeString = TimeSpan.FromSeconds(song.Duration).ToString(@"mm\:ss");

            _timer.Start();
        }

        [RelayCommand]
        public void TogglePlayPause()
        {
            if (IsPlaying)
            {
                _audioPlayer.Pause();
                IsPlaying = false;
                _timer.Stop();
            }
            else
            {
                _audioPlayer.Resume();
                IsPlaying = true;
                _timer.Start();
            }
        }

        [RelayCommand]
        public void StopMusic()
        {
            _audioPlayer.Stop();
            IsPlaying = false;
            _timer.Stop();
            CurrentPositionSeconds = 0;
            CurrentTimeString = "00:00";
        }

        // Lệnh khi người dùng kéo thanh seekbar
        [RelayCommand]
        public void SeekTo(double seconds)
        {
            _audioPlayer.Seek(TimeSpan.FromSeconds(seconds));
            CurrentPositionSeconds = seconds;
        }

        // Khi Volume thay đổi trên UI
        partial void OnVolumeChanged(double value)
        {
            _audioPlayer.SetVolume((float)value);
        }
    }
}