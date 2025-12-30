using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    partial class UCHome
    {
        private System.ComponentModel.IContainer components = null;

        private FlowLayoutPanel flowMain; // Panel chính (Cuộn dọc)
        private Label lblSection1;
        private FlowLayoutPanel flowAlbums; // Panel Album (Cuộn ngang)
        private Label lblSection2;
        private FlowLayoutPanel flowSongs; // Panel Bài hát (Dọc)

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            flowMain = new FlowLayoutPanel();
            flowAlbums = new FlowLayoutPanel();
            flowSongs = new FlowLayoutPanel();
            flowMain.SuspendLayout();
            SuspendLayout();
            // 
            // flowMain
            // 
            flowMain.AutoScroll = true;
            flowMain.Controls.Add(flowAlbums);
            flowMain.Controls.Add(flowSongs);
            flowMain.Dock = DockStyle.Fill;
            flowMain.FlowDirection = FlowDirection.TopDown;
            flowMain.Location = new Point(0, 0);
            flowMain.Name = "flowMain";
            flowMain.Padding = new Padding(20);
            flowMain.Size = new Size(1582, 703);
            flowMain.TabIndex = 0;
            flowMain.WrapContents = false;
            // 
            // flowAlbums
            // 
            flowAlbums.AutoScroll = true;
            flowAlbums.Location = new Point(23, 23);
            flowAlbums.Name = "flowAlbums";
            flowAlbums.Size = new Size(1200, 260);
            flowAlbums.TabIndex = 0;
            flowAlbums.WrapContents = false;
            // 
            // flowSongs
            // 
            flowSongs.AutoSize = true;
            flowSongs.FlowDirection = FlowDirection.TopDown;
            flowSongs.Location = new Point(23, 289);
            flowSongs.MinimumSize = new Size(1000, 0);
            flowSongs.Name = "flowSongs";
            flowSongs.Size = new Size(1000, 0);
            flowSongs.TabIndex = 1;
            flowSongs.WrapContents = false;
            // 
            // UCHome
            // 
            BackColor = Color.Black;
            Controls.Add(flowMain);
            Name = "UCHome";
            Size = new Size(1582, 703);
            flowMain.ResumeLayout(false);
            flowMain.PerformLayout();
            ResumeLayout(false);
        }

        private Label CreateTitle(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(3, 10, 3, 15)
            };
        }
    }
}