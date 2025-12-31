using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RX_Server.Data;
using RX_Server.Services; // Cần dùng IStreamingService
using RX_Server.Data;
using RX_Server.Services;
using System.Security.Claims;

namespace RX_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStreamingService _streamingService;

        // Inject IStreamingService thay vì IWebHostEnvironment
        // Vì logic tìm đường dẫn file đã được chuyển vào Service rồi
        public SongsController(AppDbContext context, IStreamingService streamingService)
        {
            _context = context;
            _streamingService = streamingService;
        }

        // GET: api/songs
        // Lấy danh sách tất cả bài hát (Public)
        // GET: api/songs
        // Lấy danh sách tất cả bài hát (Public)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dbSongs = await _context.Songs
                .Include(s => s.Album)
                .ToListAsync();

            var songs = dbSongs.Select(s => new
            {
                s.Id,
                s.Title,
                s.ArtistName,
                s.Duration,
                s.IsExclusive,
                AlbumName = s.Album != null ? s.Album.Title : "Unknown Album",
                s.GenreId,
                ThumbnailUrl = !string.IsNullOrEmpty(s.CoverImageUrl) 
                    ? $"{Request.Scheme}://{Request.Host}/images/covers/{s.CoverImageUrl}"
                    : $"{Request.Scheme}://{Request.Host}/images/covers/{s.Id}.jpg",
                // Karaoke
                s.Lyrics,
                InstrumentalUrl = s.InstrumentUrl,
                s.VocalUrl
            });

            return Ok(songs);
        }

        // GET: api/songs/5
        // Lấy chi tiết bài hát
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var s = await _context.Songs
                .Include(x => x.Album)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (s == null) return NotFound();

            var songDto = new
            {
                s.Id,
                s.Title,
                s.ArtistName,
                s.Duration,
                s.IsExclusive,
                AlbumName = s.Album != null ? s.Album.Title : "Unknown Album",
                s.GenreId,
                ThumbnailUrl = !string.IsNullOrEmpty(s.CoverImageUrl) 
                    ? $"{Request.Scheme}://{Request.Host}/images/covers/{s.CoverImageUrl}"
                    : $"{Request.Scheme}://{Request.Host}/images/covers/{s.Id}.jpg",
                // Karaoke
                s.Lyrics,
                InstrumentalUrl = s.InstrumentUrl,
                s.VocalUrl
            };
            return Ok(songDto);
        }

        // GET: api/songs/search?query=...
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if(string.IsNullOrWhiteSpace(query)) return Ok(new List<object>());

            var dbSongs = await _context.Songs
                .Include(s => s.Album)
                .Where(s => s.Title.Contains(query) || s.ArtistName.Contains(query))
                .ToListAsync();

             var songs = dbSongs.Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.ArtistName,
                    s.Duration,
                    s.IsExclusive,
                    AlbumName = s.Album != null ? s.Album.Title : "Unknown Album",
                    s.GenreId,
                    ThumbnailUrl = !string.IsNullOrEmpty(s.CoverImageUrl) 
                        ? $"{Request.Scheme}://{Request.Host}/images/covers/{s.CoverImageUrl}"
                        : $"{Request.Scheme}://{Request.Host}/images/covers/{s.Id}.jpg",
                    // Karaoke
                    s.Lyrics,
                    InstrumentalUrl = s.InstrumentUrl,
                    s.VocalUrl
                });

            return Ok(songs);
        }

        // GET: api/songs/5/stream
        // Stream nhạc (Yêu cầu đăng nhập)
        [HttpGet("{id}/stream")]
        [Authorize] // Bắt buộc phải có Token
        public async Task<IActionResult> StreamSong(int id)
        {
            // 1. Lấy User ID từ Token hiện tại
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            var userId = int.Parse(userIdStr);

            // 2. Lấy thông tin User và Gói cước
            var user = await _context.Users
                .Include(u => u.Subscription)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var song = await _context.Songs.FindAsync(id);

            if (song == null) return NotFound("Bài hát không tồn tại.");
            if (user == null) return Unauthorized("Không tìm thấy thông tin người dùng.");

            // 3. LOGIC KIỂM TRA QUYỀN (EXCLUSIVE)
            // Nếu bài hát là Độc quyền (IsExclusive) mà User không phải gói Audiophile (Id = 3)
            // Lưu ý: user.SubscriptionId là int, 3 là gói Audiophile
            if (song.IsExclusive && user.SubscriptionId < 3)
            {
                return StatusCode(403, "Bạn cần nâng cấp lên gói Audiophile để nghe bài Demo độc quyền này.");
            }

            // 4. Lấy đường dẫn file nhạc phù hợp từ Service
            // Service sẽ tự quyết định trả về file FLAC, AAC hay MP3 dựa trên gói cước
            string filePath = await _streamingService.GetAudioFilePathAsync(id, user.SubscriptionId);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File nhạc chưa sẵn sàng trên hệ thống.");
            }

            // 5. Xác định ContentType (MIME type)
            string contentType = "audio/mpeg"; // Mặc định mp3
            if (filePath.EndsWith(".flac")) contentType = "audio/flac";
            else if (filePath.EndsWith(".m4a")) contentType = "audio/mp4";
            else if (filePath.EndsWith(".wav")) contentType = "audio/wav";

            // 6. Mở Stream và trả về
            // FileShare.Read: Cho phép nhiều người cùng đọc file này 1 lúc
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            // enableRangeProcessing: true -> QUAN TRỌNG: Cho phép tua nhạc
            return File(fileStream, contentType, enableRangeProcessing: true);
        }

        // Helper: Tạo URL ảnh bìa
        private static string ConfigUrl(HttpRequest request, int songId)
        {
            // Trả về: https://localhost:5000/images/covers/10.jpg
            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/images/covers/{songId}.jpg"; // Giả định ảnh luôn là jpg, bạn có thể check file thật nếu muốn kỹ hơn
        }
    }
}