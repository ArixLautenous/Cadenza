using Newtonsoft.Json;
using RestSharp;
using RX_Client_WF.Utils;
using System;
using System.Net.Http; // Cần thiết cho SSL
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RX_Client_WF.Services
{
    public class ApiService
    {
        private readonly RestClient _client;

        public ApiService()
        {
            // --- CẤU HÌNH QUAN TRỌNG ĐỂ SỬA LỖI SSL ---
            // "RemoteCertificateValidationCallback... true" nghĩa là: 
            // "Kệ xác chứng chỉ bảo mật, cứ cho kết nối đi!"
            var options = new RestClientOptions(Config.BaseUrl)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            _client = new RestClient(options);
        }

        public async Task<bool> UploadAvatarAsync(string imagePath)
        {
            var request = new RestRequest("/api/users/avatar", Method.Post);
            AddAuthHeader(request);
            request.AddFile("image", imagePath);

            try
            {
                var response = await _client.ExecuteAsync(request);
                return response.IsSuccessful;
            }
            catch
            {
                return false;
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(body);
            AddAuthHeader(request);

            try
            {
                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    if (string.IsNullOrEmpty(response.Content)) return default;
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
                else
                {
                    // Xử lý các lỗi HTTP (400, 401, 500)
                    string msg = $"Lỗi {response.StatusCode}: ";

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        msg += "Phiên đăng nhập hết hạn.";
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        msg += "Dữ liệu không hợp lệ.";
                    else
                        msg += response.ErrorMessage ?? response.Content;

                    MessageBox.Show(msg, "Lỗi Server");
                    return default;
                }
            }
            catch (Exception ex)
            {
                // --- BẮT LỖI KẾT NỐI CHI TIẾT ---
                // Đây là chỗ sẽ hiện ra nguyên nhân thực sự
                string debugInfo = ex.Message;
                if (ex.InnerException != null)
                {
                    debugInfo += $"\n\nChi tiết kỹ thuật: {ex.InnerException.Message}";
                }

                MessageBox.Show($"Không thể kết nối đến Server!\n\nNguyên nhân có thể:\n1. Server chưa chạy?\n2. Sai cổng (Port) trong Config.cs?\n3. Bị chặn SSL?\n\n{debugInfo}", "Lỗi Mạng Nghiêm Trọng");
                return default;
            }
        }

        // Các hàm GetAsync, UploadAsync bạn giữ nguyên logic cũ hoặc copy lại từ đây
        public async Task<T> GetAsync<T>(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            AddAuthHeader(request);

            try
            {
                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
                else
                {
                    // Xử lý lỗi tương tự PostAsync
                    string msg = $"Lỗi {response.StatusCode} khi lấy dữ liệu: {endpoint}\n";
                    msg += response.ErrorMessage ?? response.Content;
                    // Chỉ hiện MessageBox nếu lỗi nghiêm trọng hoặc cần thiết. 
                    // Để tránh spam popup khi load nhiều resources, có thể log ra Debug hoặc Status bar.
                    // Nhưng ở đây để debug cho User thấy, ta sẽ Show.
                    MessageBox.Show(msg, "Lỗi API Get");
                    return default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối API (Get): {ex.Message}");
                return default;
            }
        }

        public async Task<bool> UploadAsync(string endpoint, string title, int genreId, string audioPath, string imagePath)
        {
            var request = new RestRequest(endpoint, Method.Post);
            AddAuthHeader(request);
            request.AddParameter("Title", title);
            request.AddParameter("GenreId", genreId);
            request.AddParameter("IsExclusive", false);
            if (!string.IsNullOrEmpty(audioPath)) request.AddFile("AudioFile", audioPath);
            if (!string.IsNullOrEmpty(imagePath)) request.AddFile("ImageFile", imagePath);

            try
            {
                var response = await _client.ExecuteAsync(request);
                return response.IsSuccessful;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi upload: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var request = new RestRequest("/api/auth/forgot-password", Method.Post);
            request.AddJsonBody(email); // Server expect [FromBody] string email

            try
            {
                var response = await _client.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                    MessageBox.Show(response.Content ?? "Lỗi gửi request quên mật khẩu", "Lỗi");
                }
                return response.IsSuccessful;
            }
            catch { return false; }
        }

        public async Task<bool> ResetPasswordAsync(string email, string code, string newPass)
        {
            var request = new RestRequest("/api/auth/reset-password", Method.Post);
            request.AddJsonBody(new { Email = email, Code = code, NewPassword = newPass });

            try
            {
                var response = await _client.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                     MessageBox.Show(response.Content ?? "Lỗi đặt lại mật khẩu", "Lỗi");
                }
                return response.IsSuccessful;
            }
            catch { return false; }
        }

        public async Task<bool> PutAsync(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Put);
            request.AddJsonBody(body);
            AddAuthHeader(request);

            try
            {
                var response = await _client.ExecuteAsync(request);
                return response.IsSuccessful;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Delete);
            AddAuthHeader(request);

            try
            {
                var response = await _client.ExecuteAsync(request);
                return response.IsSuccessful;
            }
            catch
            {
                return false;
            }
        }

        private void AddAuthHeader(RestRequest request)
        {
            if (Session.IsLoggedIn && !string.IsNullOrEmpty(Session.Token))
            {
                request.AddHeader("Authorization", $"Bearer {Session.Token}");
            }
        }
    }
}