using HaliSaha.DataAccess;
using HaliSaha.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaliSaha.Business
{
    public class SahaService
    {
        private readonly AppDbContext _context;

        public SahaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Saha>> TumSahalariGetirAsync()
        {
            return await _context.Sahalar
                .Where(s => s.Aktif)
                .ToListAsync();
        }

        public async Task<Saha?> SahaGetirAsync(int id)
        {
            return await _context.Sahalar.FindAsync(id);
        }

        public async Task<Saha> SahaEkleAsync(Saha saha)
        {
            _context.Sahalar.Add(saha);
            await _context.SaveChangesAsync();
            return saha;
        }

        public async Task<(bool Basarili, string Mesaj)> SahaGuncelleAsync(int id, Saha guncellenmis)
        {
            var saha = await _context.Sahalar.FindAsync(id);
            if (saha is null)
                return (false, "Saha bulunamadı.");

            saha.Ad = guncellenmis.Ad;
            saha.Adres = guncellenmis.Adres;
            saha.Tip = guncellenmis.Tip;
            saha.SaatlikUcret = guncellenmis.SaatlikUcret;
            saha.Aktif = guncellenmis.Aktif;

            await _context.SaveChangesAsync();
            return (true, "Saha güncellendi.");
        }

        public async Task<(bool Basarili, string Mesaj)> SahaSilAsync(int id)
        {
            var saha = await _context.Sahalar.FindAsync(id);
            if (saha is null)
                return (false, "Saha bulunamadı.");

            // Fiziksel silmek yerine pasife al (soft delete)
            saha.Aktif = false;
            await _context.SaveChangesAsync();
            return (true, "Saha pasife alındı.");
        }
    }
}