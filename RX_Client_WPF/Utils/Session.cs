using Shared.DTOs.Auth;
using Shared.Enums;

namespace RX_Client.Utils
{
    public static class Session
    {
        public static LoginResponse CurrentUser { get; private set; }
        public static string Token => CurrentUser?.Token;
        public static bool IsLoggedIn => CurrentUser != null;

        // Lưu thông tin khi đăng nhập thành công
        public static void StartSession(LoginResponse user)
        {
            CurrentUser = user;
        }

        // Xóa thông tin khi đăng xuất
        public static void ClearSession()
        {
            CurrentUser = null;
        }

        // Helper để check quyền nhanh
        public static bool IsArtist => CurrentUser?.Role == UserRole.Producer
                                    || CurrentUser?.Role == UserRole.Singer
                                    || CurrentUser?.Role == UserRole.Composer;

        public static bool IsAudiophile => CurrentUser?.PlanId == (int)PlanType.Audiophile;
    }
}
