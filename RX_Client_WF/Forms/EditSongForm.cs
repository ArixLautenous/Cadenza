using Guna.UI2.WinForms;
using RX_Client_WF.Services;
using Shared.DTOs;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    public partial class EditSongForm : Form
    {
        private readonly ApiService _apiService;
        private readonly int _songId;
        private Guna2TextBox txtTitle;
        private Guna2ComboBox cbGenre;
        private Guna2CheckBox chkExclusive;
        private Guna2Button btnSave;
        private Guna2Button btnCancel;

        public bool IsUpdated { get; private set; } = false;

        public EditSongForm(SongDto song)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _songId = song.Id;

            // Load data
            txtTitle.Text = song.Title;
            
            // Demo Genres (Can improve by fetching from server)
            cbGenre.Items.AddRange(new object[] { "Pop", "Rock", "Ballad", "Indie", "Rap" });
            cbGenre.SelectedItem = song.GenreName; 
            if (cbGenre.SelectedIndex < 0) cbGenre.SelectedIndex = 0;

            chkExclusive.Checked = song.IsExclusive;
        }

        private void InitializeComponent()
        {
            this.Text = "Chỉnh Sửa Bài Hát";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.ForeColor = Color.White;

            var lblTitle = new Label { Text = "Tên bài hát:", Location = new Point(20, 20), AutoSize = true, ForeColor = Color.Gray };
            txtTitle = new Guna2TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(340, 36),
                BorderRadius = 4,
                FillColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White
            };

            var lblGenre = new Label { Text = "Thể loại:", Location = new Point(20, 100), AutoSize = true, ForeColor = Color.Gray };
            cbGenre = new Guna2ComboBox
            {
                Location = new Point(20, 130),
                Size = new Size(340, 36),
                BorderRadius = 4,
                FillColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White
            };

            chkExclusive = new Guna2CheckBox
            {
                Text = "Nội dung độc quyền (Audiophile Only)",
                Location = new Point(20, 190),
                AutoSize = true,
                ForeColor = Color.Gold
            };

            btnSave = new Guna2Button
            {
                Text = "LƯU THAY ĐỔI",
                Location = new Point(20, 240),
                Size = new Size(160, 40),
                FillColor = Color.FromArgb(29, 185, 84), // Green
                BorderRadius = 20,
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Guna2Button
            {
                Text = "HỦY",
                Location = new Point(200, 240),
                Size = new Size(160, 40),
                FillColor = Color.FromArgb(60, 60, 60),
                BorderRadius = 20,
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtTitle);
            this.Controls.Add(lblGenre);
            this.Controls.Add(cbGenre);
            this.Controls.Add(chkExclusive);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("Vui lòng nhập tên bài hát.");
                return;
            }

            btnSave.Enabled = false;
            btnSave.Text = "ĐANG LƯU...";

            var updateModel = new
            {
                Title = txtTitle.Text,
                GenreId = cbGenre.SelectedIndex + 1, // Map tam thoi tuong ung voi List
                IsExclusive = chkExclusive.Checked
            };

            bool success = await _apiService.PutAsync($"/api/artist/songs/{_songId}", updateModel);

            if (success)
            {
                MessageBox.Show("Cập nhật thành công!");
                IsUpdated = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Lỗi khi cập nhật. Vui lòng thử lại.");
                btnSave.Enabled = true;
                btnSave.Text = "LƯU THAY ĐỔI";
            }
        }
    }
}
