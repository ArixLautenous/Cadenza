using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.DTOs.Auth
{
    public class RegisterRequest1
    {
        [Required]
        [MinLength(3, ErrorMessage = "Tên tài khoản phải có ít nhất 3 ký tự.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare ("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        //Vai tro mac dinh la Listener
        [Required]
        public Shared.Enums.UserRole Role { get; set; } = Shared.Enums.UserRole.Listener;
    }
}
