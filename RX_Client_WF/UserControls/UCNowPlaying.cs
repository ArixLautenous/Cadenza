using Guna.UI2.WinForms;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCNowPlaying : UserControl
    {
        // UI Elements
        private Guna2Panel pnLeft;
        private Guna2PictureBox picCover;

        private Guna2Panel pnRight;
        private Guna2Panel pnTabs;
        private Guna2Button btnTabNext;
        private Guna2Button btnTabLyrics;
        private Guna2Button btnTabInfo;
        private Guna2Panel pnTabIndicator;

        // Content Panels
        private Guna2Panel pnContent;
        private FlowLayoutPanel flowQueue;
        private Panel pnLyricsContainer;
        private Label lblLyrics;
        private Panel pnSongInfo;
        private Label lblInfoData; // Label to show Song Info text

        private Label lblQueueHeader; // "Đang phát từ..."

        public UCNowPlaying()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(10, 10, 10);
            this.Size = new Size(1200, 700);

            // --- LEFT PANEL (Album Art) ---
            pnLeft = new Guna2Panel { Dock = DockStyle.Left, Width = 600, BackColor = Color.Transparent };
            picCover = new Guna2PictureBox
            {
                Size = new Size(500, 500),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderRadius = 20,
                FillColor = Color.FromArgb(30,30,30),
                Location = new Point(50, 100)
            };
             // Drop Shadow setup
            picCover.Paint += (s, e) => 
            {
                // Simple workaround for shadow if ShadowDecoration causes lag or issues
            };
            picCover.ShadowDecoration.Enabled = true;
            picCover.ShadowDecoration.Depth = 15;
            picCover.ShadowDecoration.Color = Color.Black;
            picCover.ShadowDecoration.BorderRadius = 20;

            pnLeft.Controls.Add(picCover);


            // --- RIGHT PANEL (Tabs + Content) ---
            pnRight = new Guna2Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(20, 10, 20, 10) };

            // 1. Tabs Container
            pnTabs = new Guna2Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(10, 10, 10) };
            
            // Just add buttons directly or use a FlowLayout, but manual positioning is fine if we center or align left.
            // Let's keep manual layout for simplicity as per previous code but without the Right Panel.

            btnTabNext = CreateTabButton("TIẾP THEO");
            btnTabLyrics = CreateTabButton("LỜI NHẠC");
            btnTabInfo = CreateTabButton("THÔNG TIN BÀI NHẠC");

            // Manually position buttons
            btnTabNext.Location = new Point(20, 15);
            btnTabNext.Size = new Size(120, 40);

            btnTabLyrics.Location = new Point(150, 15);
            btnTabLyrics.Size = new Size(120, 40);

            btnTabInfo.Location = new Point(280, 15);
            btnTabInfo.Size = new Size(220, 40);

            // Indicator
            pnTabIndicator = new Guna2Panel 
            { 
                Height = 3, 
                FillColor = Color.White, 
                Location = new Point(20, 60), 
                Size = new Size(120, 3) 
            };

            btnTabNext.Click += (s, e) => SwitchTab(0);
            btnTabLyrics.Click += (s, e) => SwitchTab(1);
            btnTabInfo.Click += (s, e) => SwitchTab(2);

            pnTabs.Controls.Add(btnTabNext);
            pnTabs.Controls.Add(btnTabLyrics);
            pnTabs.Controls.Add(btnTabInfo);
            pnTabs.Controls.Add(pnTabIndicator);
            
            // Ensure Tabs are on top visually
            pnTabs.BringToFront();


            // 2. Content Area
            pnContent = new Guna2Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            pnContent.Padding = new Padding(0, 10, 0, 0);

            // --- VIEW 1: QUEUE ---
            flowQueue = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent
            };
            // Header inside Queue (optional, often nice to separate)
             lblQueueHeader = new Label
            {
                Text = "Đang phát từ: Danh sách phát",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 10)
            };
            // Wrapper for Queue to include header
            var pnQueueWrapper = new Panel { Dock = DockStyle.Fill, Visible = true };
            pnQueueWrapper.Controls.Add(flowQueue);
            pnQueueWrapper.Controls.Add(lblQueueHeader);

            // --- VIEW 2: LYRICS ---
            pnLyricsContainer = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Visible = false };
            lblLyrics = new Label 
            { 
                AutoSize = true, 
                MaximumSize = new Size(500, 0), // Width limit updated on resize
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Padding = new Padding(10),
                Text = "Chưa có lời bài hát."
            };
            pnLyricsContainer.Controls.Add(lblLyrics);


            // --- VIEW 3: SONG INFO ---
            pnSongInfo = new Panel { Dock = DockStyle.Fill, Visible = false, Padding = new Padding(20) };
            lblInfoData = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.LightGray,
                Text = "Thông tin chi tiết..."
            };
            pnSongInfo.Controls.Add(lblInfoData);


            pnContent.Controls.Add(pnQueueWrapper);
            pnContent.Controls.Add(pnLyricsContainer);
            pnContent.Controls.Add(pnSongInfo);


            // Add to Right
            pnRight.Controls.Add(pnContent);
            pnRight.Controls.Add(pnTabs);

            // Add to Control
            this.Controls.Add(pnRight);
            this.Controls.Add(pnLeft);

            // Resize Logic
            this.Resize += (s, e) =>
            {
                // Responsive Layout
                if (this.Width < 900)
                {
                    pnLeft.Visible = false;
                    pnLeft.Width = 0;
                }
                else
                {
                    pnLeft.Visible = true;
                    pnLeft.Width = (int)(this.Width * 0.45);
                    
                    int size = Math.Min(pnLeft.Width - 60, pnLeft.Height - 60);
                    if (size > 500) size = 500;
                    picCover.Size = new Size(size, size);
                    picCover.Location = new Point((pnLeft.Width - size) / 2, (pnLeft.Height - size) / 2);
                }

                // Update Lyrics Width limits
                lblLyrics.MaximumSize = new Size(pnContent.Width - 50, 0);
                
                // Update Queue Width
                foreach (Control c in flowQueue.Controls)
                {
                    c.Width = flowQueue.Width - 30;
                }
            };
            
            // Set Default Tab
            SwitchTab(0);
        }

        private void SwitchTab(int index)
        {
            // Reset Colors
            btnTabNext.ForeColor = Color.Gray;
            btnTabLyrics.ForeColor = Color.Gray;
            btnTabInfo.ForeColor = Color.Gray;
            
            // Hide Contents
            flowQueue.Parent.Visible = false;
            pnLyricsContainer.Visible = false;
            pnSongInfo.Visible = false;

            if (index == 0) // Next
            {
                btnTabNext.ForeColor = Color.White;
                MoveIndicator(btnTabNext);
                flowQueue.Parent.Visible = true;
            }
            else if (index == 1) // Lyrics
            {
                btnTabLyrics.ForeColor = Color.White;
                MoveIndicator(btnTabLyrics);
                pnLyricsContainer.Visible = true;
            }
            else // Info
            {
                btnTabInfo.ForeColor = Color.White;
                MoveIndicator(btnTabInfo);
                pnSongInfo.Visible = true;
            }
        }

        private void MoveIndicator(Control target)
        {
            pnTabIndicator.Width = target.Width;
            pnTabIndicator.Location = new Point(target.Location.X, pnTabIndicator.Location.Y);
        }

        private Guna2Button CreateTabButton(string text)
        {
            return new Guna2Button
            {
                Text = text,
                FillColor = Color.Transparent,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                HoverState = { FillColor = Color.Transparent, ForeColor = Color.White },
                PressedDepth = 0,
                // Cursor = Cursors.Hand
            };
        }

        public void SetSong(SongDto song)
        {
            if (song == null) return;

            // Update Cover
             if (!string.IsNullOrEmpty(song.ThumbnailUrl))
                picCover.LoadAsync(song.ThumbnailUrl);
            else
                picCover.Image = null;

            // Update Header
            lblQueueHeader.Text = $"Đang phát từ: {song.AlbumName ?? "Danh sách bài hát"}";

            // Update Info Tab
            lblInfoData.Text = $"Bài hát: {song.Title}\n" +
                               $"Nghệ sĩ: {song.ArtistName}\n" +
                               $"Album: {song.AlbumName}\n" +
                               $"Thể loại: {song.GenreName}\n" +
                               $"Thời lượng: {TimeSpan.FromSeconds(song.Duration).ToString(@"mm\:ss")}\n" +
                               $"Loại: {(song.IsExclusive ? "Độc quyền (Premium)" : "Miễn phí")}";

            // Update Lyrics (Placeholder)
            // Update Lyrics
            if (!string.IsNullOrEmpty(song.Lyrics))
            {
                lblLyrics.Text = song.Lyrics;
            }
            else
            {
                lblLyrics.Text = "Lời bài hát đang được cập nhật...";
            }
        }

        public void SetQueue(List<SongDto> songs)
        {
            flowQueue.Controls.Clear();
            if (songs == null) return;

            int idx = 1;
            foreach (var song in songs)
            {
                var item = new UCSongItem();
                item.SetData(song, idx++);
                item.Width = flowQueue.Width - 30;
                item.Height = 60;
                item.BackColor = Color.Transparent; 
                flowQueue.Controls.Add(item);
            }
        }
    }
}
