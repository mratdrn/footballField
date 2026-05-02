namespace HaliSaha.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Ad { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SifreHash { get; set; } = null!;
        public string Rol { get; set; } = "Uye"; // "Uye" veya "Admin"
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        public ICollection<Rezervasyon> Rezervasyonlar { get; set; } = new List<Rezervasyon>();
    }
}