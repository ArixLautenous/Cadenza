using System.ComponentModel.DataAnnotations;
using Shared.Enums; // Can reference Shared Project de dung Enum

namespace RX_Server.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; } // URL anh dai dien

        [EmailAddress]
        public string? Email { get; set; } // Email de khoi phuc mat khau
        public string? ResetToken { get; set; } // Token khoi phuc
        public DateTime? ResetTokenExpiry { get; set; } // Han cua Token

        [Required]
        public string PasswordHash { get; set; } = string.Empty; //Mat khau da ma hoa
        public UserRole Role { get; set; } = UserRole.Listener; //Vai tro nguoi dung

        //Khoa lien ket den Subscription
        public int SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
        public DateTime? SubscriptionExpireDate { get; set; } //Ngay het han goi dang ki

        //Quan he voi ArtistProfile (Neu la Producer/Singer)
        public ArtistProfile? ArtistProfile { get; set; }
        public ICollection<Album>? Albums { get; set; }
    }
}