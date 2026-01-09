using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Threading.Tasks;

namespace RX_Client_WF.Services
{
    public class MashupService
    {
        private IWavePlayer _outputDevice;
        private MixingSampleProvider _mixer;
        
        // Nguồn âm thanh
        private MediaFoundationReader _readerBeat;
        private MediaFoundationReader _readerVocal;

        // Bộ điều chỉnh âm lượng
        private VolumeSampleProvider _volBeat;
        private VolumeSampleProvider _volVocal;

        public event EventHandler PlaybackStopped;

        public bool IsPlaying => _outputDevice != null && _outputDevice.PlaybackState == PlaybackState.Playing;

        public MashupService()
        {
            _outputDevice = new WaveOutEvent();
            ((WaveOutEvent)_outputDevice).PlaybackStopped += (s, e) => PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        public async Task LoadAndPlay(string urlBeat, string urlVocal)
        {
            Stop(); // Reset trước khi chơi mới

            await Task.Run(() =>
            {
                try
                {
                    // 1. Khởi tạo Reader từ URL
                    _readerBeat = new MediaFoundationReader(urlBeat);
                    _readerVocal = new MediaFoundationReader(urlVocal);

                    // 2. Chuyển đổi định dạng về chuẩn chung (44.1kHz, Stereo, Float)
                    // Bắt buộc phải giống nhau thì Mixer mới chịu
                    var beatSample = EnsureFormat(_readerBeat);
                    var vocalSample = EnsureFormat(_readerVocal);

                    // 3. Bọc trong Volume Provider để chỉnh to nhỏ từng track
                    _volBeat = new VolumeSampleProvider(beatSample) { Volume = 1.0f };
                    _volVocal = new VolumeSampleProvider(vocalSample) { Volume = 1.0f };

                    // 4. Tạo Mixer
                    _mixer = new MixingSampleProvider(new[] { _volBeat, _volVocal });

                    // 5. Init thiết bị
                    _outputDevice.Init(_mixer);
                    _outputDevice.Play();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"Lỗi trộn nhạc: {ex.Message}");
                }
            });
        }

        // Hàm chuẩn hóa định dạng (Resample về 44100Hz Stereo)
        private ISampleProvider EnsureFormat(WaveStream stream)
        {
            var waveProvider = stream.ToSampleProvider();
            
            // Nếu khác SampleRate, dùng WdlResamplingSampleProvider để convert
            if (waveProvider.WaveFormat.SampleRate != 44100)
            {
                waveProvider = new WdlResamplingSampleProvider(waveProvider, 44100);
            }
            
            // Nếu Mono, chuyển sang Stereo
            if (waveProvider.WaveFormat.Channels == 1)
            {
                waveProvider = new MultiplexingSampleProvider(new[] { waveProvider }, 2);
            }

            return waveProvider;
        }

        public void Pause()
        {
            if (_outputDevice?.PlaybackState == PlaybackState.Playing)
                _outputDevice.Pause();
        }

        public void Resume()
        {
            if (_outputDevice?.PlaybackState == PlaybackState.Paused)
                _outputDevice.Play();
        }

        public void Stop()
        {
            _outputDevice?.Stop();
            
            // Dispose ressources
            _readerBeat?.Dispose();
            _readerVocal?.Dispose();
            _readerBeat = null;
            _readerVocal = null;
        }

        public void SetVolumeBeat(float vol) // 0.0 -> 1.0
        {
            if (_volBeat != null) _volBeat.Volume = vol;
        }

        public void SetVolumeVocal(float vol) // 0.0 -> 1.0
        {
            if (_volVocal != null) _volVocal.Volume = vol;
        }
    }
}
