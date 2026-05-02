using HaliSaha.DataAccess;
using HaliSaha.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaliSaha.Business
{
    public class RezervasyonService
    {
        private readonly AppDbContext _context;

        public RezervasyonService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Rezervasyon>> KullanicininRezervasyonlari(int userId)
        {
            return await _context.Rezervasyonlar
                .Include(r => r.Saha)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.BaslangicZamani)
                .ToListAsync();
        }

        public async Task<List<Rezervasyon>> TumRezervasyonlar()
        {
            return await _context.Rezervasyonlar
                .Include(r => r.Saha)
                .Include(r => r.User)
                .OrderByDescending(r => r.BaslangicZamani)
                .ToListAsync();
        }

        public async Task<(bool Basarili, string Mesaj)> RezervasyonIptalEt(int rezervasyonId, int userId, string rol)
        {
            var rezervasyon = await _context.Rezervasyonlar.FindAsync(rezervasyonId);

            if (rezervasyon is null)
                return (false, "Rezervasyon bulunamadı.");

            // Sadece kendi rezervasyonunu iptal edebilir, Admin hepsini iptal edebilir
            if (rol != "Admin" && rezervasyon.UserId != userId)
                return (false, "Bu rezervasyonu iptal etme yetkiniz yok.");

            rezervasyon.Durum = "İptal";
            await _context.SaveChangesAsync();
            return (true, "Rezervasyon iptal edildi.");
        }



        public async Task<(bool Basarili, string Mesaj, Rezervasyon? Rezervasyon)>
            RezervasyonYapAsync(int sahaId, int userId, DateTime baslangic, DateTime bitis)
        {
            // 1. Saha var mı ve aktif mi?
            var saha = await _context.Sahalar
                .FirstOrDefaultAsync(s => s.Id == sahaId && s.Aktif);

            if (saha is null)
                return (false, "Saha bulunamadı veya aktif değil.", null);

            // 2. Zaman geçerli mi?
            if (baslangic >= bitis)
                return (false, "Başlangıç zamanı bitiş zamanından önce olmalı.", null);

            if (baslangic < DateTime.UtcNow)
                return (false, "Geçmiş bir tarihe rezervasyon yapılamaz.", null);

            // ★ 3. ÇAKIŞMA KONTROLÜ — Projenin kalbi bu satırlar
            var cakismaVar = await _context.Rezervasyonlar
                .AnyAsync(r =>
                    r.SahaId == sahaId &&
                    r.Durum == "Aktif" &&
                    r.BaslangicZamani < bitis &&      // Mevcut rezervasyon, yeni bitiş'ten önce başlıyor
                    r.BitisZamani > baslangic);       // Mevcut rezervasyon, yeni başlangıç'tan sonra bitiyor

            if (cakismaVar)
                return (false, "Seçtiğiniz saat dilimi dolu.", null);

            // 4. Toplam ücret hesapla
            var sure = (decimal)(bitis - baslangic).TotalHours;
            var toplamUcret = sure * saha.SaatlikUcret;

            // 5. Kaydet
            var rezervasyon = new Rezervasyon
            {
                SahaId = sahaId,
                UserId = userId,
                BaslangicZamani = baslangic,
                BitisZamani = bitis,
                ToplamUcret = toplamUcret
            };

            _context.Rezervasyonlar.Add(rezervasyon);
            await _context.SaveChangesAsync();

            return (true, "Rezervasyon başarıyla oluşturuldu.", rezervasyon);
        }
    }
}