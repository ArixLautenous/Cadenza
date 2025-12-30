using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    partial class UCPlayerBar
    {
        private System.ComponentModel.IContainer components = null;

        // UI Controls công khai để Form cha truy cập
        public Guna2PictureBox picCover;
        public Label lblTitle;
        public Label lblArtist;

        public Guna2Button btnPlay;
        public Guna2Button btnNext;
        public Guna2Button btnPrev;
        public Guna2Button btnShuffle;
        public Guna2Button btnRepeat;

        public Guna2TrackBar trackBarTime;
        public Label lblCurrentTime;
        public Label lblTotalTime;

        public Guna2TrackBar trackBarVolume;
        private Label lblVolumeIcon;
        public Guna2Button btnKaraoke; // New member

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            picCover = new Guna2PictureBox();
            lblTitle = new Label();
            lblArtist = new Label();
            btnPlay = new Guna2Button();
            btnNext = new Guna2Button();
            btnPrev = new Guna2Button();
            btnShuffle = new Guna2Button();
            btnRepeat = new Guna2Button();
            trackBarTime = new Guna2TrackBar();
            lblCurrentTime = new Label();
            lblTotalTime = new Label();
            trackBarVolume = new Guna2TrackBar();
            lblVolumeIcon = new Label();
            ((System.ComponentModel.ISupportInitialize)picCover).BeginInit();
            SuspendLayout();
            // 
            // picCover
            // 
            picCover.BorderRadius = 4;
            picCover.CustomizableEdges = customizableEdges1;
            picCover.FillColor = Color.FromArgb(50, 50, 50);
            picCover.ImageRotate = 0F;
            picCover.Location = new Point(15, 15);
            picCover.Name = "picCover";
            picCover.ShadowDecoration.CustomizableEdges = customizableEdges2;
            picCover.Size = new Size(60, 60);
            picCover.SizeMode = PictureBoxSizeMode.StretchImage;
            picCover.TabIndex = 0;
            picCover.TabStop = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(85, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(97, 20);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Chọn bài hát";
            // 
            // lblArtist
            // 
            lblArtist.AutoSize = true;
            lblArtist.Font = new Font("Segoe UI", 9F);
            lblArtist.ForeColor = Color.DarkGray;
            lblArtist.Location = new Point(85, 45);
            lblArtist.Name = "lblArtist";
            lblArtist.Size = new Size(16, 15);
            lblArtist.TabIndex = 2;
            lblArtist.Text = "...";
            // 
            // btnPlay
            // 
            btnPlay.Anchor = AnchorStyles.Top;
            btnPlay.BorderRadius = 20;
            btnPlay.CustomizableEdges = customizableEdges3;
            btnPlay.FillColor = Color.White;
            btnPlay.Font = new Font("Segoe UI Symbol", 16F, FontStyle.Bold);
            btnPlay.ForeColor = Color.Black;
            btnPlay.Location = new Point(1321, 10);
            btnPlay.Name = "btnPlay";
            btnPlay.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnPlay.Size = new Size(40, 40);
            btnPlay.TabIndex = 3;
            btnPlay.Text = "▶";
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Top;
            btnNext.BorderRadius = 4;
            btnNext.CustomizableEdges = customizableEdges5;
            btnNext.FillColor = Color.Transparent;
            btnNext.Font = new Font("Segoe UI Symbol", 12F, FontStyle.Bold);
            btnNext.ForeColor = Color.White;
            btnNext.Location = new Point(1371, 12);
            btnNext.Name = "btnNext";
            btnNext.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnNext.Size = new Size(35, 35);
            btnNext.TabIndex = 4;
            btnNext.Text = "⏭";
            // 
            // btnPrev
            // 
            btnPrev.Anchor = AnchorStyles.Top;
            btnPrev.BorderRadius = 4;
            btnPrev.CustomizableEdges = customizableEdges7;
            btnPrev.FillColor = Color.Transparent;
            btnPrev.Font = new Font("Segoe UI Symbol", 12F, FontStyle.Bold);
            btnPrev.ForeColor = Color.White;
            btnPrev.Location = new Point(1271, 12);
            btnPrev.Name = "btnPrev";
            btnPrev.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnPrev.Size = new Size(35, 35);
            btnPrev.TabIndex = 5;
            btnPrev.Text = "⏮";
            // 
            // btnShuffle
            // 
            btnShuffle.Anchor = AnchorStyles.Top;
            btnShuffle.BorderRadius = 4;
            btnShuffle.CustomizableEdges = customizableEdges9;
            btnShuffle.FillColor = Color.Transparent;
            btnShuffle.Font = new Font("Segoe UI Symbol", 12F);
            btnShuffle.ForeColor = Color.Gray;
            btnShuffle.Location = new Point(1231, 15);
            btnShuffle.Name = "btnShuffle";
            btnShuffle.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnShuffle.Size = new Size(30, 30);
            btnShuffle.TabIndex = 6;
            btnShuffle.Text = "🔀";
            // 
            // btnRepeat
            // 
            btnRepeat.Anchor = AnchorStyles.Top;
            btnRepeat.BorderRadius = 4;
            btnRepeat.CustomizableEdges = customizableEdges11;
            btnRepeat.FillColor = Color.Transparent;
            btnRepeat.Font = new Font("Segoe UI Symbol", 12F);
            btnRepeat.ForeColor = Color.Gray;
            btnRepeat.Location = new Point(1416, 15);
            btnRepeat.Name = "btnRepeat";
            btnRepeat.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnRepeat.Size = new Size(30, 30);
            btnRepeat.TabIndex = 7;
            btnRepeat.Text = "🔁";
            // 
            // trackBarTime
            // 
            trackBarTime.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trackBarTime.FillColor = Color.Gray;
            trackBarTime.Location = new Point(300, 55);
            trackBarTime.Name = "trackBarTime";
            trackBarTime.Size = new Size(2082, 23);
            trackBarTime.TabIndex = 8;
            trackBarTime.ThumbColor = Color.Red;
            trackBarTime.Value = 0;
            // 
            // lblCurrentTime
            // 
            lblCurrentTime.Anchor = AnchorStyles.Top;
            lblCurrentTime.AutoSize = true;
            lblCurrentTime.ForeColor = Color.Gray;
            lblCurrentTime.Location = new Point(1101, 58);
            lblCurrentTime.Name = "lblCurrentTime";
            lblCurrentTime.Size = new Size(34, 15);
            lblCurrentTime.TabIndex = 9;
            lblCurrentTime.Text = "00:00";
            // 
            // lblTotalTime
            // 
            lblTotalTime.Anchor = AnchorStyles.Top;
            lblTotalTime.AutoSize = true;
            lblTotalTime.ForeColor = Color.Gray;
            lblTotalTime.Location = new Point(1551, 58);
            lblTotalTime.Name = "lblTotalTime";
            lblTotalTime.Size = new Size(34, 15);
            lblTotalTime.TabIndex = 10;
            lblTotalTime.Text = "00:00";
            // 
            // trackBarVolume
            // 
            trackBarVolume.Anchor = AnchorStyles.Right;
            trackBarVolume.Location = new Point(2552, 35);
            trackBarVolume.Name = "trackBarVolume";
            trackBarVolume.Size = new Size(100, 23);
            trackBarVolume.TabIndex = 11;
            trackBarVolume.ThumbColor = Color.White;
            trackBarVolume.Value = 80;
            // 
            // lblVolumeIcon
            // 
            lblVolumeIcon.Anchor = AnchorStyles.Right;
            lblVolumeIcon.AutoSize = true;
            lblVolumeIcon.Font = new Font("Segoe UI", 12F);
            lblVolumeIcon.ForeColor = Color.Gray;
            lblVolumeIcon.Location = new Point(2522, 32);
            lblVolumeIcon.Name = "lblVolumeIcon";
            lblVolumeIcon.Size = new Size(32, 21);
            lblVolumeIcon.TabIndex = 12;
            lblVolumeIcon.Text = "🔊";
            // 
            // btnKaraoke
            // 
            btnKaraoke = new Guna2Button();
            btnKaraoke.Anchor = AnchorStyles.Right;
            btnKaraoke.BorderRadius = 15;
            btnKaraoke.FillColor = Color.Transparent;
            btnKaraoke.DisabledState.BorderColor = Color.DarkGray;
            btnKaraoke.DisabledState.CustomBorderColor = Color.DarkGray;
            btnKaraoke.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnKaraoke.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnKaraoke.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnKaraoke.ForeColor = Color.White;
            btnKaraoke.Location = new Point(2380, 29); // Left of Volume Icon (2522)
            btnKaraoke.Name = "btnKaraoke";
            btnKaraoke.Size = new Size(120, 32); 
            btnKaraoke.TabIndex = 13;
            btnKaraoke.Text = "🎤 KARAOKE";
            btnKaraoke.Cursor = Cursors.Hand;
            // 
            // UCPlayerBar
            // 
            BackColor = Color.FromArgb(33, 33, 33);
            Controls.Add(picCover);
            Controls.Add(lblTitle);
            Controls.Add(lblArtist);
            Controls.Add(btnPlay);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnShuffle);
            Controls.Add(btnRepeat);
            Controls.Add(btnKaraoke); // Add new button
            Controls.Add(trackBarTime);
            Controls.Add(lblCurrentTime);
            Controls.Add(lblTotalTime);
            Controls.Add(trackBarVolume);
            Controls.Add(lblVolumeIcon);
            Name = "UCPlayerBar";
            Size = new Size(1682, 90);
            ((System.ComponentModel.ISupportInitialize)picCover).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}