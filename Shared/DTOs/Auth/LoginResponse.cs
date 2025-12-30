using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty; // JWT token sau khi dang nhap thanh cong
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        // Thong tin goi dang ki hien tai de Client biet
        public string PlanName { get; set; } = string.Empty;
        public int PlanId { get; set; }
        public DateTime? SubscriptionExpireDate { get; set; }
    }
}