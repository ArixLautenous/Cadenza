using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RX_Server.Data;
using RX_Server.Entities;
using Shared.DTOs.Subscription;
using Shared.Enums;
using System.Security.Claims;
using System.Transactions;

namespace RX_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("upgrade")]
        public async Task<IActionResult> UpgradePlan([FromBody] UpgradeRequest1 request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.Users.FindAsync(userId);
            var plan = await _context.Subscriptions.FindAsync(request.PlanId);

            if (user == null || plan == null) return BadRequest("Người dùng hoặc gói đăng ký không hợp lệ.");

            //Gia lap thanh toan thanh cong
            //Cap nhat thong tin goi dang ki cho User
            user.SubscriptionId = plan.Id;

            //Cap nhat ngay het han dang ki
            user.SubscriptionExpireDate = DateTime.Now.AddMonths(1); //Gia su dang ki theo thang

            //Luu lich su thanh toan
            var transaction = new Transaction1
            {
                UserId = user.Id,
                PlanId = plan.Id,
                Amount = plan.Price,
                TransactionDate = DateTime.Now,
                PaymentMethod = request.PaymentMethod, //Visa, MasterCard, PayPal
                Status = TransactionStatus1.Success.ToString(),
                Description = $"Nâng cấp gói {plan.Name}"
            };

            _context.Transactions.Add(transaction);

            //Luu thay doi vao Database
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Nâng cấp lên gói {plan.Name} thành công!",
                ExpireDate = user.SubscriptionExpireDate,
                TransactionId = transaction.Id
            });
        }
    }
}