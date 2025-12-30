using RX_Client_WF.Services;
using RX_Client_WF.UserControls;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCHome : UserControl
    {
        private ApiService _apiService;

        // Sự kiện khi click bài hát -> Báo cho MainForm biết để Play
        public event Action<SongDto> SongClicked;

        public UCHome()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 1. Load Dummy Albums (Hoặc gọi API Albums nếu có)
            for (int i = 0; i < 5; i++)
            {
                var album = new UCAlbumCard();
                album.SetData($"Album {i + 1}", "Nghệ sĩ nổi bật", null);
                flowAlbums.Controls.Add(album);
            }

            // 2. Load Songs từ API
            var songs = await _apiService.GetAsync<List<SongDto>>("/api/songs");

            if (songs != null)
            {
                int index = 1;
                foreach (var song in songs)
                {
                    var item = new UCSongItem();
                    item.SetData(song, index++);
                    item.Width = this.Width - 80; // Trừ hao scrollbar

                    // Bắt sự kiện click
                    item.Click += (s, ev) => SongClicked?.Invoke(song);

                    flowSongs.Controls.Add(item);
                }
            }
        }
    }
}