using Blog.Domain.Entities;
using Blog.Domain.MaterializedView;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure
{
    public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
    {
        public DbSet<Post> Post { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<MvTagsStatistics> MvTagsStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>(entity =>
            {
                entity.ToTable("t_pictures", "public");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.PostId).HasColumnName("post_id").IsRequired();
                entity.Property(p => p.ImageUrl).HasColumnName("image_url").IsRequired();
                entity.Property(p => p.ThumbnailUrl).HasColumnName("thumbnail_url").IsRequired();
                entity.Property(p => p.ImageType).HasColumnName("image_type").IsRequired();
                entity.Property(p => p.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(p => p.UpdatedAt).HasColumnName("updated_at");
                entity.HasOne(p => p.Post)
                    .WithMany()
                    .HasForeignKey(p => p.PostId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("t_posts", "public");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.AuthorId).HasColumnName("author_id");
                entity.Property(p => p.Status).HasColumnName("status").IsRequired();
                entity.Property(p => p.Title).HasColumnName("title").IsRequired();
                entity.Property(p => p.Content).HasColumnName("content").IsRequired(false);
                entity.Property(p => p.Tags).HasColumnName("tags").HasColumnType("jsonb").IsRequired();
                entity.Property(p => p.CreatedAt).HasColumnName("created_at")
                    .HasDefaultValueSql("EXTRACT(EPOCH FROM CURRENT_TIMESTAMP) * 1000");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("t_tags", "public");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.Title).HasColumnName("title");
                entity.Property(p => p.CreatedAt).HasColumnName("created_at")
                    .HasDefaultValueSql("EXTRACT(EPOCH FROM CURRENT_TIMESTAMP) * 1000");
                ;
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("t_users", "auth");
                entity.HasKey(u => u.Guid);
                entity.Property(u => u.Guid).HasColumnName("guid");
                entity.Property(u => u.UserName).HasColumnName("name").HasMaxLength(256).IsRequired();
                entity.Property(u => u.NormalizedUserName)
                    .HasColumnName("normalized_user_name")
                    .HasMaxLength(256)
                    .IsRequired(false);
                entity.Property(u => u.Email)
                    .HasColumnName("email")
                    .HasMaxLength(256)
                    .IsRequired();
                entity.Property(u => u.NormalizedEmail)
                    .HasColumnName("normalized_email")
                    .HasMaxLength(256)
                    .IsRequired(false);
                entity.Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
                entity.Property(u => u.PasswordHash).HasColumnName("password_hash").IsRequired(false);
                entity.Property(u => u.SecurityStamp).HasColumnName("security_stamp").IsRequired(false);
                entity.Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp").IsRequired(false);
                entity.Property(u => u.PhoneNumber).HasColumnName("phone_number").IsRequired(false);
                entity.Property(u => u.PhoneNumberConfirmed)
                    .HasColumnName("phone_number_confirmed")
                    .HasDefaultValue(false);
                entity.Property(u => u.TwoFactorEnabled)
                    .HasColumnName("two_factor_enabled")
                    .HasDefaultValue(false);
                entity.Property(u => u.LockoutEnd).HasColumnName("lockout_end").IsRequired(false);
                entity.Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled").HasDefaultValue(false);
                entity.Property(u => u.AccessFailedCount).HasColumnName("access_failed_count").HasDefaultValue(false);
            });
        }
    }
}