using Guna.UI2.WinForms;
using Shared.DTOs;
using RX_Client_WF.Forms;
using RX_Client_WF.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCLibrary : UserControl
    {
        private readonly ApiService _apiService;

        public UCLibrary()
        {
            InitializeComponent();
            _apiService = new ApiService();

            // Gắn sự kiện click cho nút Tạo mới
            btnCreateNew.Click += BtnCreateNew_Click;
        }

        // Tự động tải dữ liệu khi Control hiện ra
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadPlaylists();
        }

        private void BtnCreateNew_Click(object sender, EventArgs e)
        {
            CreatePlaylistForm frm = new CreatePlaylistForm();
            // Đăng ký sự kiện: Khi tạo xong thì reload lại thư viện
            frm.PlaylistCreated += (s, args) => LoadPlaylists();
            frm.ShowDialog();
        }

        // Hàm tải danh sách Playlist từ Server
        public async void LoadPlaylists()
        {
            // Xóa danh sách cũ
            flowPanel.Controls.Clear();

            // Hiển thị Loading (Optional: Có thể thêm Spinner nếu muốn)

            try
            {
                var playlists = await _apiService.GetAsync<List<PlaylistDto>>("/api/users/playlists");

                if (playlists != null && playlists.Count > 0)
                {
                    foreach (var p in playlists)
                    {
                        var card = CreatePlaylistCard(p);
                        flowPanel.Controls.Add(card);
                    }
                }
                else
                {
                    // Hiển thị thông báo trống
                    Label lblEmpty = new Label();
                    lblEmpty.Text = "Bạn chưa có playlist nào. Hãy tạo cái đầu tiên!";
                    lblEmpty.ForeColor = Color.Gray;
                    lblEmpty.Font = new Font("Segoe UI", 12);
                    lblEmpty.AutoSize = true;
                    lblEmpty.Margin = new Padding(20);
                    flowPanel.Controls.Add(lblEmpty);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ mất mạng)
            }
        }

        // Hàm tạo giao diện thẻ Playlist (Card)
        private Control CreatePlaylistCard(PlaylistDto playlist)
        {
            // 1. Card Container (Panel)
            Guna2Panel card = new Guna2Panel();
            card.Size = new Size(180, 240);
            card.Margin = new Padding(0, 0, 20, 20); // Khoảng cách giữa các thẻ
            card.FillColor = Color.Transparent;
            card.Cursor = Cursors.Hand;

            // 2. Ảnh Playlist (Dùng màu Gradient hoặc Icon mặc định)
            Guna2Panel imgPanel = new Guna2Panel();
            imgPanel.Size = new Size(180, 180);
            imgPanel.Dock = DockStyle.Top;
            imgPanel.BorderRadius = 8;
            // Màu ngẫu nhiên hoặc màu cố định cho đẹp
            imgPanel.FillColor = Color.FromArgb(45, 45, 45);

            // Icon nhạc ở giữa
            Label icon = new Label();
            icon.Text = "🎵";
            icon.Font = new Font("Segoe UI Emoji", 40);
            icon.ForeColor = Color.DimGray;
            icon.AutoSize = true;
            // Căn giữa icon (thủ công)
            icon.Location = new Point(55, 50);

            imgPanel.Controls.Add(icon);

            // 3. Tên Playlist
            Label lblName = new Label();
            lblName.Text = playlist.Name;
            lblName.ForeColor = Color.White;
            lblName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblName.Location = new Point(0, 190);
            lblName.Size = new Size(180, 20);
            lblName.AutoEllipsis = true; // Tự động ... nếu tên quá dài

            // 4. Số lượng bài hát
            Label lblCount = new Label();
            lblCount.Text = $"{playlist.SongCount} bài hát";
            lblCount.ForeColor = Color.Gray;
            lblCount.Font = new Font("Segoe UI", 9);
            lblCount.Location = new Point(0, 215);
            lblCount.AutoSize = true;

            // Add vào Card
            card.Controls.Add(imgPanel);
            card.Controls.Add(lblName);
            card.Controls.Add(lblCount);

            // Hiệu ứng Hover: Sáng nền ảnh lên
            card.MouseEnter += (s, e) => imgPanel.FillColor = Color.FromArgb(60, 60, 60);
            card.MouseLeave += (s, e) => imgPanel.FillColor = Color.FromArgb(45, 45, 45);

            // Truyền sự kiện hover cho các control con
            imgPanel.MouseEnter += (s, e) => imgPanel.FillColor = Color.FromArgb(60, 60, 60);

            // Sự kiện Click (Mở chi tiết Playlist - Tính năng nâng cao sau này)
            card.Click += (s, e) =>
            {
                // MessageBox.Show($"Mở playlist: {playlist.Name}"); 
            };

            return card;
        }
    }
}