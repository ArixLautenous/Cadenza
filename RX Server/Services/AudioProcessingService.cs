using System.Diagnostics;

namespace RX_Server.Services
{
    public interface IAudioProcessingService
    {
        Task SeparateAudioAsync(int songId, string inputFilePath);
    }

    public class AudioProcessingService : IAudioProcessingService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IServiceScopeFactory _scopeFactory;

        public AudioProcessingService(IWebHostEnvironment env, IServiceScopeFactory scopeFactory)
        {
            _env = env;
            _scopeFactory = scopeFactory;
        }

        public async Task SeparateAudioAsync(int songId, string inputFilePath)
        {
            // Chạy trong Task.Run để không block thread chính nếu gọi await (nhưng ở Controller nên gọi fire-and-forget cẩn thận)
            // Tốt nhất là Controller gọi hàm này và không await nếu muốn trả response ngay.
            
            try
            {
                string scriptPath = Path.Combine(_env.ContentRootPath, "Python", "split_audio.py");
                string outputDir = Path.Combine(_env.WebRootPath, "audio", "separated");

                // Tạo thư mục output nếu chưa có
                Directory.CreateDirectory(outputDir);

                // Cấu hình Process
                var startInfo = new ProcessStartInfo
                {
                    FileName = "python", 
                    Arguments = $"-u \"{scriptPath}\" \"{inputFilePath}\" \"{outputDir}\"", // Thêm -u để unbuffered IO
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process { StartInfo = startInfo };
                
                // StringBuilder dể gom output cuối cùng
                var outputBuilder = new System.Text.StringBuilder();
                var errorBuilder = new System.Text.StringBuilder();

                // Lắng nghe output realtime
                process.OutputDataReceived += (sender, args) => 
                {
                    if (args.Data != null)
                    {
                        Console.WriteLine($"[Py] {args.Data}");
                        outputBuilder.AppendLine(args.Data);
                    }
                };

                process.ErrorDataReceived += (sender, args) => 
                {
                    if (args.Data != null)
                    {
                        // Demucs/Whisper thường in progress bar ra stderr
                        Console.WriteLine($"[Py Err] {args.Data}"); 
                        errorBuilder.AppendLine(args.Data);
                    }
                };

                Console.WriteLine($"[AudioProcessing] Starting Demucs for Song {songId}...");
                process.Start();
                
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                try 
                { 
                    process.PriorityClass = ProcessPriorityClass.BelowNormal; 
                } 
                catch { }

                await process.WaitForExitAsync();

                string output = outputBuilder.ToString();
                string error = errorBuilder.ToString();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"[Demucs Error] ExitCode: {process.ExitCode}");
                }
                
                // Phân tích Output để lấy đường dẫn file (Logic cũ)
                // ...

                // Phân tích Output để lấy đường dẫn file
                // Format mong đợi từ script python:
                // OUTPUT_PATHS:
                // VOCAL|path
                // INSTRUMENTAL|path

                string vocalPath = null;
                string instPath = null;
                string lyricsPath = null;

                var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (line.StartsWith("VOCAL|"))
                    {
                        vocalPath = line.Substring(6).Trim();
                    }
                    else if (line.StartsWith("INSTRUMENTAL|"))
                    {
                        instPath = line.Substring(13).Trim();
                    }
                    else if (line.StartsWith("LYRICS_FILE|"))
                    {
                        lyricsPath = line.Substring(12).Trim();
                    }
                }

                if (!string.IsNullOrEmpty(vocalPath) && !string.IsNullOrEmpty(instPath))
                {
                    string webRoot = _env.WebRootPath;
                    
                    string relativeVocal = Path.GetRelativePath(webRoot, vocalPath).Replace("\\", "/");
                    string relativeInst = Path.GetRelativePath(webRoot, instPath).Replace("\\", "/");
                    
                    // Đọc nội dung Lyrics nếu có
                    string lyricsContent = null;
                    if (!string.IsNullOrEmpty(lyricsPath) && File.Exists(lyricsPath))
                    {
                        lyricsContent = await File.ReadAllTextAsync(lyricsPath);
                    }

                    // Cập nhật Database
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<RX_Server.Data.AppDbContext>();
                        var song = await context.Songs.FindAsync(songId);
                        if (song != null)
                        {
                            song.VocalUrl = relativeVocal;
                            song.InstrumentUrl = relativeInst;

                            if (!string.IsNullOrEmpty(lyricsContent)) 
                            {
                                song.Lyrics = lyricsContent;
                            }
                            
                            await context.SaveChangesAsync();
                            Console.WriteLine($"[Audio Processing Success] Song {songId} separated & transcribed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioProcessing Error] Song {songId}: {ex.Message}");
            }
        }
    }
}
