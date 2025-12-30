using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using RX_Server.Data;
using RX_Server.Services; // Add this
using RX_Server.Entities;
using Shared.DTOs.Auth;
using Shared.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

[Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthController(AppDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest1 request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("Tên tài khoản đã tồn tại.");

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email đã được sử dụng bởi tài khoản khác.");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), //Can cai goi BCrypt.net-Next
                Role = request.Role,
                SubscriptionId = 1, //Mac dinh goi Standard
                SubscriptionExpireDate = null,
                Email = request.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //Log cho nguoi dang tai (Producer/Singer), tao them profile rong
            if (request.Role != UserRole.Listener)
            {
                var artistProfile = new ArtistProfile { UserId = user.Id, IsVerified = false };
                _context.ArtistProfiles.Add(artistProfile);
                await _context.SaveChangesAsync();
            }
            return Ok("Đăng kí thành công!");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest1 request)
        {
            var user = await _context.Users
                .Include(u => u.Subscription) //Load kem goi dang ki
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Sai tài khoản hoặc mật khẩu.");

            //Kiem tra goi dang ki
            if (user.SubscriptionId > 1 && user.SubscriptionExpireDate < DateTime.Now)
            {
                user.SubscriptionId = 1; //Ha ve Standard
                user.SubscriptionExpireDate = null;
                await _context.SaveChangesAsync();
            }

            // TẠO JWT TOKEN CHUẨN
            // Token này phải khớp với Validate ở Program.cs
            // QUAN TRỌNG: Lấy key từ AppSettings để khớp với Program.cs
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                 // Fallback nếu quên cấu hình (nhưng nên có để tránh lỗi 500)
                 jwtKey = "super-secret-key-1234567890-min-length-32-chars";
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), // Token sống 7 ngày
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return Ok(new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role,
                PlanName = user.Subscription?.Name ?? "Standard",
                SubscriptionExpireDate = user.SubscriptionExpireDate,
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("Vui lòng nhập email.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return BadRequest("Email không tồn tại trong hệ thống.");

            // Tao code 6 so
            var code = new Random().Next(100000, 999999).ToString();
            user.ResetToken = code;
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(15);
            await _context.SaveChangesAsync();

            // Gui mail
            try 
            {
                await _emailService.SendEmailAsync(email, "Mã xác nhận quên mật khẩu - RX Music", 
                    $"<h1>Mã xác nhận của bạn là: <b style='color:red; font-size: 24px'>{code}</b></h1><p>Mã có hiệu lực trong 15 phút. Vui lòng không chia sẻ cho bất kì ai.</p>");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi gửi email: " + ex.Message);
            }

            return Ok("Mã xác nhận đã được gửi vào email của bạn.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) return BadRequest("Email không tồn tại.");

            if (user.ResetToken != request.Code || user.ResetTokenExpiry < DateTime.Now)
            {
                return BadRequest("Mã xác nhận không đúng hoặc đã hết hạn.");
            }

            // Doi mat khau
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _context.SaveChangesAsync();

            return Ok("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.");
        }
    }

    public class ResetPasswordRequest 
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }