namespace RX_Client_WF.Utils
{
    public static class Config
    {
        // Địa chỉ API Server (đổi port nếu cần)
        public const string BaseUrl = "https://localhost:7210";

        // Đường dẫn thư mục ảnh trên Server
        public const string ImageUrl = BaseUrl + "/images/";

        // Đường dẫn stream nhạc cơ bản
        public const string StreamUrl = BaseUrl + "/api/songs/{0}/stream";
    }
}