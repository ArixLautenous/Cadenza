using Shared.DTOs;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCSongItem : UserControl
    {
        public SongDto SongData { get; private set; }

        public UCSongItem()
        {
            InitializeComponent();

            // Xử lý hiệu ứng Hover giống YouTube Music
            this.MouseEnter += (s, e) => this.BackColor = Color.FromArgb(40, 40, 40);
            this.MouseLeave += (s, e) => this.BackColor = Color.Transparent;

            // Truyền sự kiện hover cho các control con để trải nghiệm mượt mà
            foreach (Control c in this.Controls)
            {
                c.MouseEnter += (s, e) => this.OnMouseEnter(e);
                c.Click += (s, e) => this.OnClick(e); // Click vào chữ cũng tính là click vào Item
            }
        }

        public event EventHandler ItemUpdated; // Reload list
        public event EventHandler ItemDeleted; // Remove from list

        public void SetData(SongDto song, int index, bool isEditable = false)
        {
            this.SongData = song;
            lblIndex.Text = index.ToString();
            lblTitle.Text = song.Title;
            lblArtist.Text = song.ArtistName;
            lblDuration.Text = TimeSpan.FromSeconds(song.Duration).ToString(@"mm\:ss");

            if (!string.IsNullOrEmpty(song.ThumbnailUrl))
            {
                picCover.LoadAsync(song.ThumbnailUrl);
            }

            // Nếu là bài VIP/Exclusive thì đổi màu tên bài thành vàng
            if (song.IsExclusive)
            {
                lblTitle.ForeColor = Color.Gold;
            }
            else
            {
                 lblTitle.ForeColor = Color.White;
            }

            // Context Menu cho Artist tu quan ly nhac cua minh
            if (isEditable)
            {
                var cms = new ContextMenuStrip();
                var btnEdit = cms.Items.Add("Chỉnh sửa thông tin");
                var btnDelete = cms.Items.Add("Xóa bài hát");
                btnDelete.ForeColor = Color.Red;

                btnEdit.Click += (s, e) => 
                {
                    var frm = new RX_Client_WF.Forms.EditSongForm(song);
                    frm.ShowDialog();
                    if (frm.IsUpdated) ItemUpdated?.Invoke(this, EventArgs.Empty);
                };

                btnDelete.Click += async (s, e) => 
                {
                    if (MessageBox.Show($"Bạn có chắc muốn xóa bài hát '{song.Title}' không?\nHành động này không thể hoàn tác.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        var api = new RX_Client_WF.Services.ApiService();
                        bool success = await api.DeleteAsync($"/api/artist/songs/{song.Id}");
                        if (success)
                        {
                            MessageBox.Show("Đã xóa bài hát.");
                            ItemDeleted?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                             MessageBox.Show("Không thể xóa bài hát (Có thể do lỗi Server hoặc bạn không có quyền).");
                        }
                    }
                };
                
                this.ContextMenuStrip = cms;
            }
        }
    }
}