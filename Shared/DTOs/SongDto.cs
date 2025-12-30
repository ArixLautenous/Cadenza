using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class SongDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string AlbumName { get; set; } = string.Empty;
        public string GenreName { get; set; } = string.Empty;

        public double Duration { get; set; } // Duration tinh theo giay
        public string ThumbnailUrl { get; set; } = string.Empty; // URL hinh anh thumbnail

        public bool IsExclusive { get; set; } // Cho biet bai hat co phai la bai doc quyen khong

        //Logic giup Client nhan biet bai hat co duoc nghe khong
        public bool IsLocked { get; set; } = false; // Neu true thi bai hat bi khoa (Khong duoc phep nghe)

        // Karaoke Feature
        public string? InstrumentalUrl { get; set; }
        public string? VocalUrl { get; set; }
        public string? Lyrics { get; set; }
    }
}