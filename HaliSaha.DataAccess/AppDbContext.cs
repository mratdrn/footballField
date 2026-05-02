using HaliSaha.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaliSaha.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Saha> Sahalar => Set<Saha>();
        public DbSet<Rezervasyon> Rezervasyonlar => Set<Rezervasyon>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Saha -> Rezervasyon ilişkisi
            modelBuilder.Entity<Rezervasyon>()
                .HasOne(r => r.Saha)
                .WithMany(s => s.Rezervasyonlar)
                .HasForeignKey(r => r.SahaId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Rezervasyon ilişkisi
            modelBuilder.Entity<Rezervasyon>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rezervasyonlar)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Email unique olmalı
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Para tipi hassasiyeti
            modelBuilder.Entity<Saha>()
                .Property(s => s.SaatlikUcret)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Rezervasyon>()
                .Property(r => r.ToplamUcret)
                .HasPrecision(10, 2);
        }
    }
}