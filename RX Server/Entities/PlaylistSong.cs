using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RX_Server.Entities
{
    //Bang lien ket nhieu-nhieu giua Playlist va Song
    //Luu them thong tin ve thu tu bai hat trong playlist hoac thoi diem them bai hat
    public class PlaylistSong
    {
        [Key]
        public int Id { get; set; }

        public int PlaylistId { get; set; } //Khoa ngoai den Playlist
        public Playlist? Playlist { get; set; }

        public int SongId { get; set; } //Khoa ngoai den Song
        public Song? Song { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.Now; //Ngay them bai hat vao playlist
        public int OrderIndex { get; set; } //Thu tu bai hat trong playlist
    }
}