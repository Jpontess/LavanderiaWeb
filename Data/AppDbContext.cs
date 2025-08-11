using Microsoft.EntityFrameworkCore;
using WebLavApp.Models;

namespace WebLavApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ServicoInterno> ServicoInternos { get; set; }
        public DbSet<Modalidades> Modalidades { get; set; }
        public DbSet<Secretaria> Secretarias { get; set; }
        public DbSet<Fechamento> Fechamentos { get; set; }
    }
}