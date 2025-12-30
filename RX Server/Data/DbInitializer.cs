using Microsoft.EntityFrameworkCore;
using RX_Server.Entities;
using System.Diagnostics.Metrics;

namespace RX_Server.Data
{
    public class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            //Tao DB neu chua co
            context.Database.EnsureCreated();

            // Patch Update DB (Antigravity): Them cot ProfileImageUrl neu DB cu chua co
            context.Database.ExecuteSqlRaw(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Users]') AND name = 'ProfileImageUrl')
                BEGIN
                    ALTER TABLE Users ADD ProfileImageUrl NVARCHAR(MAX) NULL
                END");

            // Patch Update DB (Antigravity): Them cot Email, ResetToken
            context.Database.ExecuteSqlRaw(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Users]') AND name = 'Email')
                BEGIN
                    ALTER TABLE Users ADD Email NVARCHAR(200) NULL
                END");

            context.Database.ExecuteSqlRaw(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Users]') AND name = 'ResetToken')
                BEGIN
                    ALTER TABLE Users ADD ResetToken NVARCHAR(MAX) NULL
                END");

            context.Database.ExecuteSqlRaw(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Users]') AND name = 'ResetTokenExpiry')
                BEGIN
                    ALTER TABLE Users ADD ResetTokenExpiry DATETIME2 NULL
                END");

            // Patch Update DB (Antigravity): Them cot CoverImageUrl cho Songs
            context.Database.ExecuteSqlRaw(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Songs]') AND name = 'CoverImageUrl')
                BEGIN
                    ALTER TABLE Songs ADD CoverImageUrl NVARCHAR(MAX) NULL
                END");

            //1. Seed Subscriptions
            if (!context.Subscriptions.Any())
            {
                var subscriptions = new Subscription[]
                {
                    new Subscription
                    {
                        Name = "Standard",
                        Price = 0,
                        MaxBitrate = 128,
                        AllowExclusiveContent = false,
                        Description = "Nghe nhạc miễn phí ở chất lương tiêu chuẩn."
                    },
                    new Subscription
                    {
                        Name = "Like a Pro",
                        Price = 29000,
                        MaxBitrate = 256,
                        AllowExclusiveContent = false,
                        Description = "Nghe nghe nhạc ở chất lượng cao và không bị gián đoạn."
                    },
                    new Subscription
                    {
                        Name = "Audiophile",
                        Price = 59000,
                        MaxBitrate = 1411,
                        AllowExclusiveContent = true,
                        Description = "Nghe nghe nhạc với chất lượng Lossless và được nhận Demo độc quyền từ nghệ sĩ."
                    }
                };
                context.Subscriptions.AddRange(subscriptions);
                context.SaveChanges();
            }

            //2. Seed Genres
            if (!context.Genres.Any())
            {
                var genres = new Genre[]
                {
                    new Genre { Name = "Pop", Description = "Nhạc Pop phổ biến" },
                    new Genre { Name = "Rock", Description = "Nhạc Rock mạnh mẽ" },
                    new Genre { Name = "Ballad", Description = "Nhạc nhẹ trữ tình" },
                    new Genre { Name = "Indie", Description = "Nhạc Indie độc lập" },
                    new Genre { Name = "Rap", Description = "Nhạc Rap/Hip-hop" }
                };
                context.Genres.AddRange(genres);
                // Reset identity is sometimes needed but EF Core usually handles 1-based index on fresh insert
                context.SaveChanges();
            }


        }
    }
}