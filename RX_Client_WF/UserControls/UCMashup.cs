using Guna.UI2.WinForms;
using RX_Client_WF.Services;
using RX_Client_WF.Utils;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCMashup : UserControl
    {
        private MashupService _mashupService;
        private ApiService _apiService;
        
        // UI Left (Beat)
        private Guna2ComboBox cbBeat;
        private Guna2TrackBar tbVolBeat;
        private Label lblBeatStatus;

        // UI Right (Vocal)
        private Guna2ComboBox cbVocal;
        private Guna2TrackBar tbVolVocal;
        private Label lblVocalStatus;

        // UI Center
        private Guna2Button btnPlay;
        private Guna2Button btnStop;
        private Guna2Button btnLoad;

        private List<SongDto> _allSongs;

        public UCMashup()
        {
            InitializeComponent();
            _mashupService = new MashupService();
            _apiService = new ApiService();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 600);
            this.BackColor = Color.FromArgb(15, 15, 15);

            // Title
            var lblTitle = new Label 
            { 
                Text = "PHÒNG THÍ NGHIỆM MASHUP", 
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            var lblSubtitle = new Label
            {
                Text = "Kết hợp giai điệu của bài này với giọng hát của bài khác!",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(35, 65)
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);

            // --- PANEL BEAT (LEFT) ---
            var pnBeat = CreateDeskPanel("NHẠC NỀN (BEAT)", Color.FromArgb(0, 100, 200), 50, out cbBeat, out tbVolBeat);
            lblBeatStatus = new Label { Text = "Sẵn sàng", ForeColor = Color.Gray, Location = new Point(20, 250), AutoSize = true };
            pnBeat.Controls.Add(lblBeatStatus);

            // --- PANEL VOCAL (RIGHT) ---
            var pnVocal = CreateDeskPanel("GIỌNG HÁT (VOCAL)", Color.FromArgb(200, 50, 100), 550, out cbVocal, out tbVolVocal);
            lblVocalStatus = new Label { Text = "Sẵn sàng", ForeColor = Color.Gray, Location = new Point(20, 250), AutoSize = true };
            pnVocal.Controls.Add(lblVocalStatus);

            this.Controls.Add(pnBeat);
            this.Controls.Add(pnVocal);

            // --- CONTROLS (CENTER DOWN) ---
            btnLoad = new Guna2Button
            {
                Text = "TRỘN & PHÁT",
                Size = new Size(200, 50),
                Location = new Point(400, 450),
                FillColor = Color.FromArgb(40, 40, 40),
                BorderRadius = 25,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnLoad.Click += BtnLoad_Click;

            btnStop = new Guna2Button
            {
                Text = "DỪNG",
                Size = new Size(100, 40),
                Location = new Point(450, 520),
                FillColor = Color.FromArgb(150, 20, 20),
                BorderRadius = 20
            };
            btnStop.Click += (s, e) => { _mashupService.Stop(); lblBeatStatus.Text = "Đã dừng"; lblVocalStatus.Text = "Đã dừng"; };

            this.Controls.Add(btnLoad);
            this.Controls.Add(btnStop);

            // Events Volume
             tbVolBeat.Scroll += (s, e) => _mashupService.SetVolumeBeat(tbVolBeat.Value / 100f);
             tbVolVocal.Scroll += (s, e) => _mashupService.SetVolumeVocal(tbVolVocal.Value / 100f);
        }

        private Guna2Panel CreateDeskPanel(string title, Color accentColor, int x, out Guna2ComboBox cb, out Guna2TrackBar vol)
        {
            var p = new Guna2Panel
            {
                Size = new Size(400, 300),
                Location = new Point(x, 120),
                FillColor = Color.FromArgb(25, 25, 25),
                BorderRadius = 10,
                BorderColor = accentColor,
                BorderThickness = 2
            };

            var lbl = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            p.Controls.Add(lbl);

            cb = new Guna2ComboBox
            {
                Location = new Point(20, 70),
                Width = 360,
                Height = 40,
                FillColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                DisplayMember = "DisplayTitle",
                ValueMember = "Id" 
            };
            p.Controls.Add(cb);

            var lblVol = new Label { Text = "Âm lượng:", ForeColor = Color.Gray, Location = new Point(20, 140), AutoSize = true };
            p.Controls.Add(lblVol);

            vol = new Guna2TrackBar
            {
                Location = new Point(20, 170),
                Width = 360,
                Value = 100,
                ThumbColor = accentColor
            };
            p.Controls.Add(vol);

            return p;
        }

        private async void LoadData()
        {
             try
             {
                 var songs = await _apiService.GetAsync<List<SongDto>>("/api/songs");
                 if (songs != null)
                 {
                     _allSongs = songs.Where(s => !string.IsNullOrEmpty(s.InstrumentalUrl) && !string.IsNullOrEmpty(s.VocalUrl)).ToList();
                     
                     // Helper Class de hien thi dep hon
                     var comboSource = _allSongs.Select(s => new { Id = s.Id, DisplayTitle = $"{s.Title} - {s.ArtistName}", Data = s }).ToList();

                     cbBeat.DataSource = new List<object>(comboSource);
                     cbBeat.DisplayMember = "DisplayTitle";
                     cbBeat.ValueMember = "Id";

                     cbVocal.DataSource = new List<object>(comboSource);
                     cbVocal.DisplayMember = "DisplayTitle";
                     cbVocal.ValueMember = "Id";     
                 }
             }
             catch { }
        }

        private async void BtnLoad_Click(object sender, EventArgs e)
        {
            if (cbBeat.SelectedItem == null || cbVocal.SelectedItem == null) 
            {
                MessageBox.Show("Vui lòng chọn 2 bài hát!");
                return;
            }

            // Lay Object that tu ComboBox (Do dung Anonymous Type wrapper)
            dynamic itemBeat = cbBeat.SelectedItem;
            dynamic itemVocal = cbVocal.SelectedItem;

            SongDto songBeat = itemBeat.Data;
            SongDto songVocal = itemVocal.Data;

            // Xay dung URL tuyet doi neu can
            // InstrumentalUrl va VocalUrl tu API da la URL day du hoac tuong doi?
            // Test trong SongsController thi la tuong doi "audio/separated/..." chua co host?
            // Check lai SongsController: 
            // no, SongsController.GetAll dang tra ve Clean URL neu logic cu, nhung logic moi la Demucs path.
            // Hay check SongsController Step 959: no tra ve s.InstrumentUrl truc tiep tu DB.
            // DB luu "separated/htdemucs/..." hay full url? Code python in ra path local.
            // Service C# parse path local -> Save to DB?
            
            // Can than: AudioProcessingService save local path vao DB. Web can URL.
            // Ta can fix Path -> URL o day hoac o API.
            // Let's assume API needs to return URL.
            
            string urlBeat = ResolveUrl(songBeat.InstrumentalUrl);
            string urlVocal = ResolveUrl(songVocal.VocalUrl);

            lblBeatStatus.Text = $"Đang tải: {songBeat.Title} (Beat)...";
            lblVocalStatus.Text = $"Đang tải: {songVocal.Title} (Vocals)...";
            btnLoad.Enabled = false;

            await _mashupService.LoadAndPlay(urlBeat, urlVocal);

            lblBeatStatus.Text = "Đang phát";
            lblVocalStatus.Text = "Đang phát";
            btnLoad.Enabled = true;
        }

        private string ResolveUrl(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath)) return "";
            if (dbPath.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return dbPath;
            
            // Fix input path processing
            string normalizedPath = dbPath.Replace("\\", "/");

            // Truong hop 1: Path tuyet doi chua wwwroot (Legacy issues)
            if (normalizedPath.Contains("wwwroot"))
            {
                int idx = normalizedPath.IndexOf("wwwroot", StringComparison.OrdinalIgnoreCase);
                string relative = normalizedPath.Substring(idx + 7); // 7 is len of wwwroot
                return $"{Config.BaseUrl}/{relative.TrimStart('/')}";
            }

            // Truong hop 2: Path tuong doi dep (audio/separated/...)
            return $"{Config.BaseUrl}/{normalizedPath.TrimStart('/')}";
        }
    }
}
