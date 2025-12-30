using Guna.UI2.WinForms;
using Shared.DTOs;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCPlayerBar : UserControl
    {
        // Các sự kiện để MainForm đăng ký
        public event EventHandler PlayClicked;
        public event EventHandler PauseClicked;
        public event EventHandler NextClicked;
        public event EventHandler PrevClicked;
        public event Action<int> SeekChanged;
        public event Action<int> VolumeChanged;
        public event EventHandler SongInfoClick; // Mo Now Playing
        public event EventHandler KaraokeToggled; // New Event

        private bool _isPlaying = false;
        private bool _isReplayMode = false;

        public UCPlayerBar()
        {
            InitializeComponent();

            // Gán sự kiện Click
            btnPlay.Click += (s, e) => TogglePlayPause();
            btnNext.Click += (s, e) => NextClicked?.Invoke(this, EventArgs.Empty);
            btnPrev.Click += (s, e) => PrevClicked?.Invoke(this, EventArgs.Empty);

            // Logic Karaoke Toggle
            if (btnKaraoke != null)
            {
                btnKaraoke.Click += (s, e) =>
                {
                    if (btnKaraoke.FillColor == Color.Transparent)
                    {
                        // Enable Karaoke (Active Mode - High Contrast)
                        btnKaraoke.FillColor = Color.White; // White Background
                        btnKaraoke.ForeColor = Color.Black; // Black Text
                    }
                    else
                    {
                        // Disable Karaoke (Inactive Mode)
                        btnKaraoke.FillColor = Color.Transparent;
                        btnKaraoke.ForeColor = Color.White;
                    }
                    KaraokeToggled?.Invoke(this, EventArgs.Empty);
                };
            }

            trackBarTime.Scroll += (s, e) => SeekChanged?.Invoke(trackBarTime.Value);
            trackBarVolume.Scroll += (s, e) => VolumeChanged?.Invoke(trackBarVolume.Value);

            // Click vao thong tin bai hat de mo man hinh Now Playing
            picCover.Cursor = Cursors.Hand;
            lblTitle.Cursor = Cursors.Hand;
            lblArtist.Cursor = Cursors.Hand;
            
            EventHandler openNowPlaying = (s, e) => SongInfoClick?.Invoke(this, EventArgs.Empty);

            picCover.Click += openNowPlaying;
            lblTitle.Click += openNowPlaying;
            lblArtist.Click += openNowPlaying;

            // Tự động căn giữa lại các nút khi kích thước thay đổi
            this.Resize += UCPlayerBar_Resize;
        }

        // Logic căn giữa các nút điều khiển
        // Logic căn giữa các nút điều khiển
        private void UCPlayerBar_Resize(object sender, EventArgs e)
        {
            if (this.Width == 0) return;

            int center = this.Width / 2;

            // 1. CĂN GIỮA: Play, Prev, Next, Shuffle, Repeat
            btnPlay.Location = new Point(center - 20, 10);
            btnPrev.Location = new Point(center - 70, 12);
            btnNext.Location = new Point(center + 30, 12);
            btnShuffle.Location = new Point(center - 110, 15);
            btnRepeat.Location = new Point(center + 75, 15);

            // 2. CĂN GIỮA (DƯỚI): Thanh thời gian
            // Giảm độ rộng để tránh đè lên Karaoke (35% thay vì 40%)
            int trackWidth = Math.Min(500, (int)(this.Width * 0.35)); 
            if (trackWidth < 200) trackWidth = 200;

            trackBarTime.Size = new Size(trackWidth, 23);
            trackBarTime.Location = new Point(center - (trackWidth / 2), 55);

            lblCurrentTime.Location = new Point(trackBarTime.Left - 45, 58);
            lblTotalTime.Location = new Point(trackBarTime.Right + 10, 58);

            // 3. CĂN PHẢI: Karaoke + Volume
            int rightMargin = 30;
            
            if (trackBarVolume != null)
            {
                trackBarVolume.Location = new Point(this.Width - trackBarVolume.Width - rightMargin, 35);
                
                if (lblVolumeIcon != null)
                {
                    lblVolumeIcon.Location = new Point(trackBarVolume.Left - 35, 32);
                }

                if (btnKaraoke != null)
                {
                    // Dịch sang trái (Gap 15px so với Volume)
                    int karaokeX = (lblVolumeIcon != null ? lblVolumeIcon.Left : trackBarVolume.Left) - btnKaraoke.Width - 15;
                    btnKaraoke.Location = new Point(karaokeX, 29);
                }
            }
        }

        public void UpdateSongInfo(SongDto song)
        {
            if (song == null) return;

            lblTitle.Text = song.Title;
            lblArtist.Text = song.ArtistName;

            if (!string.IsNullOrEmpty(song.ThumbnailUrl))
            {
                // Dùng LoadAsync để không đơ giao diện
                try { picCover.LoadAsync(song.ThumbnailUrl); } catch { }
            }
            else
            {
                picCover.Image = null;
                picCover.FillColor = Color.Gray;
            }

            trackBarTime.Value = 0;
            // Tranh loi Maximum < Minimum khi duration = 0
            trackBarTime.Maximum = song.Duration > 0 ? (int)song.Duration : 1; 
            lblTotalTime.Text = TimeSpan.FromSeconds(song.Duration).ToString(@"mm\:ss");

            SetPlayingState(true);
        }

        public void UpdateProgress(TimeSpan current, TimeSpan total)
        {
            if (current.TotalSeconds <= trackBarTime.Maximum)
            {
                trackBarTime.Value = (int)current.TotalSeconds;
            }
            lblCurrentTime.Text = current.ToString(@"mm\:ss");
        }

        public void SetPlayingState(bool isPlaying)
        {
            _isPlaying = isPlaying;
            _isReplayMode = false; // Reset replay mode

            if (_isPlaying)
            {
                btnPlay.Text = "⏸"; // Pause icon
                btnPlay.ImageOffset = new Point(0, 0);
            }
            else
            {
                btnPlay.Text = "▶";  // Play icon
                btnPlay.ImageOffset = new Point(2, 0);
            }
        }

        public void ShowReplayButton()
        {
            _isPlaying = false;
            _isReplayMode = true;
            btnPlay.Text = "↺"; // Replay icon
            btnPlay.ImageOffset = new Point(0, 0);
        }

        private void TogglePlayPause()
        {
            if (_isReplayMode)
            {
                PlayClicked?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (_isPlaying)
            {
                PauseClicked?.Invoke(this, EventArgs.Empty);
                SetPlayingState(false);
            }
            else
            {
                PlayClicked?.Invoke(this, EventArgs.Empty);
                SetPlayingState(true);
            }
        }
    }
}