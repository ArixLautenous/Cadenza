using Microsoft.AspNetCore.SignalR;
using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace RX_Server.Entities
{
    public class Transaction1
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; } //Khoa ngoai den User
        public User? User { get; set; }

        public int PlanId { get; set; } //Khoa ngoai den Subscription
        public Subscription? Plan { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } //So tien giao dich (VND)
        public DateTime TransactionDate { get; set; } = DateTime.Now; //Ngay giao dich
        public string PaymentMethod { get; set; } = Shared.Enums.PaymentMethod.Unknown.ToString(); //Phuong thuc thanh toan
        public string Status { get; set; } = TransactionStatus1.Pending.ToString(); //Trang thai giao dich (Success, Failed)
        public string Description { get; set; } = string.Empty; //Mo ta giao dich
    }
}
