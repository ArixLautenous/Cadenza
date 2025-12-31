using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RX_Server.Data;
using RX_Server.Entities;
using RX_Server.Services;
using Shared.Enums;
using System.Security.Claims;

namespace RX_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArtistController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStreamingService _streamingService;
        private readonly IAudioProcessingService _audioProcessingService;
        private readonly IWebHostEnvironment _env;

        public ArtistController(AppDbContext context, IStreamingService streamingService, IAudioProcessingService audioProcessingService, IWebHostEnvironment env)
        {
            _context = context;
            _streamingService = streamingService;
            _audioProcessingService = audioProcessingService;
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadSong([FromForm] SongUploadModel request)
        {
            //Kiem tra quyen
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.Role == UserRole.Listener)
                return StatusCode(403, "Bạn không có quyền đăng tải nhạc.");

            //Validate input
            if (request.AudioFile == null || request.AudioFile.Length == 0)
                return BadRequest("Vui lòng chọn file nhạc (.mp3, .m4a, .wav, .flac, .aac, .wma, .ogg, .opus)");

            if (request.ImageFile == null || request.ImageFile.Length == 0)
                return BadRequest("Vui lòng chọn ảnh bìa (.png, .jpg, .jpeg, .bmp, .gif, .webp)");

            //Tao Entity Song va luu vao DB truoc (de lay ID)
            var song = new Song
            {
                Title = request.Title,
                ArtistName = user.Username, //Mac dinh ten user, co the thay doi ve sau
                UploadedById = userId,
                GenreId = request.GenreId,
                AlbumId = request.AlbumId, //Co the de null neu la single
                IsExclusive = request.IsExclusive,
                Duration = 0, //Se update sau khi xu ly file
                UploadedDate = DateTime.Now
            };

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            //Xu ly luu file nhac (Goi Service convert nhac)
            double duration = await _streamingService.ProcessUploadedFileAsync(request.AudioFile, song.Id);
            if (duration <= 0)
            {
                //Neu loi xu ly file, xoa record trong DB de tranh rac
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
                return BadRequest("Lỗi khi xử lý file nhạc. Vui lòng thử lại.");
            }

            // Cap nhat thoi luong bai hat
            song.Duration = duration;
            await _context.SaveChangesAsync();

            // --- BẮT ĐẦU TÁCH BEAT (DEMUCS) ---
            string storagePath = Path.Combine(_env.WebRootPath, "audio", "lossless");
            string ext = Path.GetExtension(request.AudioFile.FileName);
            // StreamingService thường lưu file với tên là {SongId}{OriginalExtension}
            string inputFilePath = Path.Combine(storagePath, $"{song.Id}{ext}");

            // Chạy ngầm (Fire and Forget)
            _ = Task.Run(() => _audioProcessingService.SeparateAudioAsync(song.Id, inputFilePath));
            // ----------------------------------

            //Xu ly luu anh bia
            try
            {
                string coverFolder = Path.Combine(_env.WebRootPath, "images", "covers");
                Directory.CreateDirectory(coverFolder);

                string imageExt = Path.GetExtension(request.ImageFile.FileName);
                string fileName = $"{song.Id}{imageExt}";
                string imagePath = Path.Combine(coverFolder, fileName); // Tên ảnh = ID bài hát

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                // Update DB Extension
                song.CoverImageUrl = fileName;
                await _context.SaveChangesAsync();
            }
            catch
            {
                //Log loi anh (Khong quan trong nen co the bo qua hoac return warning)
            }

            return Ok(new { Message = "Upload thành công!", SongId = song.Id });
        }

        //Tao Album moi
        [HttpPost("album")]
        public async Task<IActionResult> CreateAlbum([FromBody] CreateAlbumRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // Check role Artist...
            var user = await _context.Users.FindAsync(userId);
            if (user.Role == UserRole.Listener) return StatusCode(403, "Chỉ nghệ sĩ mới được tạo Album.");

            var album = new Album
            {
                Title = request.Title,
                ArtistId = userId,
                ReleaseDate = DateTime.Now,
                CoverImageUrl = "default_album.png" //Co the them API upload anh rieng ve sau
            };

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tạo Album thành công", AlbumId = album.Id });
        }

        [HttpPut("songs/{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] SongUpdateModel request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var song = await _context.Songs.FindAsync(id);

            if (song == null) return NotFound("Bài hát không tồn tại.");
            if (song.UploadedById != userId) return StatusCode(403, "Bạn không có quyền chỉnh sửa bài hát này.");

            // Update Metadata only
            song.Title = request.Title;
            song.GenreId = request.GenreId;
            song.IsExclusive = request.IsExclusive;
            
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Cập nhật thành công!" });
        }

        [HttpDelete("songs/{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var song = await _context.Songs.FindAsync(id);

            if (song == null) return NotFound("Bài hát không tồn tại.");
            if (song.UploadedById != userId) return StatusCode(403, "Bạn không có quyền xóa bài hát này.");

            // 1. Delete files
            await _streamingService.DeleteSongFilesAsync(id);

            // 2. Delete from DB
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Xóa bài hát thành công!" });
        }
    }

    //Cac Model dung de hung du lieu upload
    public class SongUploadModel
    {
        public string Title { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public int? AlbumId { get; set; } // Nullable
        public bool IsExclusive { get; set; } = false;

        // IFormFile: Interface đặc biệt của ASP.NET Core để hứng file upload
        public IFormFile AudioFile { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class CreateAlbumRequest
    {
        public string Title { get; set; } = string.Empty;
    }

    public class SongUpdateModel
    {
        public string Title { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public bool IsExclusive { get; set; }
    }
}
