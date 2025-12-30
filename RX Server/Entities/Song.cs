using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RX_Server.Entities
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty; //Ten bai hat
        //Giu nguyen ArtistName de hien thi nhanh hon
        public string ArtistName { get; set; } = string.Empty; //Ten nghe si
        public double Duration { get; set; } //Thoi luong bai hat (giay)
        public bool IsExclusive { get; set; } = false; //Bai hat doc quyen
        public int ListenCount { get; set; } = 0; //Luot nghe
        public string? CoverImageUrl { get; set; } // Anh bia (Luu ten file: 123.png)

        // Karaoke Feature
        public string? InstrumentUrl { get; set; } // Beat
        public string? VocalUrl { get; set; } // Vocal tach biet
        public string? Lyrics { get; set; } // Loi bai hat


        //Chuyen tu string sang khoa ngoai den Genre
        public int GenreId { get; set; } //Khoa ngoai den The loai
        public Genre? Genre { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.Now; //Ngay tai len

        //Nguoi tai len (Producer/Singer)
        public int UploadedById { get; set; }
        public User? Uploader { get; set; }

        //Lien ket den Album (Neu co)
        public int? AlbumId { get; set; } //Cho phep null neu khong thuoc album nao
        public Album? Album { get; set; }

        //Lien ket nguoc voi PlaylistSong
        public ICollection<PlaylistSong>? InPlaylists { get; set; }
    }
}