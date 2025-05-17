using AppStore.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppStore.DAL.Context
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<App> Apps { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Download> Downloads { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Screenshot> Screenshots { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.RegisteredAt)
                      .IsRequired();
            });

            // Configure App
            modelBuilder.Entity<App>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.Description)
                      .HasMaxLength(1000);

                entity.Property(a => a.Price)
                      .HasColumnType("decimal(10,2)")
                      .IsRequired();

                entity.Property(a => a.Version)
                      .HasMaxLength(20);

                entity.HasOne(a => a.Developer)
                      .WithMany(u => u.Apps)
                      .HasForeignKey(a => a.DeveloperId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Category)
                      .WithMany(c => c.Apps)
                      .HasForeignKey(a => a.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);

                // b. Індекс за CategoryId
                entity.HasIndex(a => a.CategoryId);

                // c. Індекс за Name
                entity.HasIndex(a => a.Name);
            });

            // Configure Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(50);
            });

            // Configure Download
            modelBuilder.Entity<Download>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.HasOne(d => d.User)
                      .WithMany(u => u.Downloads)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.App)
                      .WithMany(a => a.Downloads)
                      .HasForeignKey(d => d.AppId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(d => d.DownloadedAt)
                      .IsRequired();

                // e. Комплексний індекс за UserId + AppId
                entity.HasIndex(d => new { d.UserId, d.AppId });
            });

            // Configure Review
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Rating)
                      .IsRequired();

                entity.Property(r => r.Comment)
                      .HasMaxLength(1000);

                entity.Property(r => r.CreatedAt)
                      .IsRequired();

                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.App)
                      .WithMany(a => a.Reviews)
                      .HasForeignKey(r => r.AppId)
                      .OnDelete(DeleteBehavior.Cascade);

                // d. Індекс за AppId
                entity.HasIndex(r => r.AppId);
            });

            // Configure Screenshot
            modelBuilder.Entity<Screenshot>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.ImageUrl)
                      .IsRequired()
                      .HasMaxLength(300);

                entity.HasOne(s => s.App)
                      .WithMany(a => a.Screenshots)
                      .HasForeignKey(s => s.AppId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
