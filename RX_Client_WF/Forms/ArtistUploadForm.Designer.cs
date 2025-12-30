using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    partial class ArtistUploadForm
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2TextBox txtTitle;
        private Guna2ComboBox cbGenre;
        private Guna2CheckBox chkExclusive;
        private Guna2Button btnSelectAudio;
        private Guna2Button btnSelectImage;
        private Guna2TextBox txtAudioPath;
        private Guna2TextBox txtImagePath;
        private Guna2Button btnUpload;
        private Label lblTitle;
        private Guna2ControlBox btnClose;
        private Guna2DragControl dragControl;
        private Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(500, 600);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(20, 20, 20); // Nền xám đậm

            this.dragControl = new Guna2DragControl(this.components);
            this.dragControl.TargetControl = this;

            this.btnClose = new Guna2ControlBox { Anchor = AnchorStyles.Top | AnchorStyles.Right, FillColor = Color.Transparent, Location = new Point(455, 0) };

            this.lblTitle = new Label();
            this.lblTitle.Text = "Đăng Tải Nhạc Mới";
            this.lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(30, 30);
            this.lblTitle.AutoSize = true;

            // 1. Tên bài hát
            this.txtTitle = new Guna2TextBox();
            this.txtTitle.PlaceholderText = "Tên bài hát";
            this.txtTitle.FillColor = Color.FromArgb(40, 40, 40);
            this.txtTitle.ForeColor = Color.White;
            this.txtTitle.BorderThickness = 0;
            this.txtTitle.BorderRadius = 4;
            this.txtTitle.Location = new Point(30, 90);
            this.txtTitle.Size = new Size(440, 45);
            this.txtTitle.Font = new Font("Segoe UI", 11);

            // 2. Thể loại
            this.cbGenre = new Guna2ComboBox();
            this.cbGenre.Text = "Chọn thể loại";
            this.cbGenre.FillColor = Color.FromArgb(40, 40, 40);
            this.cbGenre.ForeColor = Color.White;
            this.cbGenre.BorderThickness = 0;
            this.cbGenre.BorderRadius = 4;
            this.cbGenre.Location = new Point(30, 150);
            this.cbGenre.Size = new Size(440, 45);
            // Items add in logic

            // 3. Checkbox Exclusive
            this.chkExclusive = new Guna2CheckBox();
            this.chkExclusive.Text = "Nội dung độc quyền (Audiophile Only)";
            this.chkExclusive.ForeColor = Color.Gold;
            this.chkExclusive.Font = new Font("Segoe UI", 10);
            this.chkExclusive.Location = new Point(35, 210);
            this.chkExclusive.Size = new Size(400, 30);

            // 4. File Audio
            this.btnSelectAudio = CreateButton("Chọn File Nhạc", 250, Color.Gray);
            this.txtAudioPath = CreateReadOnlyBox(250);

            // 5. File Image
            this.btnSelectImage = CreateButton("Chọn Ảnh Bìa", 310, Color.Gray);
            this.txtImagePath = CreateReadOnlyBox(310);

            // 6. Upload Button
            this.btnUpload = CreateButton("UPLOAD NGAY", 400, Color.FromArgb(29, 185, 84));
            this.btnUpload.Width = 440; // Full width

            // 7. Status Label
            this.lblStatus = new Label();
            this.lblStatus.Text = "";
            this.lblStatus.ForeColor = Color.Yellow;
            this.lblStatus.Location = new Point(30, 460);
            this.lblStatus.AutoSize = true;

            this.Controls.Add(btnClose);
            this.Controls.Add(lblTitle);
            this.Controls.Add(txtTitle);
            this.Controls.Add(cbGenre);
            this.Controls.Add(chkExclusive);
            this.Controls.Add(btnSelectAudio);
            this.Controls.Add(txtAudioPath);
            this.Controls.Add(btnSelectImage);
            this.Controls.Add(txtImagePath);
            this.Controls.Add(btnUpload);
            this.Controls.Add(lblStatus);
        }

        private Guna2Button CreateButton(string text, int y, Color color)
        {
            return new Guna2Button
            {
                Text = text,
                FillColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, y),
                Size = new Size(140, 45),
                BorderRadius = 4
            };
        }

        private Guna2TextBox CreateReadOnlyBox(int y)
        {
            return new Guna2TextBox
            {
                ReadOnly = true,
                FillColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Gray,
                BorderThickness = 0,
                Location = new Point(180, y),
                Size = new Size(290, 45)
            };
        }
    }
}