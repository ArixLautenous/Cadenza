using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Enums
{
    //Dinh nghia cac vai tro nguoi dung trong he thong
    public enum UserRole
    {
        Listener = 0, // Người nghe nhạc
        Producer = 1, // Nhà sản xuất nội dung
        Composer = 2, // Nhạc sĩ
        Singer = 3,   // Ca sĩ
        Admin = 99     // Quản trị viên hệ thống
    }
}