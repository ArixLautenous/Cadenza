using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    partial class UCSongItem
    {
        private System.ComponentModel.IContainer components = null;

        public Label lblIndex;
        public Guna2PictureBox picCover;
        public Label lblTitle;
        public Label lblArtist;
        public Label lblDuration;
        private Guna2Panel pnHover; // Lớp phủ để bắt sự kiện click

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Size = new Size(800, 60);
            this.BackColor = Color.Transparent;
            this.Cursor = Cursors.Hand;

            // 1. Số thứ tự (Index)
            this.lblIndex = new Label();
            this.lblIndex.Text = "1";
            this.lblIndex.ForeColor = Color.Gray;
            this.lblIndex.Font = new Font("Segoe UI", 10);
            this.lblIndex.Location = new Point(15, 20);
            this.lblIndex.AutoSize = true;

            // 2. Ảnh bìa (Nhỏ 40x40)
            this.picCover = new Guna2PictureBox();
            this.picCover.Size = new Size(40, 40);
            this.picCover.Location = new Point(50, 10);
            this.picCover.BorderRadius = 4;
            this.picCover.FillColor = Color.FromArgb(40, 40, 40);
            this.picCover.SizeMode = PictureBoxSizeMode.StretchImage;

            // 3. Tên bài hát
            this.lblTitle = new Label();
            this.lblTitle.Text = "Song Title";
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            this.lblTitle.Location = new Point(105, 10);
            this.lblTitle.Size = new Size(400, 20);
            this.lblTitle.AutoEllipsis = true;

            // 4. Tên nghệ sĩ
            this.lblArtist = new Label();
            this.lblArtist.Text = "Artist Name";
            this.lblArtist.ForeColor = Color.DarkGray;
            this.lblArtist.Font = new Font("Segoe UI", 9);
            this.lblArtist.Location = new Point(105, 32);
            this.lblArtist.Size = new Size(400, 15);
            this.lblArtist.AutoEllipsis = true;

            // 5. Thời lượng
            this.lblDuration = new Label();
            this.lblDuration.Text = "03:45";
            this.lblDuration.ForeColor = Color.Gray;
            this.lblDuration.Font = new Font("Segoe UI", 10);
            this.lblDuration.Anchor = AnchorStyles.Right;
            this.lblDuration.Location = new Point(this.Width - 60, 20);
            this.lblDuration.AutoSize = true;

            // Add Controls
            this.Controls.Add(lblIndex);
            this.Controls.Add(picCover);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblArtist);
            this.Controls.Add(lblDuration);
        }
    }
}