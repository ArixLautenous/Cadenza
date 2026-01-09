using NAudio.Wave;
using System;
using System.Threading.Tasks;

namespace RX_Client.Services
{
    public class AudioPlayer : IDisposable
    {
        private IWavePlayer _outputDevice;
        private WaveStream _audioFile;
        private float _currentVolume = 0.8f; // Mặc định 80%

        // Sự kiện khi nhạc hết bài (để tự chuyển bài tiếp theo)
        public event EventHandler PlaybackStopped;

        public bool IsPlaying => _outputDevice?.PlaybackState == PlaybackState.Playing;

        // Thời gian hiện tại của bài hát
        public TimeSpan CurrentTime => _audioFile?.CurrentTime ?? TimeSpan.Zero;

        // Tổng thời lượng
        public TimeSpan TotalTime => _audioFile?.TotalTime ?? TimeSpan.Zero;

        public AudioPlayer()
        {
            // Khởi tạo thiết bị output (Loa/Tai nghe mặc định của Windows)
            _outputDevice = new WaveOutEvent();
            _outputDevice.PlaybackStopped += OnPlaybackStopped;
        }

        // Hàm quan trọng: Stream nhạc từ URL (Server)
        public async Task PlayUrlAsync(string url)
        {
            Stop(); // Dừng bài cũ nếu đang chạy

            try
            {
                // Chạy task background để việc load file không làm đơ giao diện WPF
                await Task.Run(() =>
                {
                    // MediaFoundationReader là phép màu của NAudio:
                    // Nó đọc được URL HTTP và tự động stream (vừa tải vừa nghe)
                    // Nó hỗ trợ MP3, AAC, WMA, và cả FLAC (trên Win 10+)
                    _audioFile = new MediaFoundationReader(url);
                });

                // Kết nối file nhạc vào loa
                _outputDevice.Init(_audioFile);
                _outputDevice.Volume = _currentVolume;
                _outputDevice.Play();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: mất mạng, link sai)
                Console.WriteLine($"Lỗi phát nhạc: {ex.Message}");
            }
        }

        public void Pause()
        {
            if (_outputDevice?.PlaybackState == PlaybackState.Playing)
            {
                _outputDevice.Pause();
            }
        }

        public void Resume()
        {
            if (_outputDevice?.PlaybackState == PlaybackState.Paused)
            {
                _outputDevice.Play();
            }
        }

        public void Stop()
        {
            _outputDevice?.Stop();

            // Giải phóng file stream cũ để tiết kiệm RAM
            if (_audioFile != null)
            {
                _audioFile.Dispose();
                _audioFile = null;
            }
        }

        public void Seek(TimeSpan time)
        {
            if (_audioFile != null)
            {
                // Kiểm tra giới hạn để không tua quá độ dài bài hát
                if (time > _audioFile.TotalTime) time = _audioFile.TotalTime;
                if (time < TimeSpan.Zero) time = TimeSpan.Zero;

                _audioFile.CurrentTime = time;
            }
        }

        public void SetVolume(float volume)
        {
            // Volume chạy từ 0.0f đến 1.0f
            if (volume < 0) volume = 0;
            if (volume > 1) volume = 1;

            _currentVolume = volume;
            if (_outputDevice != null)
            {
                _outputDevice.Volume = _currentVolume;
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            // Bắn sự kiện ra ngoài để ViewModel biết (ví dụ để Next bài)
            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            Stop();
            _outputDevice?.Dispose();
        }
    }
}
