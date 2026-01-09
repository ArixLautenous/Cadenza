using RX_Client.Utils;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace RX_Client.Services
{
    public class ApiService
    {
        private readonly RestClient _client;

        public ApiService()
        {
            // Lấy Base URL từ Config (Ví dụ: http://localhost:5000)
            _client = new RestClient(Config.BaseUrl);
        }

        // Hàm gọi API GET (dùng cho Lấy danh sách nhạc, Profile...)
        public async Task<T> GetAsync<T>(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            AddAuthHeader(request);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful && response.Content != null)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }

            // Có thể throw exception hoặc return default tùy logic
            return default;
        }

        // Hàm gọi API POST (dùng cho Upload, Payment...)
        public async Task<T> PostAsync<T>(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(body);
            AddAuthHeader(request);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful && response.Content != null)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }

            return default;
        }

        // Hàm POST đặc biệt để Upload File (Multipart/form-data)
        public async Task<bool> UploadAsync(string endpoint, string title, string filePath, string imagePath)
        {
            var request = new RestRequest(endpoint, Method.Post);
            AddAuthHeader(request);

            // Thêm các trường dữ liệu text
            request.AddParameter("Title", title);
            // ... thêm các param khác như GenreId, AlbumId ...

            // Thêm file nhạc và ảnh
            request.AddFile("AudioFile", filePath);
            request.AddFile("ImageFile", imagePath);

            var response = await _client.ExecuteAsync(request);
            return response.IsSuccessful;
        }

        // Helper: Tự động gắn Token vào Header nếu đã đăng nhập
        private void AddAuthHeader(RestRequest request)
        {
            if (Session.IsLoggedIn)
            {
                request.AddHeader("Authorization", $"Bearer {Session.Token}");
            }
        }
    }
}
