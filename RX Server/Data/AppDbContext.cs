using Microsoft.EntityFrameworkCore;
using RX_Server.Entities;

namespace RX_Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ArtistProfile> ArtistProfiles { get; set; }
        public DbSet<Transaction1> Transactions { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //User - ArtistProfile: 1-1
            modelBuilder.Entity<ArtistProfile>()
                .HasOne(ap => ap.User)
                .WithOne(u => u.ArtistProfile)
                .HasForeignKey<ArtistProfile>(ap => ap.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            //Set default subscription
            modelBuilder.Entity<User>()
                .Property(u => u.SubscriptionId)
                .HasDefaultValue(1);
            //Album - Artist: 1-Nhieu
            modelBuilder.Entity<Album>()
                .HasOne(a => a.Artist)
                .WithMany() //1 Artist co the co nhieu Album
                .HasForeignKey(a => a.ArtistId)
                .OnDelete(DeleteBehavior.Restrict); //Xoa user nhung khong xoa album ngay de bao toan data
            //Playlist - Song: Nhieu-Nhieu: 1 bai hat co the o trong nhieu playlist va 1 playlist co the co nhieu bai hat
            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId);
            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany(s => s.InPlaylists)
                .HasForeignKey(ps => ps.SongId);
            //The loai - Song: 1-Nhieu
            modelBuilder.Entity<Song>()
                .HasOne(s => s.Genre)
                .WithMany(g => g.Songs)
                .HasForeignKey(s => s.GenreId)
                .OnDelete(DeleteBehavior.Restrict); //Xoa the loai nhung khong xoa bai hat
            //Cấu hình mối quan hệ User - Subscription
            modelBuilder.Entity<User>()
                .HasOne(u => u.Subscription)
                .WithMany()
                .HasForeignKey(u => u.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}