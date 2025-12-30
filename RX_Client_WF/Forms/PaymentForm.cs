using Guna.UI2.WinForms;
using RX_Client_WF.Utils;
using RX_Client_WF.Services;
using Shared.DTOs.Subscription;
using Shared.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    public partial class PaymentForm : Form
    {
        private readonly ApiService _apiService;

        public PaymentForm()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadPlans();
        }

        private void LoadPlans()
        {
            // Tạo 3 gói cước (Thực tế nên lấy từ DB, ở đây hardcode để demo giao diện nhanh)
            flowPlans.Controls.Add(CreatePlanCard("STANDARD", "Miễn phí", "128kbps\nCó quảng cáo\nKhông nhạc độc quyền", Color.Gray, 1));
            flowPlans.Controls.Add(CreatePlanCard("LIKE A PRO", "29.000đ/tháng", "256kbps AAC\nKhông quảng cáo\nNghe Offline", Color.FromArgb(29, 185, 84), 2));
            flowPlans.Controls.Add(CreatePlanCard("AUDIOPHILE", "59.000đ/tháng", "Lossless (FLAC)\nDemo Độc Quyền\nÂm thanh Hi-Res", Color.Gold, 3));
        }

        private Guna2Panel CreatePlanCard(string name, string price, string features, Color color, int planId)
        {
            Guna2Panel card = new Guna2Panel
            {
                Size = new Size(250, 380),
                Margin = new Padding(10),
                FillColor = Color.FromArgb(35, 35, 35),
                BorderRadius = 10,
                BorderColor = color,
                BorderThickness = 2
            };

            Label lblName = new Label { Text = name, ForeColor = color, Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(20, 30), AutoSize = true };
            Label lblPrice = new Label { Text = price, ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(20, 70), AutoSize = true };
            Label lblFeatures = new Label { Text = features, ForeColor = Color.LightGray, Font = new Font("Segoe UI", 10), Location = new Point(20, 130), Size = new Size(210, 100) };

            Guna2Button btnBuy = new Guna2Button
            {
                Text = planId == 1 ? "ĐANG DÙNG" : "NÂNG CẤP",
                FillColor = color,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(200, 45),
                Location = new Point(25, 300),
                BorderRadius = 22,
                Enabled = planId != 1 // Gói Free thì không cần mua
            };

            // Sự kiện mua
            if (planId > 1)
            {
                btnBuy.Click += async (s, e) =>
                {
                    if (MessageBox.Show($"Bạn muốn nâng cấp lên gói {name}?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var request = new UpgradeRequest1 { PlanId = planId, PaymentMethod = PaymentMethod.Momo.ToString() }; // Demo Momo

                        // Gọi API Payment (Cần tạo PaymentController bên server trả về dynamic hoặc PaymentResponse)
                        var result = await _apiService.PostAsync<dynamic>("/api/payment/upgrade", request);

                        if (result != null)
                        {
                            MessageBox.Show("Thanh toán thành công! Vui lòng đăng nhập lại để cập nhật quyền lợi.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Thanh toán thất bại.");
                        }
                    }
                };
            }

            card.Controls.Add(lblName);
            card.Controls.Add(lblPrice);
            card.Controls.Add(lblFeatures);
            card.Controls.Add(btnBuy);

            return card;
        }
    }
}