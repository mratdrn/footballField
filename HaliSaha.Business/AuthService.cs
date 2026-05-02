using HaliSaha.DataAccess;
using HaliSaha.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HaliSaha.Business
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<(bool Basarili, string Mesaj)> KayitOlAsync(string ad, string email, string sifre)
        {
            // Email daha önce alınmış mı?
            var mevcutKullanici = await _context.Users
                .AnyAsync(u => u.Email == email);

            if (mevcutKullanici)
                return (false, "Bu email zaten kayıtlı.");

            var user = new User
            {
                Ad = ad,
                Email = email,
                SifreHash = BCrypt.Net.BCrypt.HashPassword(sifre)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, "Kayıt başarılı.");
        }

        public async Task<(bool Basarili, string Mesaj, string? Token)> GirisYapAsync(string email, string sifre)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                return (false, "Email veya şifre hatalı.", null);

            var sifreGecerli = BCrypt.Net.BCrypt.Verify(sifre, user.SifreHash);
            if (!sifreGecerli)
                return (false, "Email veya şifre hatalı.", null);

            var token = TokenUret(user);
            return (true, "Giriş başarılı.", token);
        }

        private string TokenUret(User user)
        {
            // Token içine yazılacak bilgiler (Claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}