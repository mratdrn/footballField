namespace HaliSaha.Entities
{
    public class Saha
    {
        public int Id { get; set; }
        public string Ad { get; set; } = null!;
        public string Adres { get; set; } = null!;
        public string Tip { get; set; } = null!;       // "5v5", "7v7", "11v11"
        public decimal SaatlikUcret { get; set; }
        public bool Aktif { get; set; } = true;

        public ICollection<Rezervasyon> Rezervasyonlar { get; set; } = new List<Rezervasyon>();
    }
}