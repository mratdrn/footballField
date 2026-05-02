namespace HaliSaha.Entities
{
    public class Rezervasyon
    {
        public int Id { get; set; }
        public int SahaId { get; set; }
        public int UserId { get; set; }
        public DateTime BaslangicZamani { get; set; }
        public DateTime BitisZamani { get; set; }
        public decimal ToplamUcret { get; set; }
        public string Durum { get; set; } = "Aktif"; // "Aktif", "İptal"

        // Navigation Properties
        public Saha Saha { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}