using Guna.UI2.WinForms;
using RX_Client_WF.UserControls;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnSidebar;
        private Panel pnContent;
        private UCPlayerBar playerBar;

        private Label lblLogo;
        private Guna2Button btnHome;
        private Guna2Button btnLibrary;
        private Guna2Button btnProfile; // MỚI THÊM
        private Guna2Button btnUpload;
        private Guna2Button btnSettings;
        private Guna2Button btnLogout;
        private Guna2Separator separator;

        private Guna2DragControl dragControl;
        private Guna2ControlBox btnClose;
        private Guna2ControlBox btnMaximize;
        private Guna2ControlBox btnMinimize;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(3, 3, 3);

            this.dragControl = new Guna2DragControl(this.components);
            this.dragControl.TargetControl = this;

            // --- Sidebar ---
            this.pnSidebar = new Panel();
            this.pnSidebar.Dock = DockStyle.Left;
            this.pnSidebar.Width = 240;
            this.pnSidebar.BackColor = Color.FromArgb(18, 18, 18);

            this.lblLogo = new Label();
            this.lblLogo.Text = "Cadenza";
            this.lblLogo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblLogo.ForeColor = Color.White;
            this.lblLogo.Location = new Point(30, 30);
            this.lblLogo.AutoSize = true;

            // Các nút Menu
            this.btnHome = CreateMenuButton("Trang chủ", 100, "🏠");
            this.btnLibrary = CreateMenuButton("Thư viện", 160, "📚");
            this.btnProfile = CreateMenuButton("Hồ sơ", 220, "👤"); // Nút mới
            this.btnUpload = CreateMenuButton("Upload Nhạc", 280, "⬆️");
            this.btnUpload.Visible = false;
            this.btnSettings = CreateMenuButton("Cài đặt", 550, "⚙️");

            this.separator = new Guna2Separator();
            this.separator.Location = new Point(20, 580);
            this.separator.Size = new Size(200, 10);
            this.separator.FillColor = Color.FromArgb(50, 50, 50);
            this.separator.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            this.btnLogout = CreateMenuButton("Đăng xuất", 600, "🚪");
            this.btnLogout.FillColor = Color.Transparent;
            this.btnLogout.ForeColor = Color.IndianRed;
            this.btnLogout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnLogout.HoverState.FillColor = Color.FromArgb(30, 0, 0);

            this.pnSidebar.Controls.Add(lblLogo);
            this.pnSidebar.Controls.Add(btnHome);
            this.pnSidebar.Controls.Add(btnLibrary);
            this.pnSidebar.Controls.Add(btnProfile); // Add nút Profile
            this.pnSidebar.Controls.Add(btnUpload);
            this.pnSidebar.Controls.Add(btnSettings);
            this.pnSidebar.Controls.Add(separator);
            this.pnSidebar.Controls.Add(btnLogout);

            // --- PlayerBar ---
            this.playerBar = new UCPlayerBar();
            this.playerBar.Dock = DockStyle.Bottom;

            // --- Content Panel ---
            this.pnContent = new Panel();
            this.pnContent.Dock = DockStyle.Fill;
            this.pnContent.BackColor = Color.FromArgb(3, 3, 3);
            
            // --- Header ---
            this.pnHeader = new Guna2Panel();
            this.pnHeader.Dock = DockStyle.Top;
            this.pnHeader.Height = 60;
            this.pnHeader.BackColor = Color.FromArgb(3, 3, 3);
            // Search Box
            this.txtSearch = new Guna2TextBox();
            this.txtSearch.PlaceholderText = "Tìm kiếm bài hát, nghệ sĩ, album...";
            this.txtSearch.Size = new Size(500, 40);
            this.txtSearch.Location = new Point(270, 10);
            this.txtSearch.BorderRadius = 20;
            this.txtSearch.FillColor = Color.FromArgb(40, 40, 40);
            this.txtSearch.ForeColor = Color.White;
            this.txtSearch.BorderThickness = 0;
            this.txtSearch.TextOffset = new Point(10, 0);
            this.txtSearch.Anchor = AnchorStyles.Top;
            
            this.pnHeader.Controls.Add(this.txtSearch);

            // --- Window Controls ---
            this.btnClose = new Guna2ControlBox { Anchor = AnchorStyles.Top | AnchorStyles.Right, FillColor = Color.Transparent, Location = new Point(1235, 0), Size = new Size(45, 30) };
            this.btnMaximize = new Guna2ControlBox { ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox, Anchor = AnchorStyles.Top | AnchorStyles.Right, FillColor = Color.Transparent, Location = new Point(1190, 0), Size = new Size(45, 30) };
            this.btnMinimize = new Guna2ControlBox { ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox, Anchor = AnchorStyles.Top | AnchorStyles.Right, FillColor = Color.Transparent, Location = new Point(1145, 0), Size = new Size(45, 30) };

            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnMaximize);
            this.Controls.Add(this.btnMinimize);

            this.Controls.Add(this.pnContent);
            this.Controls.Add(this.pnHeader); // Add Header
            this.Controls.Add(this.playerBar);
            this.Controls.Add(this.pnSidebar);
        }

        private Guna2Panel pnHeader;
        public Guna2TextBox txtSearch; // Public de Logic truy cap

        private Guna2Button CreateMenuButton(string text, int y, string iconSymbol = "")
        {
            return new Guna2Button
            {
                Text = $"  {iconSymbol}  {text}",
                FillColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(10, y),
                Size = new Size(220, 45),
                BorderRadius = 10,
                TextAlign = HorizontalAlignment.Left,
                TextOffset = new Point(10, 0),
                Cursor = Cursors.Hand,
                UseTransparentBackground = true
            };
        }
    }
}