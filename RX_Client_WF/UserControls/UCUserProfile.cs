using Guna.UI2.WinForms;
using RX_Client_WF.Forms;
using RX_Client_WF.Utils;
using Shared.Enums;
using RX_Client_WF.Services;
using Shared.DTOs;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCUserProfile : UserControl
    {
        private readonly ApiService _apiService;
        public event Action<SongDto> SongClicked; // New Event

        public UCUserProfile()
        {
            InitializeComponent();
            _apiService = new ApiService();

            // Click avatar de doi anh
            picAvatar.Cursor = Cursors.Hand;
            picAvatar.Click += async (s, e) => 
            {
               using (OpenFileDialog ofd = new OpenFileDialog())
               {
                   ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.webp";
                   if (ofd.ShowDialog() == DialogResult.OK)
                   {
                       bool success = await _apiService.UploadAvatarAsync(ofd.FileName);
                       if (success)
                       {
                           MessageBox.Show("Cập nhật ảnh đại diện thành công!");
                           LoadUserProfile(); // Reload to fetch new URL
                       }
                       else
                       {
                           MessageBox.Show("Lỗi cập nhật ảnh.");
                       }
                   }
               }
            };

            // Gắn sự kiện nâng cấp
            btnUpgrade.Click += (s, e) =>
            {
                PaymentForm payForm = new PaymentForm();
                payForm.ShowDialog();
                // Sau khi mua xong thì reload lại profile
                LoadUserProfile();
            };
        }

        // Tự động load khi Control hiện lên
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadUserProfile();
        }

        private async void LoadUserProfile()
        {
            try
            {
                // Gọi API lấy thông tin chi tiết
                var profile = await _apiService.GetAsync<UserProfileDto>("/api/users/profile");

                if (profile != null)
                {
                    // 1. Basic Info
                    lblUsername.Text = profile.Username;
                    if (!string.IsNullOrEmpty(profile.ProfileImageUrl))
                    {
                         picAvatar.LoadAsync(profile.ProfileImageUrl);
                    }

                    // Căn chỉnh vị trí tích xanh (nếu có) ngay sau tên
                    // MeasureString giúp đo độ dài tên để đặt icon chính xác
                    using (Graphics g = CreateGraphics())
                    {
                        SizeF size = g.MeasureString(lblUsername.Text, lblUsername.Font);
                        iconVerified.Location = new Point(lblUsername.Location.X + (int)size.Width + 10, lblUsername.Location.Y + 10);
                    }

                    // 2. Role Info
                    if (profile.IsArtist)
                    {
                        lblRole.Text = "Nghệ sĩ";
                        lblRole.ForeColor = Color.Gold; // Artist màu vàng cho sang

                        // Hiện thông số Follower
                        lblFollowers.Visible = true;
                        lblFollowersTitle.Visible = true;
                        lblFollowers.Text = FormatNumber(profile.FollowersCount);

                        // Tích xanh
                        iconVerified.Visible = profile.IsVerified;
                        // iconVerified.Image = ... (Set icon checkmark.png ở đây nếu có resource)
                    }
                    else
                    {
                        lblRole.Text = "Người nghe";
                        lblRole.ForeColor = Color.Gray;
                        lblFollowers.Visible = false;
                        lblFollowersTitle.Visible = false;
                        iconVerified.Visible = false;
                    }

                    // 3. Plan Info (Quan trọng)
                    lblPlanName.Text = profile.PlanName.ToUpper();

                    // Logic màu sắc và nút bấm dựa trên gói cước
                    if (profile.PlanName.Contains("Audiophile", StringComparison.OrdinalIgnoreCase))
                    {
                        lblPlanName.ForeColor = Color.Gold;
                        pnPlanCard.BorderColor = Color.Gold; // Viền vàng
                        btnUpgrade.Text = "Gia Hạn";
                        btnUpgrade.FillColor = Color.FromArgb(40, 40, 40);
                        btnUpgrade.ForeColor = Color.Gold;
                    }
                    else if (profile.PlanName.Contains("Pro", StringComparison.OrdinalIgnoreCase))
                    {
                        lblPlanName.ForeColor = Color.FromArgb(29, 185, 84); // Xanh Spotify
                        pnPlanCard.BorderColor = Color.FromArgb(29, 185, 84);
                        btnUpgrade.Text = "Nâng Cấp"; // Lên Audiophile
                        btnUpgrade.FillColor = Color.White;
                    }
                    else
                    {
                        lblPlanName.ForeColor = Color.White;
                        pnPlanCard.BorderColor = Color.FromArgb(40, 40, 40); // Viền xám
                        btnUpgrade.Text = "Nâng Cấp";
                    }

                    // 4. Hạn sử dụng
                    if (profile.SubscriptionExpireDate.HasValue)
                    {
                        var expire = profile.SubscriptionExpireDate.Value;
                        var daysLeft = (expire - DateTime.Now).Days;

                        lblExpireDate.Text = $"Hết hạn: {expire:dd/MM/yyyy}";

                        if (daysLeft < 5)
                        {
                            lblExpireDate.ForeColor = Color.Red; // Cảnh báo sắp hết hạn
                            lblExpireDate.Text += $" (Còn {daysLeft} ngày)";
                        }
                    }
                    else
                    {
                        lblExpireDate.Text = "Vĩnh viễn (Gói miễn phí)";
                    }

                    // 5. Hien thi bai hat da upload (Neu la Artist)
                    pnSongs.Controls.Clear();
                    if (profile.IsArtist && profile.UploadedSongs.Count > 0)
                    {
                        pnSongs.Visible = true;
                        
                        //Them Label Tieu de
                        Label lblSongsTitle = new Label();
                        lblSongsTitle.Text = "BÀI HÁT ĐÃ ĐĂNG (" + profile.UploadedSongs.Count + ")";
                        lblSongsTitle.ForeColor = Color.White;
                        lblSongsTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                        lblSongsTitle.AutoSize = true;
                        lblSongsTitle.Margin = new Padding(0, 0, 0, 10);
                        pnSongs.Controls.Add(lblSongsTitle);

                        int idx = 1;
                        foreach (var song in profile.UploadedSongs)
                        {
                            var item = new UCSongItem();
                            // Enable Edit Mode cho chu so huu
                            item.SetData(song, idx++, true);
                            item.Width = pnSongs.Width - 25;
                            
                            // Reload lai profile neu co thay doi
                            item.Click += (s, e) => SongClicked?.Invoke(song); // Hook Play Event
                            item.ItemDeleted += (s, e) => LoadUserProfile();
                            item.ItemUpdated += (s, e) => LoadUserProfile();

                            pnSongs.Controls.Add(item);
                        }
                    }
                    else
                    {
                        pnSongs.Visible = false;
                    }
                }
                else
                {
                     lblUsername.Text = "Lỗi tải Profile";
                     lblRole.Text = "Vui lòng kiểm tra kết nối";
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ rớt mạng)
                lblUsername.Text = "Lỗi tải dữ liệu";
            }
        }

        // Helper format số (1000 -> 1K)
        private string FormatNumber(int num)
        {
            if (num >= 1000000) return (num / 1000000D).ToString("0.#") + "M";
            if (num >= 1000) return (num / 1000D).ToString("0.#") + "K";
            return num.ToString();
        }
    }
}