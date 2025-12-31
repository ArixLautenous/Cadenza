using NAudio.Wave;
using System;
using System.Threading.Tasks;
using System.Windows.Forms; // Dùng Timer của WinForms

namespace RX_Client_WF.Services
{
    public class AudioPlayer : IDisposable
    {
        private IWavePlayer _outputDevice;
        private WaveStream _audioFile;
        private EqualizerProvider _equalizer;
        public event EventHandler PlaybackStopped;
        private bool _suppressStopEvent = false;

        // Timer này dành riêng cho WinForms để cập nhật UI
        public System.Windows.Forms.Timer PlaybackTimer { get; private set; }

        public TimeSpan CurrentTime => _audioFile?.CurrentTime ?? TimeSpan.Zero;
        public TimeSpan TotalTime => _audioFile?.TotalTime ?? TimeSpan.Zero;
        public bool IsPlaying => _outputDevice?.PlaybackState == PlaybackState.Playing;

        public AudioPlayer()
        {
            _outputDevice = new WaveOutEvent();
            _outputDevice.PlaybackStopped += (s, e) => 
            {
                if (!_suppressStopEvent) PlaybackStopped?.Invoke(this, EventArgs.Empty);
            };

            // Khởi tạo Timer cập nhật tiến độ (1 giây 1 lần)
            PlaybackTimer = new System.Windows.Forms.Timer();
            PlaybackTimer.Interval = 1000;
        }
        
        // API chỉnh EQ
        public void SetEq(int bandIndex, float gain)
        {
            _equalizer?.UpdateGain(bandIndex, gain);
        }

        public async Task PlayUrlAsync(string url)
        {
            _suppressStopEvent = true;
            Stop(); // Dừng bài cũ trước
            _suppressStopEvent = false;

            try
            {
                // 1. Kiểm tra URL có hợp lệ không (tránh lỗi 404 gây crash COM)
                // 1. Kiểm tra URL có hợp lệ không
                // Dùng GET + ResponseHeadersRead thay vì HEAD để tránh lỗi 405 MethodNotAllowed
                // 1. Kiểm tra URL (Optional Check)
                // Nếu Server đang bận, request này có thể timeout. Ta chỉ cảnh báo, KHÔNG chặn phát nhạc.
                try
                {
                    using (var client = new System.Net.Http.HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(2); // Timeout nhanh (2s)
                        var response = await client.GetAsync(url, System.Net.Http.HttpCompletionOption.ResponseHeadersRead);
                        
                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"[Warning] Server check: {response.StatusCode}");
                        }
                    }
                }
                catch
                {
                     // Ignore check errors
                }

                // 2. Khởi tạo Player (Bỏ Task.Run để tránh lỗi Threading với COM Object)
                _audioFile = new MediaFoundationReader(url);

                // --- EQ PIPELINE ---
                var sampleProvider = _audioFile.ToSampleProvider();
                _equalizer = new EqualizerProvider(sampleProvider);
                
                _outputDevice.Init(_equalizer);
                _outputDevice.Play();

                PlaybackTimer.Start(); // Bắt đầu đếm giờ
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi phát nhạc: {ex.Message}\nURL: {url}");
            }
        }

        public void Pause()
        {
            _outputDevice?.Pause();
            PlaybackTimer.Stop();
        }

        public void Resume()
        {
            if (_audioFile == null || _outputDevice == null) return;

            try 
            {
                // Nếu đã chạy hết bài, tua về đầu trước khi Resume
                if (_audioFile.CurrentTime >= _audioFile.TotalTime)
                {
                    _audioFile.Position = 0;
                }

                _outputDevice.Play();
                PlaybackTimer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Resume Error: {ex.Message}");
            }
        }

        public void Stop()
        {
            _outputDevice?.Stop();
            PlaybackTimer.Stop();

            if (_audioFile != null)
            {
                _audioFile.Dispose();
                _audioFile = null;
            }
        }

        // Hàm tua nhạc
        public void Seek(double seconds)
        {
            if (_audioFile != null)
            {
                // Giới hạn không tua quá độ dài bài
                if (seconds > _audioFile.TotalTime.TotalSeconds)
                    seconds = _audioFile.TotalTime.TotalSeconds - 1;

                _audioFile.CurrentTime = TimeSpan.FromSeconds(seconds);
            }
        }

        public void SetVolume(int value) // Nhận vào 0-100
        {
            if (_outputDevice != null)
            {
                _outputDevice.Volume = value / 100f;
            }
        }

        public void Dispose()
        {
            Stop();
            _outputDevice?.Dispose();
            PlaybackTimer?.Dispose();
        }
    }
}