using RX_Client_WF.Utils;
using Newtonsoft.Json;
using RestSharp;
using Shared.DTOs.Auth;
using System;
using System.Threading.Tasks;

namespace RX_Client_WF.Services
{
    public class AuthService
    {
        private readonly RestClient _client;

        public AuthService()
        {
            _client = new RestClient(Config.BaseUrl);
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var request = new RestRequest("/api/auth/login", Method.Post);
            request.AddJsonBody(new LoginRequest1 { Username = username, Password = password });

            try
            {
                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful && response.Content != null)
                {
                    // Parse dữ liệu Server trả về (Token, User Info, Plan Info)
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response.Content);

                    if (loginResponse != null)
                    {
                        // Lưu thông tin vào Session (Singleton) để dùng toàn app
                        Session.StartSession(loginResponse);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                // Log lỗi
            }

            return false;
        }

        public void Logout()
        {
            Session.ClearSession();
        }
    }
}