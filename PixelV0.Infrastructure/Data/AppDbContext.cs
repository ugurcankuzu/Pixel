
using Microsoft.EntityFrameworkCore;
using PixelV0.Modules.Catalog.Entity;
using PixelV0.Modules.Identity.Domain.Entity;
using PixelV0.Modules.Social.Entity;
using System.Text.Json;
namespace PixelV0.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        //Identity Domain
        public DbSet<User> Users { get; set; }
        //Catalog Domain
        public DbSet<Catalog> Catalog { get; set; }
        //Social Domain
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Preset> Presets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Identity Domain
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "identity");

                entity.HasKey(e => e.Id);

                entity.HasIndex(entity => entity.Username).IsUnique();
                entity.HasIndex(entity => entity.Email).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
                entity.Property(e => e.AvatarUrl).IsRequired(false);
            });

            // Catalog Domain
            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.ToTable("Catalog", "catalog");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Game_Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Publisher).IsRequired(true).HasMaxLength(255);
            });

            // Social Domain

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Posts", "social");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Image_URL).IsRequired();
                entity.Property(e => e.Caption).IsRequired().HasMaxLength(512);

                entity.HasIndex(e => e.CreatedAt);

                // Post Indexes: A post can have only one User, but a User can have many Posts. If a User is deleted, all their Posts should be deleted as well.

                entity.HasOne<User>().WithMany()
                    .HasForeignKey(e => e.User_ID)
                    .OnDelete(DeleteBehavior.Cascade);

                // A post must be associated with a game in the catalog, but a game can have many posts. If a post is deleted, the associated game should not be deleted.
                entity.HasOne<Catalog>().WithMany().HasForeignKey(e => e.Game_ID)
                    .OnDelete(DeleteBehavior.Restrict);


                // A post be associated with a preset, but a preset can have many posts. If a post is deleted, the associated preset should not be deleted.
                entity.HasOne<Preset>().WithMany().HasForeignKey(e => e.Preset_ID).IsRequired(true)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Like>(entity =>
            {

                entity.ToTable("Likes", "social");
                entity.HasKey(e => new { e.User_ID, e.Post_ID });
                // A like must be associated with a user, but a user can have many likes. If a user is deleted, all their likes should be deleted as well.
                entity.HasOne<User>().WithMany()
                    .HasForeignKey(e => e.User_ID)
                    .OnDelete(DeleteBehavior.Cascade);
                // A like must be associated with a post, but a post can have many likes. If a post is deleted, all its likes should be deleted as well.
                entity.HasOne<Post>().WithMany()
                    .HasForeignKey(e => e.Post_ID)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Preset>(entity =>
            {
                entity.ToTable("Presets", "social");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Version).IsRequired();

                entity.Property(e => e.Values).HasColumnType("jsonb").HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<PresetValues>(v, (JsonSerializerOptions)null) ?? new PresetValues());

                entity.HasOne<User>().WithMany().HasForeignKey(e => e.User_ID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne<Catalog>().WithMany().HasForeignKey(e => e.Game_ID).OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
