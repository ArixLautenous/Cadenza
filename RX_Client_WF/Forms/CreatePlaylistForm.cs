using RX_Client_WF.Services;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RX_Client_WF.Forms
{
    public partial class CreatePlaylistForm : Form
    {
        private readonly ApiService _apiService;

        // Sự kiện: Báo cho Form cha (UCLibrary) biết là tạo xong rồi, hãy reload lại list đi!
        public event EventHandler PlaylistCreated;

        public CreatePlaylistForm()
        {
            InitializeComponent();
            _apiService = new ApiService();

            // Gắn sự kiện Click cho nút
            btnCreate.Click += BtnCreate_Click;

            // Xử lý nút Đóng (X)
            btnClose.Click += (s, e) => this.Close();

            // Xử lý phím Enter trong ô nhập tên -> Tương đương bấm nút Tạo
            txtName.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter) BtnCreate_Click(s, e);
            };
        }

        private async void BtnCreate_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng đặt tên cho playlist.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. UI Feedback (Chặn bấm liên tục)
            btnCreate.Enabled = false;
            btnCreate.Text = "ĐANG TẠO...";

            try
            {
                // 3. Chuẩn bị dữ liệu gửi lên Server
                // Server đang mong đợi body là { "name": "Tên playlist" }
                // Ta dùng anonymous object cho nhanh
                var request = new { Name = txtName.Text };

                // 4. Gọi API POST /api/users/playlists
                // Dùng dynamic vì ta chỉ cần check null (thành công) hay không
                var result = await _apiService.PostAsync<dynamic>("/api/users/playlists", request);

                if (result != null)
                {
                    // Thành công!
                    MessageBox.Show("Đã tạo playlist mới thành công!", "Tuyệt vời", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Kích hoạt sự kiện để Form cha reload lại danh sách
                    PlaylistCreated?.Invoke(this, EventArgs.Empty);

                    this.Close(); // Đóng form
                }
                else
                {
                    MessageBox.Show("Tạo thất bại. Vui lòng thử lại sau.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Reset lại nút nếu thất bại (hoặc form chưa đóng kịp)
                if (!this.IsDisposed)
                {
                    btnCreate.Enabled = true;
                    btnCreate.Text = "TẠO";
                }
            }
        }
    }
}