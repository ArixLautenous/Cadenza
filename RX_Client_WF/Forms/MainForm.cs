using Guna.UI2.WinForms;
using RX_Client_WF.Services;
using RX_Client_WF.UserControls;
using RX_Client_WF.Utils;
using Shared.DTOs;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    public partial class MainForm : Form
    {
        private AudioPlayer _player;

        // Cache lại các View (UserControl) để không phải load lại
        private UCHome _ucHome;
        private UCLibrary _ucLibrary;
        private UCUserProfile _ucProfile;
        private UCNowPlaying _ucNowPlaying;
        private bool _isSwitchingSongs = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeLogic();
        }

        private void InitializeLogic()
        {
            _player = new AudioPlayer();

            // 1. Kết nối PlayerBar với AudioPlayer
            playerBar.PlayClicked += (s, e) => _player.Resume();
            playerBar.PauseClicked += (s, e) => _player.Pause();
            playerBar.SeekChanged += (seconds) => _player.Seek(seconds);
            playerBar.VolumeChanged += (vol) => _player.SetVolume(vol);
            playerBar.SongInfoClick += (s, e) => ShowNowPlaying();

            // Timer: Cập nhật thanh thời gian mỗi giây
            _player.PlaybackTimer.Tick += (s, e) =>
            {
                if (_player.IsPlaying)
                    playerBar.UpdateProgress(_player.CurrentTime, _player.TotalTime);
            };

            // Sự kiện khi nhạc dừng hẳn (Hết bài hoặc Stop)
            _player.PlaybackStopped += (s, e) =>
            {
                // Nếu đang chủ động chuyển bài thì bỏ qua sự kiện Stop này
                if (_isSwitchingSongs) return;

                // Khi dừng, chuyển trạng thái về Replay
                // Invoke để đảm bảo chạy trên UI Thread
                this.Invoke((MethodInvoker)delegate 
                {
                    playerBar.ShowReplayButton();
                });
            };

            // 2. Xử lý Sidebar Menu
            btnHome.Click += (s, e) => ShowHome();
            btnLibrary.Click += (s, e) => ShowLibrary();
            btnProfile.Click += (s, e) => ShowProfile(); // Logic mới
            btnUpload.Click += (s, e) => OpenUploadForm();
            btnMashup.Click += (s, e) => ShowMashup();
            btnSettings.Click += (s, e) => ShowSettings();
            btnLogout.Click += BtnLogout_Click;

            // 3. Phân quyền Artist
            if (Session.IsArtist)
            {
                btnUpload.Visible = true;
            }

            // 4. Hiệu ứng Hover
            AddHoverEffect(btnHome);
            AddHoverEffect(btnLibrary);
            AddHoverEffect(btnProfile);
            AddHoverEffect(btnUpload);
            AddHoverEffect(btnMashup);
            AddHoverEffect(btnSettings);

            // 5. Mặc định vào trang chủ
            ShowHome();

            // 6. Search Logic
            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    PerformSearch(txtSearch.Text);
                    e.SuppressKeyPress = true; // Prevent 'ding' sound
                }
            };

            // 7. Dynamic Center Search Bar
            this.Resize += (s, e) => CenterSearchBar();
            CenterSearchBar(); // Call immediately
        }

        private void CenterSearchBar()
        {
            if (pnHeader != null && txtSearch != null)
            {
                txtSearch.Location = new Point((pnHeader.Width - txtSearch.Width) / 2, 10);
            }
        }

        private void PerformSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;
            
            var ucSearch = new UCSearchResults(query);
            ucSearch.SongClicked += PlayMusic; // Cho phep play truc tiep tu search
            
            LoadView(ucSearch);
            SetActiveButton(null); // Clear active menu
        }

        // --- NAVIGATION LOGIC ---

        private void ShowHome()
        {
            if (_ucHome == null)
            {
                _ucHome = new UCHome();
                // Khi click bài hát ở Home -> Phát nhạc ngay
                _ucHome.SongClicked += PlayMusic;
            }
            LoadView(_ucHome);
            SetActiveButton(btnHome);
        }

        private void ShowLibrary()
        {
            if (_ucLibrary == null)
            {
                _ucLibrary = new UCLibrary();
            }
            _ucLibrary.LoadPlaylists(); // Reload dữ liệu mới nhất
            LoadView(_ucLibrary);
            SetActiveButton(btnLibrary);
        }

        private void ShowProfile()
        {
            // Profile nên load lại mỗi lần mở để cập nhật gói cước nếu vừa mua xong
            _ucProfile = new UCUserProfile();
            _ucProfile.SongClicked += PlayMusic;
            LoadView(_ucProfile);
            SetActiveButton(btnProfile);
        }

        private void ShowSettings()
        {
            var uc = new UCSettings(_player);
            uc.LogoutClicked += BtnLogout_Click;
            LoadView(uc);
            SetActiveButton(btnSettings);
        }

        private void ShowMashup()
        {
            // Stop Main Player neu vao Mashup de tranh lan tieng
            _player.Pause();
            playerBar.SetPlayingState(false);
            
            var uc = new UCMashup();
            LoadView(uc);
            SetActiveButton(btnMashup);
        }

        private void LoadView(UserControl uc)
        {
            pnContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnContent.Controls.Add(uc);
        }

        // --- AUDIO LOGIC ---

        private async void PlayMusic(SongDto song)
        {
            if (song == null) return;
            
            _isSwitchingSongs = true; // Bắt đầu chuyển (chặn Stopping event)

            try
            {
                // Cập nhật giao diện PlayerBar
                playerBar.UpdateSongInfo(song);

                // Cap nhat Now Playing UI
                if (_ucNowPlaying == null) _ucNowPlaying = new UCNowPlaying();
                _ucNowPlaying.SetSong(song);

                // Gọi API Stream
                string url = $"{Config.BaseUrl}/api/songs/{song.Id}/stream?access_token={Session.Token}";

                await _player.PlayUrlAsync(url);
                
                // Quan trọng: Cập nhật nút Play/Pause
                playerBar.SetPlayingState(true);

                // --- FETCH INFO MỚI NHẤT (Lyric, Karaoke) ---
                _ = Task.Run(async () => 
                {
                    try 
                    {
                        var api = new ApiService();
                        var updatedSong = await api.GetAsync<SongDto>($"/api/songs/{song.Id}");
                        if (updatedSong != null)
                        {
                            this.Invoke((MethodInvoker)delegate 
                            {
                                if (_ucNowPlaying != null) _ucNowPlaying.SetSong(updatedSong);
                            });
                        }
                    }
                    catch { }
                });
            }
            finally
            {
                // Đợi một chút để đảm bảo event cũ đã qua
                await Task.Delay(200);
                _isSwitchingSongs = false;
            }
        }

        // --- HELPER FUNCTIONS ---

        private void OpenUploadForm()
        {
            ArtistUploadForm frm = new ArtistUploadForm();
            frm.ShowDialog();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            _player.Stop();
            Session.ClearSession();

            LoginForm login = new LoginForm();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        private void ShowNowPlaying()
        {
            if (_ucNowPlaying == null) _ucNowPlaying = new UCNowPlaying();
            LoadView(_ucNowPlaying);
            SetActiveButton(null); // Tat highlight cac nut khac
        }

        private void SetActiveButton(Guna2Button activeBtn)
        {
            btnHome.FillColor = Color.Transparent;
            btnLibrary.FillColor = Color.Transparent;
            btnProfile.FillColor = Color.Transparent;
            btnUpload.FillColor = Color.Transparent;
            btnMashup.FillColor = Color.Transparent;
            btnSettings.FillColor = Color.Transparent;

            if (activeBtn != null)
                activeBtn.FillColor = Color.FromArgb(40, 40, 40); // Highlight
        }

        private void AddHoverEffect(Guna2Button btn)
        {
            btn.MouseEnter += (s, e) => { if (btn.FillColor == Color.Transparent) btn.FillColor = Color.FromArgb(25, 25, 25); };
            btn.MouseLeave += (s, e) => { if (btn.FillColor == Color.FromArgb(25, 25, 25)) btn.FillColor = Color.Transparent; };
        }
    }
}