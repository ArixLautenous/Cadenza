using Azure.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RX_Server.Data;
using RX_Server.Entities;
using Shared.DTOs;
using System.Security.Claims;

namespace RX_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //Bat buoc dang nhap
    [Authorize] //Bat buoc dang nhap
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UsersController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        //Lay thong tin profile cua nguoi dung dang dang nhap
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var user = await _context.Users
                .Include(u => u.Subscription)
                .Include(u => u.ArtistProfile) //Load them thong tin ArtistProfile
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();

            var dto = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                ProfileImageUrl = !string.IsNullOrEmpty(user.ProfileImageUrl)
                    ? $"{Request.Scheme}://{Request.Host}/images/avatars/{user.ProfileImageUrl}"
                    : null,
                Role = user.Role,
                PlanName = user.Subscription?.Name ?? "Standard",
                SubscriptionExpireDate = user.SubscriptionExpireDate,

                //Nap thong tin ArtistProfile neu co
                IsArtist = user.ArtistProfile != null,
                Bio = user.ArtistProfile?.Bio ?? "",
                FollowersCount = user.ArtistProfile?.FollowersCount ?? 0,
                IsVerified = user.ArtistProfile?.IsVerified ?? false
            };

            //lay danh sach bai hat upload
            //lay danh sach bai hat upload
            var dbSongs = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .Where(s => s.UploadedById == userId)
                .ToListAsync();

            var songs = dbSongs.Select(s => new SongDto
            {
                Id = s.Id,
                Title = s.Title,
                ArtistName = s.ArtistName,
                AlbumName = s.Album?.Title ?? "",
                GenreName = s.Genre?.Name ?? "",
                Duration = s.Duration,
                IsExclusive = s.IsExclusive,
                ThumbnailUrl = !string.IsNullOrEmpty(s.CoverImageUrl)
                        ? $"{Request.Scheme}://{Request.Host}/images/covers/{s.CoverImageUrl}"
                        : $"{Request.Scheme}://{Request.Host}/images/covers/{s.Id}.jpg"
            })
                .ToList();

            dto.UploadedSongs = songs;

            return Ok(dto);
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile image)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            if (image == null || image.Length == 0) return BadRequest("Vui lòng chọn ảnh.");

            // Tao folder neu chua co
            string folder = Path.Combine(_env.WebRootPath, "images", "avatars");
            Directory.CreateDirectory(folder);

            string ext = Path.GetExtension(image.FileName);
            string fileName = $"{userId}_{DateTime.Now.Ticks}{ext}"; // TimeStamp de tranh cache
            string fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Cap nhat DB
            user.ProfileImageUrl = fileName;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cập nhật avatar thành công!", Url = $"{Request.Scheme}://{Request.Host}/images/avatars/{fileName}" });
        }

        //Lay danh sach Playlist cua nguoi dung
        [HttpGet("playlists")]
        public async Task<IActionResult> GetMyPlaylists()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var playlists = await _context.Playlists
                .Where(p => p.UserId == userId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.IsPublic,
                    SongCount = p.PlaylistSongs.Count
                })
                .ToListAsync();
            return Ok(playlists);
        }

        //Tao Playlist
        [HttpPost("playlists")]
        public async Task<IActionResult> CreatePlaylist([FromBody] string playlistName)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var playlist = new Playlist
            {
                Name = playlistName,
                UserId = userId,
                IsPublic = true,
                CreatedDate = DateTime.Now
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tạo playlist thành công", Id = playlist.Id });
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // Verify old password
            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
            {
                return BadRequest("Mật khẩu cũ không chính xác.");
            }

            // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đổi mật khẩu thành công!" });
        }

        [HttpPut("change-username")]
        public async Task<IActionResult> ChangeUsername([FromBody] string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername)) return BadRequest("Tên đăng nhập không được để trống.");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // Check duplicate
            if (await _context.Users.AnyAsync(u => u.Username == newUsername && u.Id != userId))
            {
                return BadRequest("Tên đăng nhập đã tồn tại.");
            }

            user.Username = newUsername;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đổi tên đăng nhập thành công!" });
        }

        [HttpPut("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Email không được để trống.");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // Check duplicate
            if (await _context.Users.AnyAsync(u => u.Email == email && u.Id != userId))
            {
                return BadRequest("Email đã được sử dụng bởi tài khoản khác.");
            }

            user.Email = email;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cập nhật Email thành công!" });
        }

        public class ChangePasswordRequest
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
    }
}