using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.UserControls
{
    public partial class UCAlbumCard : UserControl
    {
        public UCAlbumCard()
        {
            InitializeComponent();

            // Hiệu ứng Hover nhẹ
            this.MouseEnter += (s, e) => this.BackColor = Color.FromArgb(25, 25, 25);
            this.MouseLeave += (s, e) => this.BackColor = Color.Transparent;
        }

        public void SetData(string title, string subtitle, string imageUrl)
        {
            lblTitle.Text = title;
            lblSubTitle.Text = subtitle;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                picCover.LoadAsync(imageUrl);
            }
        }
    }
}