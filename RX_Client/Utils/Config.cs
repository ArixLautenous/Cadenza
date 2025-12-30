namespace RX_Client.Utils
{
    public static class Config
    {
        // Địa chỉ API Server (đổi port nếu cần)
        public const string BaseUrl = "http://localhost:5000";

        // Đường dẫn thư mục ảnh trên Server
        public const string ImageUrl = BaseUrl + "/images/";

        // Đường dẫn stream nhạc cơ bản
        public const string StreamUrl = BaseUrl + "/api/songs/{0}/stream";
    }
}