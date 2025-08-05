using Microsoft.EntityFrameworkCore;
using StokTakip.Data.Concrete.EFcore.Mapping;
using StokTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Data.Concrete.EFcore.Contexts
{
    public class StokContext : DbContext
    {
        public DbSet<Malzeme> malzemeler { get; set; }
        public DbSet<Depo> depolar { get; set; }
        public DbSet<Stok> stoklar { get; set; }
        public DbSet<Cari> cariler { get; set; }
        public DbSet<LogTakip> logTakipler { get; set; }
        //public DbSet<kullanici> kullanicilar { get; set; }
        public DbSet<DepoTransfer> depoTransferleri { get; set; }
        public DbSet<DepoTransferDetay> depoTransferDetaylari { get; set; }
        public DbSet<Irsaliye> irsaliyeler { get; set; }
        public DbSet<IrsaliyeDetay> irsaliyeDetaylari { get; set; }
        // public DbSet<Vw_StokDurumu> Vw_StokDurumu { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString: @"Data Source=MAHMUT\MSSQLSERVER1;Initial Catalog=StokTakipDb;Integrated Security=True;Trust Server Certificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepoMap());
            modelBuilder.ApplyConfiguration(new MalzemeMap());
            modelBuilder.ApplyConfiguration(new CariMap());
            modelBuilder.ApplyConfiguration(new DepoTransferMap());
            modelBuilder.ApplyConfiguration(new DepoTransferDetayMap());
            modelBuilder.ApplyConfiguration(new IrsaliyeMap());
            modelBuilder.ApplyConfiguration(new IrsaliyeDetayMap());
            modelBuilder.ApplyConfiguration(new StokMap());
            modelBuilder.ApplyConfiguration(new LogTakipMap());
            modelBuilder.ApplyConfiguration(new KullaniciMap());

        }
    }
}
