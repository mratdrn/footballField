using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HaliSaha.DataAccess
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(
            "Host=dpg-d7r2vesm0tmc7382frqg-a.frankfurt-postgres.render.com;Database=footballfield_db;Username=footballfield_db_user;Password=2f9Prgzn396dMELkoOIGt67pnPk5gjTr;SSL Mode=Require;Trust Server Certificate=true");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}