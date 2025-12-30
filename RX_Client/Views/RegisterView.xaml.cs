using RX_Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace RX_Client.Views
{
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void TxtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm) vm.Password = ((PasswordBox)sender).Password;
        }

        private void TxtConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm) vm.ConfirmPassword = ((PasswordBox)sender).Password;
        }
    }
}