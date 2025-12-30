using System;
using System.Collections.Generic;
using System.Text;
using Shared.Enums;

namespace Shared.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? ProfileImageUrl { get; set; }
        public UserRole Role { get; set; }
        //Thong tin goi cuoc
        public string PlanName { get; set; }
        public DateTime? SubscriptionExpireDate { get; set; }
        //Thong tin cho Artist (neu co)
        public bool IsArtist { get; set; } = false;
        public string Bio { get; set; } = string.Empty;
        public int FollowersCount { get; set; } = 0;
        public bool IsVerified { get; set; } = false;

        //Danh sach bai hat da upload
        public List<SongDto> UploadedSongs { get; set; } = new List<SongDto>();
    }
}
