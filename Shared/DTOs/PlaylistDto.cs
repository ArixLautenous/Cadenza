namespace Shared.DTOs
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SongCount { get; set; }
    }
}