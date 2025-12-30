using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RX_Server.Entities
{
    public class Album
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty; //Ten album
        public DateTime ReleaseDate { get; set; } = DateTime.Now; //Ngay phat hanh
        public string CoverImageUrl { get; set; } = string.Empty; //URL anh bia album

        //Khoa ngoai den User (Producer/Singer)
        public int ArtistId { get; set; }
        [ForeignKey("ArtistId")]
        public User? Artist { get; set; }

        //Quan he 1-nhieu: 1 Album co nhieu Bai hat
        public ICollection<Song>? Songs { get; set; }
    }
}