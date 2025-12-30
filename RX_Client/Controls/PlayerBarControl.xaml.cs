using RX_Client.ViewModels;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RX_Client.Controls
{
    public partial class PlayerBarControl : UserControl
    {
        public PlayerBarControl()
        {
            InitializeComponent();
        }

        // Sự kiện khi người dùng kéo thanh nhạc xong mới seek (tránh giật)
        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (DataContext is PlayerViewModel vm && sender is Slider slider)
            {
                vm.SeekToCommand.Execute(slider.Value);
            }
        }
    }
}