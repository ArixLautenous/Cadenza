using RX_Client.ViewModels;
using System.Windows.Controls;

namespace RX_Client.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        // Xử lý binding PasswordBox thủ công (vì WPF chặn binding trực tiếp password vì lý do bảo mật)
        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
