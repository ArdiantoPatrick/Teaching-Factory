using Microsoft.EntityFrameworkCore;
using ProjekTeFa.Models;

namespace ProjekTeFa.Data
{
    public class ApplicationDbContext : DbContext
    { 
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }
        public ApplicationDbContext() { }
        public DbSet<TempatKerja> tbl_TempatKerja { get; set; }
        public DbSet<Mekanik> tbl_Mekanik { get; set; }
        public DbSet<Customer> tbl_Customer { get; set; }
        public DbSet<Prodi> tbl_Prodi { get; set; }
        public DbSet<Booking> tbl_Booking { get; set; }
        public DbSet<Grup> tbl_Grup { get; set; }
        public DbSet<RoundChecking> tbl_RoundChecking { get; set;}
        public DbSet<PKB> tbl_PKB { get; set; }
    }
}
