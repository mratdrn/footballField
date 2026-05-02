using HaliSaha.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaliSaha.DataAccess
{
    public static class SeedData
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Admin zaten varsa tekrar ekleme
            if (await context.Users.AnyAsync(u => u.Rol == "Admin"))
                return;

            var admin = new User
            {
                Ad = "Admin",
                Email = "admin@halisaha.com",
                SifreHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Rol = "Admin"
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}