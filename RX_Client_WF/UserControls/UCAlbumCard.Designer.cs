using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    partial class UCAlbumCard
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2PictureBox picCover;
        private Label lblTitle;
        private Label lblSubTitle;
        private Guna2Panel pnOverlay;
        private Guna2ImageButton btnPlay;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Size = new Size(180, 240);
            this.BackColor = Color.Transparent;
            this.Cursor = Cursors.Hand;

            // 1. Ảnh bìa Album (Vuông 160x160)
            this.picCover = new Guna2PictureBox();
            this.picCover.Size = new Size(160, 160);
            this.picCover.Location = new Point(10, 10);
            this.picCover.BorderRadius = 8;
            this.picCover.FillColor = Color.FromArgb(40, 40, 40);
            this.picCover.SizeMode = PictureBoxSizeMode.StretchImage;

            // 2. Tên Album
            this.lblTitle = new Label();
            this.lblTitle.Text = "Album Title";
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            this.lblTitle.Location = new Point(10, 180);
            this.lblTitle.Size = new Size(160, 20);
            this.lblTitle.AutoEllipsis = true;

            // 3. Tên Nghệ sĩ / Năm
            this.lblSubTitle = new Label();
            this.lblSubTitle.Text = "Artist • 2024";
            this.lblSubTitle.ForeColor = Color.Gray;
            this.lblSubTitle.Font = new Font("Segoe UI", 9);
            this.lblSubTitle.Location = new Point(10, 205);
            this.lblSubTitle.Size = new Size(160, 15);
            this.lblSubTitle.AutoEllipsis = true;

            this.Controls.Add(picCover);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubTitle);
        }
    }
}