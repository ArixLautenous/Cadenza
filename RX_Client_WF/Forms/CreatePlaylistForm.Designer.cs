using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    partial class CreatePlaylistForm
    {
        private System.ComponentModel.IContainer components = null;

        // Khai báo các Controls
        private Guna2TextBox txtName;
        private Guna2Button btnCreate;
        private Label lblTitle;
        private Guna2ControlBox btnClose;
        private Guna2DragControl dragControl;
        private Guna2ShadowForm shadowForm; // Đổ bóng cho đẹp

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // 1. Cấu hình Form
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(400, 220);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; // Không viền chuẩn style App
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(3, 3, 3); // Nền đen sâu (Deep Black) chuẩn YouTube Music

            // 2. Tiện ích (Kéo thả, Đổ bóng)
            this.dragControl = new Guna2DragControl(this.components);
            this.dragControl.TargetControl = this;

            this.shadowForm = new Guna2ShadowForm(this.components);
            this.shadowForm.TargetForm = this;

            // 3. Nút Đóng (X)
            this.btnClose = new Guna2ControlBox();
            this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnClose.FillColor = Color.Transparent;
            this.btnClose.IconColor = Color.White;
            this.btnClose.Location = new Point(355, 0);
            this.btnClose.Size = new Size(45, 30);

            // 4. Tiêu đề
            this.lblTitle = new Label();
            this.lblTitle.Text = "Tạo Playlist Mới";
            this.lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(20, 20);

            // 5. Input Tên Playlist
            this.txtName = new Guna2TextBox();
            this.txtName.PlaceholderText = "Nhập tên playlist...";
            this.txtName.FillColor = Color.FromArgb(33, 33, 33); // Xám đậm #212121
            this.txtName.ForeColor = Color.White;
            this.txtName.BorderThickness = 0;
            this.txtName.BorderRadius = 4;
            this.txtName.Font = new Font("Segoe UI", 11);
            this.txtName.Location = new Point(25, 70);
            this.txtName.Size = new Size(350, 45);
            this.txtName.TextOffset = new Point(5, 0);

            // 6. Nút Tạo (Trắng chữ Đen chuẩn YouTube Music)
            this.btnCreate = new Guna2Button();
            this.btnCreate.Text = "TẠO";
            this.btnCreate.FillColor = Color.White; // Màu trắng nổi bật trên nền đen
            this.btnCreate.ForeColor = Color.Black; // Chữ đen tương phản
            this.btnCreate.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnCreate.BorderRadius = 22; // Bo tròn
            this.btnCreate.Location = new Point(125, 140);
            this.btnCreate.Size = new Size(150, 45);
            this.btnCreate.Cursor = Cursors.Hand;
            this.btnCreate.HoverState.FillColor = Color.FromArgb(230, 230, 230); // Hover hơi xám

            // Add Controls
            this.Controls.Add(btnClose);
            this.Controls.Add(lblTitle);
            this.Controls.Add(txtName);
            this.Controls.Add(btnCreate);
        }
    }
}