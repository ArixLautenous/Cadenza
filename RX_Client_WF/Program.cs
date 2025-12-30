using RX_Client_WF.Utils;
using RX_Client_WF.Forms;
using System;
using System.Windows.Forms;

namespace RX_Client_WF
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Cấu hình hiển thị chuẩn cho màn hình DPI cao (màn 2K/4K)
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Bắt đầu bằng màn hình đăng nhập
            LoginForm loginForm = new LoginForm();

            // Logic: Chạy LoginForm dưới dạng Dialog. 
            // Nếu User đăng nhập thành công trong LoginForm -> LoginForm sẽ ẩn đi và mở MainForm
            // Ở đây ta dùng Application.Run(loginForm) đơn giản, 
            // việc chuyển sang MainForm đã được xử lý trong code của LoginForm (this.Hide(), new MainForm().ShowDialog())
            Application.Run(loginForm);
        }
    }
}