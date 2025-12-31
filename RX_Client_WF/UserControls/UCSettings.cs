using Guna.UI2.WinForms;
using RX_Client_WF.Forms;
using RX_Client_WF.Services;
using RX_Client_WF.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCSettings : UserControl
    {
        private ApiService _apiService;
        private AudioPlayer _player; // Reference to Player

        public event EventHandler LogoutClicked;

        // Constructor update
        public UCSettings(AudioPlayer player = null)
        {
            _player = player;
            InitializeComponent();
            _apiService = new ApiService();
        }

        private Panel pnContent;

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(3, 3, 3);
            this.Dock = DockStyle.Fill;
            
            // 1. Tạo Panel cuộn chứa toàn bộ nội dung
            pnContent = new Panel();
            pnContent.Dock = DockStyle.Fill;
            pnContent.AutoScroll = true;
            this.Controls.Add(pnContent);

            var lblHeader = new Label
            {
                Text = "Cài Đặt",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(40, 30),
                AutoSize = true
            };
            pnContent.Controls.Add(lblHeader);

            // --- SECTION 1: AUDIO (Equalizer) ---
            int currentY = 100;
            
            var lblAudio = new Label
            {
                Text = "Equalizer (Bộ chỉnh âm)",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Gray,
                Location = new Point(40, currentY),
                AutoSize = true
            };
            pnContent.Controls.Add(lblAudio);
            
            currentY += 40;
            
            // Init Equalizer Panel
            InitEqualizerUI(currentY);
            
            currentY += 220; // Dịch xuống sau khi vẽ xong EQ

            // --- SECTION 2: ACCOUNT ---
            var lblAccount = new Label
            {
                Text = "Tài khoản",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Gray,
                Location = new Point(40, currentY),
                AutoSize = true
            };
            pnContent.Controls.Add(lblAccount);
            
            currentY += 40;

            // Controls
            var btnChangeUsername = CreateSettingButton("Đổi tên đăng nhập", currentY);
            btnChangeUsername.Click += (s, e) => 
            {
                var frm = new ChangeUsernameForm();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LogoutClicked?.Invoke(this, EventArgs.Empty);
                }
            };
            pnContent.Controls.Add(btnChangeUsername);
            currentY += 60;

            var btnChangePass = CreateSettingButton("Đổi mật khẩu", currentY);
            btnChangePass.Click += (s, e) => new ChangePasswordForm().ShowDialog();
            pnContent.Controls.Add(btnChangePass);
            currentY += 60;

            var btnUpdateEmail = CreateSettingButton("Cập nhật / Liên kết Email", currentY);
            btnUpdateEmail.Click += (s, e) => new UpdateEmailForm().ShowDialog();
            pnContent.Controls.Add(btnUpdateEmail);
            currentY += 80;

            // --- LOGOUT ---
            var btnLogout = new Guna2Button
            {
                Text = "Đăng xuất tài khoản",
                Location = new Point(40, currentY),

                Size = new Size(200, 45),
                FillColor = Color.Transparent,
                BorderColor = Color.IndianRed,
                BorderThickness = 1,
                ForeColor = Color.IndianRed,
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogout.Click += (s, e) => LogoutClicked?.Invoke(this, EventArgs.Empty);
            pnContent.Controls.Add(btnLogout);

            // Padding bottom để scroll thoải mái
            Label lblPadding = new Label { Location = new Point(0, currentY + 100), Height = 1, Width = 1 };
            pnContent.Controls.Add(lblPadding);
        }

         private void InitEqualizerUI(int yPos = 370)
        {
            Panel pnEq = new Panel 
            { 
                Location = new Point(40, yPos), 
                Size = new Size(600, 200),
                BorderStyle = BorderStyle.None
            };

            string[] bandLabels = { "31Hz", "62Hz", "125Hz", "250Hz", "500Hz", "1K", "2K", "4K", "8K", "16K" };
            
            for (int i = 0; i < 10; i++)
            {
                int bandIndex = i; // Closure
                
                // Vertical Slider
                TrackBar slider = new TrackBar
                {
                    Location = new Point(i * 60, 0),
                    Size = new Size(45, 150),
                    Orientation = Orientation.Vertical,
                    Minimum = -20,
                    Maximum = 20,
                    Value = 0,
                    TickStyle = TickStyle.None,
                    Cursor = Cursors.Hand
                };
                
                // Label Freq
                Label lblFreq = new Label 
                { 
                    Text = bandLabels[i], 
                    ForeColor = Color.LightGray, 
                    Location = new Point((i * 60) - 5, 160), 
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8)
                };
                
                // Event change
                slider.Scroll += (s, e) => 
                {
                    _player?.SetEq(bandIndex, slider.Value);
                };

                pnEq.Controls.Add(slider);
                pnEq.Controls.Add(lblFreq);
            }

            // Chú ý: Add vào pnContent thay vì this.Controls
            if (pnContent != null) 
                pnContent.Controls.Add(pnEq);
            else 
                this.Controls.Add(pnEq); // Fallback
        }

        private Guna2Button CreateSettingButton(string text, int y)
        {
            return new Guna2Button
            {
                Text = text,
                Location = new Point(40, y),
                Size = new Size(300, 45),
                FillColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                TextAlign = HorizontalAlignment.Left,
                BorderRadius = 5,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
        }
    }
}
