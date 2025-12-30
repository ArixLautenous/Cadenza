using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RX_Server.Entities
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty; //Ten playlist
        public bool IsPublic { get; set; } = false; //Playlist cong khai hay rieng tu
        public DateTime CreatedDate { get; set; } = DateTime.Now; //Ngay tao playlist

        //Khoa ngoai den User (Nguoi tao playlist)
        public int UserId { get; set; }
        public User? User { get; set; }

        //Quan he nhieu-nhieu voi Song qua PlaylistSong
        public ICollection<PlaylistSong>? PlaylistSongs { get; set; }
    }
}