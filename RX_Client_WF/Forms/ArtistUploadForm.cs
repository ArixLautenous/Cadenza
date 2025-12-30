using RX_Client_WF.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RX_Client_WF.Forms
{
    public partial class ArtistUploadForm : Form
    {
        private readonly ApiService _apiService;
        private string _audioFilePath;
        private string _imageFilePath;

        public ArtistUploadForm()
        {
            InitializeComponent();
            _apiService = new ApiService();

            // Demo Genre Data
            cbGenre.Items.AddRange(new object[] { "Pop", "Rock", "Ballad", "Indie", "Rap" });
            cbGenre.SelectedIndex = 0;

            btnSelectAudio.Click += BtnSelectAudio_Click;
            btnSelectImage.Click += BtnSelectImage_Click;
            btnUpload.Click += BtnUpload_Click;
        }

        private void BtnSelectAudio_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "Audio Files|*.mp3;*.wav;*.flac;*.m4a;*.aac;*.wma;*.ogg" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _audioFilePath = dlg.FileName;
                txtAudioPath.Text = System.IO.Path.GetFileName(_audioFilePath);
            }
        }

        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "Image Files|*.jpg;*.png;*.jpeg;*.bmp;*.gif;*.webp" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _imageFilePath = dlg.FileName;
                txtImagePath.Text = System.IO.Path.GetFileName(_imageFilePath);
            }
        }

        private async void BtnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(_audioFilePath) || string.IsNullOrEmpty(_imageFilePath))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            btnUpload.Enabled = false;
            btnUpload.Text = "ĐANG UPLOAD...";
            lblStatus.Text = "Đang xử lý file nhạc, vui lòng đợi (có thể mất 1-2 phút)...";

            try
            {
                int genreId = cbGenre.SelectedIndex + 1; // Map index to ID (0->1, 1->2...)

                bool success = await _apiService.UploadAsync(
                    "/api/artist/upload",
                    txtTitle.Text,
                    genreId,
                    _audioFilePath,
                    _imageFilePath
                );

                if (success)
                {
                    MessageBox.Show("Upload thành công!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Upload thất bại. Vui lòng thử lại.");
                    lblStatus.Text = "Lỗi khi upload.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
            finally
            {
                btnUpload.Enabled = true;
                btnUpload.Text = "UPLOAD NGAY";
            }
        }
    }
}