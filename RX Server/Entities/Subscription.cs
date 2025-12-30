using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RX_Server.Entities
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; //Ten goi dang ki

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } //Gia goi dang ki (VND)
        public int MaxBitrate { get; set; } //Bitrate toi da (kbps)
        public bool AllowExclusiveContent { get; set; } //Cho phep nghe noi dung doc quyen
        public string Description { get; set; } = string.Empty; //Mo ta goi dang ki
    }
}