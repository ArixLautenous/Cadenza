using NAudio.Wave;
using NAudio.Lame;
using Microsoft.Extensions.FileProviders.Embedded; 

namespace RX_Server.Services
{
    public class StreamingService : IStreamingService
    {
        public readonly IWebHostEnvironment _env;
        public StreamingService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task<string> GetAudioFilePathAsync(int songId, int userSubscriptionId)
        {
            string folder = "";
            string extension = ".mp3";

            //Logic chon folder va dinh dang file dua tren goi dang ki cua user
            switch (userSubscriptionId)
            {
                case 3: //Audiophile -> Uu tien Lossless (.flac)
                    folder = "lossless";
                    extension = ".flac";
                    // Kiem tra cac dinh dang file goc khac
                    var checkExts = new[] { ".flac", ".wav", ".mp3", ".m4a", ".aac", ".wma", ".ogg" };
                    foreach (var ext in checkExts)
                    {
                         string checkPath = Path.Combine(_env.WebRootPath, "audio", "lossless", $"{songId}{ext}");
                         if (File.Exists(checkPath))
                         {
                             extension = ext;
                             break;
                         }
                    }
                    break;
                case 2: //Like a Pro -> 256kbps AAC (.m4a)
                    folder = "256kbps";
                    extension = ".m4a";
                    break;
                case 1: //Standard -> 128kbps MP3 (.mp3)
                    folder = "128kbps";
                    extension = ".mp3";
                    break;
            }
            //Tao duong dan file tuyet doi
            string fullPath = Path.Combine(_env.WebRootPath, "audio", folder, $"{songId}{extension}");

            //Fallback 1: Neu chua co file Lossless thi tra ve file 256kbps
            if (!File.Exists(fullPath) && userSubscriptionId == 3)
            {
                fullPath = Path.Combine(_env.WebRootPath, "audio", "256kbps", $"{songId}.m4a");
            }
            //Falback 2: Neu chua co file 256kbps thi tra ve file 128kbps (Bat buoc)
            if (!File.Exists(fullPath))
            {
                fullPath = Path.Combine(_env.WebRootPath, "audio", "128kbps", $"{songId}.mp3");
            }
            return Task.FromResult(fullPath);
        }

        public Task DeleteSongFilesAsync(int songId)
        {
            try
            {
                var root = _env.WebRootPath;
                var filesToDelete = new List<string>
                {
                    Path.Combine(root, "audio", "lossless", $"{songId}.flac"),
                    Path.Combine(root, "audio", "lossless", $"{songId}.wav"),
                    Path.Combine(root, "audio", "lossless", $"{songId}.mp3"),
                    Path.Combine(root, "audio", "lossless", $"{songId}.m4a"),
                    Path.Combine(root, "audio", "256kbps", $"{songId}.m4a"),
                    Path.Combine(root, "audio", "128kbps", $"{songId}.mp3"),
                    Path.Combine(root, "images", "covers", $"{songId}.jpg"),
                    Path.Combine(root, "images", "covers", $"{songId}.png") 
                };

                foreach (var file in filesToDelete)
                {
                    if (File.Exists(file)) File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting files for song {songId}: {ex.Message}");
            }
            return Task.CompletedTask;
        }

        public async Task<double> ProcessUploadedFileAsync(IFormFile file, int songId)
        {
            try
            {
                //Tao thu muc
                string root = _env.WebRootPath;
                string pathLossless = Path.Combine(root, "audio", "lossless");
                string path256kbps = Path.Combine(root, "audio", "256kbps"); //AAC
                string path128kbps = Path.Combine(root, "audio", "128kbps"); //MP3

                Directory.CreateDirectory(pathLossless);
                Directory.CreateDirectory(path256kbps);
                Directory.CreateDirectory(path128kbps);

                //Luu file goc vao server
                string extension = Path.GetExtension(file.FileName).ToLower();
                string originalFilePath = Path.Combine(pathLossless, $"{songId}{extension}");

                using (var stream = new FileStream(originalFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Tinh thoi luong (Insert boi Antigravity)
                double durationInSeconds = 0;
                try 
                { 
                    using (var reader = new MediaFoundationReader(originalFilePath)) 
                    { 
                        durationInSeconds = reader.TotalTime.TotalSeconds; 
                    } 
                } 
                catch {}

                //Chuyen doi file sang cac chat luong khac
                //Thanh 256kbps AAC
                await Task.Run(() => ConvertToAac(originalFilePath, Path.Combine(path256kbps, $"{songId}.m4a")));
                //Thanh 128kbps MP3
                await Task.Run(() => ConvertToMp3(originalFilePath, Path.Combine(path128kbps, $"{songId}.mp3"), 128));
                return durationInSeconds;
            }
            catch (Exception ex)
            {
                //Log loi
                Console.WriteLine($"Lỗi xử lý file nhạc: {ex.Message}");
                return 0;
            }
        }
        //Ham chuyen doi sang AAC - Chi chay tren Windows do dung MediaFoundation
        private void ConvertToAac(string sourceFile, string destFile)
        {
            // MediaFoundationReader doc duoc FLAC, WAV va MP3
            using (var reader = new MediaFoundationReader(sourceFile))
            {
                try
                {
                    MediaFoundationEncoder.EncodeToAac(reader, destFile, 256000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi chuyển đổi sang AAC: {ex.Message}");
                }
            }
        }
        //Ham chuyen doi sang 128kbps
        private void ConvertToMp3(string sourceFile, string destFile, int bitrate)
        {
            using (var reader = new MediaFoundationReader(sourceFile))
            using (var writer = new LameMP3FileWriter(destFile, reader.WaveFormat, bitrate))
            {
                reader.CopyTo(writer);
            }
        }
    }
}