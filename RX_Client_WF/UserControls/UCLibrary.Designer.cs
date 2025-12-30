using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    partial class UCLibrary
    {
        private System.ComponentModel.IContainer components = null;

        // UI Controls
        private Panel pnHeader;
        private Label lblTitle;
        private Guna2Button btnCreateNew;
        private FlowLayoutPanel flowPanel; // Nơi chứa các thẻ Playlist
        private Guna2VScrollBar vScrollBar; // Thanh cuộn tùy chỉnh (Optional, nếu muốn đẹp hơn scroll mặc định)

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnHeader = new Panel();
            lblTitle = new Label();
            btnCreateNew = new Guna2Button();
            flowPanel = new FlowLayoutPanel();
            pnHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnHeader
            // 
            pnHeader.BackColor = Color.Transparent;
            pnHeader.Controls.Add(lblTitle);
            pnHeader.Controls.Add(btnCreateNew);
            pnHeader.Dock = DockStyle.Top;
            pnHeader.Location = new Point(0, 0);
            pnHeader.Name = "pnHeader";
            pnHeader.Padding = new Padding(30, 0, 30, 0);
            pnHeader.Size = new Size(1580, 80);
            pnHeader.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(150, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Thư viện";
            // 
            // btnCreateNew
            // 
            btnCreateNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCreateNew.BorderRadius = 18;
            btnCreateNew.Cursor = Cursors.Hand;
            btnCreateNew.CustomizableEdges = customizableEdges1;
            btnCreateNew.FillColor = Color.White;
            btnCreateNew.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCreateNew.ForeColor = Color.Black;
            btnCreateNew.HoverState.FillColor = Color.FromArgb(230, 230, 230);
            btnCreateNew.Location = new Point(1390, 25);
            btnCreateNew.Name = "btnCreateNew";
            btnCreateNew.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnCreateNew.Size = new Size(160, 36);
            btnCreateNew.TabIndex = 1;
            btnCreateNew.Text = "+ Tạo Playlist mới";
            // 
            // flowPanel
            // 
            flowPanel.AutoScroll = true;
            flowPanel.BackColor = Color.Transparent;
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.Location = new Point(0, 80);
            flowPanel.Name = "flowPanel";
            flowPanel.Padding = new Padding(30, 10, 0, 0);
            flowPanel.Size = new Size(1580, 623);
            flowPanel.TabIndex = 0;
            // 
            // UCLibrary
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(3, 3, 3);
            Controls.Add(flowPanel);
            Controls.Add(pnHeader);
            Name = "UCLibrary";
            Size = new Size(1580, 703);
            pnHeader.ResumeLayout(false);
            pnHeader.PerformLayout();
            ResumeLayout(false);
        }
    }
}