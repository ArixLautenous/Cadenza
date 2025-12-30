using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.DTOs.Auth
{
    public class LoginRequest1
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản.")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        public string Password { get; set; } = string.Empty;
    }
}