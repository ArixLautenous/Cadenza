using Shared.DTOs;
using RX_Client_WF.Services;
using RX_Client_WF.UserControls;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace RX_Client_WF.UserControls
{
    public partial class UCSearchResults : UserControl
    {
        private ApiService _apiService;
        private string _query;
        private FlowLayoutPanel flowResults;
        private Label lblHeader;

        public event Action<SongDto> SongClicked;

        public UCSearchResults(string query)
        {
            _query = query;
            _apiService = new ApiService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(3, 3, 3);
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;

            lblHeader = new Label
            {
                Text = $"Kết quả tìm kiếm cho: \"{_query}\"",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            flowResults = new FlowLayoutPanel
            {
                Location = new Point(20, 70),
                Size = new Size(1000, 600), // Dynamic resize handled later
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            this.Controls.Add(lblHeader);
            this.Controls.Add(flowResults);
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadResults();
        }

        private async System.Threading.Tasks.Task LoadResults()
        {
            try 
            {
                var songs = await _apiService.GetAsync<List<SongDto>>($"/api/songs/search?query={_query}");

                flowResults.Controls.Clear();
                if (songs != null && songs.Count > 0)
                {
                    int idx = 1;
                    foreach (var song in songs)
                    {
                        var item = new UCSongItem();
                        item.SetData(song, idx++);
                        item.Width = this.Width - 100;
                        item.Click += (s, e) => SongClicked?.Invoke(song);
                        flowResults.Controls.Add(item);
                    }
                }
                else
                {
                    var lblEmpty = new Label 
                    { 
                        Text = "Không tìm thấy bài hát nào.", 
                        ForeColor = Color.Gray, 
                        AutoSize = true, 
                        Font = new Font("Segoe UI", 12)
                    };
                    flowResults.Controls.Add(lblEmpty);
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }
    }
}
