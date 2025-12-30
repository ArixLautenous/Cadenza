using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    partial class UCUserProfile
    {
        private System.ComponentModel.IContainer components = null;

        // UI Controls
        private Guna2CirclePictureBox picAvatar;
        private Label lblUsername;
        private Guna2Button btnEditProfile; // (Tùy chọn)
        private Label lblRole;
        private Guna2ImageButton iconVerified; // Tích xanh

        // Stats (Follower)
        private Label lblFollowers;
        private Label lblFollowersTitle;

        // Subscription Card
        private Guna2Panel pnPlanCard;
        private Label lblPlanTitle;
        private Label lblPlanName;
        private Label lblExpireDate;
        private Guna2Button btnUpgrade;
        private Guna2PictureBox picPlanIcon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCUserProfile));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            picAvatar = new Guna2CirclePictureBox();
            lblUsername = new Label();
            iconVerified = new Guna2ImageButton();
            lblRole = new Label();
            lblFollowers = new Label();
            lblFollowersTitle = new Label();
            pnPlanCard = new Guna2Panel();
            lblPlanTitle = new Label();
            lblPlanName = new Label();
            lblExpireDate = new Label();
            btnUpgrade = new Guna2Button();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            pnPlanCard.SuspendLayout();
            SuspendLayout();
            // 
            // picAvatar
            // 
            picAvatar.FillColor = Color.FromArgb(40, 40, 40);
            picAvatar.ImageRotate = 0F;
            picAvatar.Location = new Point(50, 50);
            picAvatar.Name = "picAvatar";
            picAvatar.ShadowDecoration.CustomizableEdges = customizableEdges1;
            picAvatar.Size = new Size(150, 150);
            picAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
            picAvatar.TabIndex = 0;
            picAvatar.TabStop = false;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblUsername.ForeColor = Color.White;
            lblUsername.Location = new Point(230, 60);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(203, 51);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username";
            // 
            // iconVerified
            // 
            iconVerified.Image = (Image)resources.GetObject("iconVerified.Image");
            iconVerified.ImageOffset = new Point(0, 0);
            iconVerified.ImageRotate = 0F;
            iconVerified.Location = new Point(500, 75);
            iconVerified.Name = "iconVerified";
            iconVerified.ShadowDecoration.CustomizableEdges = customizableEdges2;
            iconVerified.Size = new Size(30, 30);
            iconVerified.TabIndex = 2;
            iconVerified.Visible = false;
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 12F);
            lblRole.ForeColor = Color.FromArgb(170, 170, 170);
            lblRole.Location = new Point(235, 115);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(93, 21);
            lblRole.TabIndex = 3;
            lblRole.Text = "Người nghe";
            // 
            // lblFollowers
            // 
            lblFollowers.AutoSize = true;
            lblFollowers.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblFollowers.ForeColor = Color.White;
            lblFollowers.Location = new Point(235, 150);
            lblFollowers.Name = "lblFollowers";
            lblFollowers.Size = new Size(23, 25);
            lblFollowers.TabIndex = 4;
            lblFollowers.Text = "0";
            lblFollowers.Visible = false;
            // 
            // lblFollowersTitle
            // 
            lblFollowersTitle.AutoSize = true;
            lblFollowersTitle.Font = new Font("Segoe UI", 10F);
            lblFollowersTitle.ForeColor = Color.Gray;
            lblFollowersTitle.Location = new Point(235, 175);
            lblFollowersTitle.Name = "lblFollowersTitle";
            lblFollowersTitle.Size = new Size(99, 19);
            lblFollowersTitle.TabIndex = 5;
            lblFollowersTitle.Text = "người theo dõi";
            lblFollowersTitle.Visible = false;
            // 
            // pnPlanCard
            // 
            pnPlanCard.BorderColor = Color.FromArgb(40, 40, 40);
            pnPlanCard.BorderRadius = 10;
            pnPlanCard.BorderThickness = 1;
            pnPlanCard.Controls.Add(lblPlanTitle);
            pnPlanCard.Controls.Add(lblPlanName);
            pnPlanCard.Controls.Add(lblExpireDate);
            pnPlanCard.Controls.Add(btnUpgrade);
            pnPlanCard.CustomizableEdges = customizableEdges5;
            pnPlanCard.FillColor = Color.FromArgb(20, 20, 20);
            pnPlanCard.Location = new Point(50, 250);
            pnPlanCard.Name = "pnPlanCard";
            pnPlanCard.ShadowDecoration.CustomizableEdges = customizableEdges6;
            pnPlanCard.Size = new Size(400, 180);
            pnPlanCard.TabIndex = 6;
            // 
            // lblPlanTitle
            // 
            lblPlanTitle.AutoSize = true;
            lblPlanTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPlanTitle.ForeColor = Color.Gray;
            lblPlanTitle.Location = new Point(20, 20);
            lblPlanTitle.Name = "lblPlanTitle";
            lblPlanTitle.Size = new Size(117, 15);
            lblPlanTitle.TabIndex = 0;
            lblPlanTitle.Text = "GÓI CƯỚC HIỆN TẠI";
            // 
            // lblPlanName
            // 
            lblPlanName.AutoSize = true;
            lblPlanName.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblPlanName.ForeColor = Color.White;
            lblPlanName.Location = new Point(20, 50);
            lblPlanName.Name = "lblPlanName";
            lblPlanName.Size = new Size(133, 37);
            lblPlanName.TabIndex = 1;
            lblPlanName.Text = "Standard";
            // 
            // lblExpireDate
            // 
            lblExpireDate.AutoSize = true;
            lblExpireDate.Font = new Font("Segoe UI", 10F);
            lblExpireDate.ForeColor = Color.Silver;
            lblExpireDate.Location = new Point(22, 90);
            lblExpireDate.Name = "lblExpireDate";
            lblExpireDate.Size = new Size(66, 19);
            lblExpireDate.TabIndex = 2;
            lblExpireDate.Text = "Vĩnh viễn";
            // 
            // btnUpgrade
            // 
            btnUpgrade.BorderRadius = 18;
            btnUpgrade.Cursor = Cursors.Hand;
            btnUpgrade.CustomizableEdges = customizableEdges3;
            btnUpgrade.FillColor = Color.White;
            btnUpgrade.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnUpgrade.ForeColor = Color.Black;
            btnUpgrade.Location = new Point(240, 20);
            btnUpgrade.Name = "btnUpgrade";
            btnUpgrade.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnUpgrade.Size = new Size(140, 36);
            btnUpgrade.TabIndex = 3;
            btnUpgrade.Text = "Nâng Cấp";
            // 
            // 
            // pnSongs
            // 
            pnSongs = new FlowLayoutPanel();
            pnSongs.AutoScroll = true;
            pnSongs.Location = new Point(500, 250); // Bên phải Plan Card
            pnSongs.Name = "pnSongs";
            pnSongs.Size = new Size(1000, 400); 
            pnSongs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnSongs.TabIndex = 7;
            // 
            // UCUserProfile
            // 
            BackColor = Color.FromArgb(3, 3, 3);
            Controls.Add(picAvatar);
            Controls.Add(lblUsername);
            Controls.Add(iconVerified);
            Controls.Add(lblRole);
            Controls.Add(lblFollowers);
            Controls.Add(lblFollowersTitle);
            Controls.Add(pnPlanCard);
            Controls.Add(pnSongs);
            Name = "UCUserProfile";
            Size = new Size(1580, 703);
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            pnPlanCard.ResumeLayout(false);
            pnPlanCard.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        // Khai bao variable
        private FlowLayoutPanel pnSongs;
    }
}