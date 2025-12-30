using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RX_Server.Entities
{
    public class ArtistProfile
    {
        [Key]
        public int Id { get; set; }
        public string Bio { get; set; } = string.Empty; //Thong tin tieu su nghe si
        public bool IsVerified { get; set; } = false; //Xac thuc nghe si
        public int FollowersCount { get; set; } = 0; //So luong nguoi theo doi
        //Khoa ngoai den User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}