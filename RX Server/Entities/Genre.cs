using System.ComponentModel.DataAnnotations;

namespace RX_Server.Entities
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; //Ten the loai
        public string Description { get; set; } = string.Empty; //Mo ta the loai

        //Quan he 1-nhieu: 1 The loai co nhieu Bai hat
        public ICollection<Song>? Songs { get; set; }
    }
}