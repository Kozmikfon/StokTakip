using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StokTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Data.Concrete.EFcore.Mapping
{
    public class StokMap : IEntityTypeConfiguration<Stok>
    {
        public void Configure(EntityTypeBuilder<Stok> builder)
        {
            builder.ToTable("Stoklar");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(s => s.MalzemeId)
                   .IsRequired();

            builder.Property(s => s.DepoId)
                   .IsRequired();

            builder.Property(s => s.HareketTarihi)
                   .IsRequired();

            builder.Property(s => s.Miktar)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(s => s.HareketTipi)
                   .IsRequired()
                   .HasConversion<int>(); // Enum için

            builder.Property(s => s.ReferansId); // opsiyonel

            builder.Property(s => s.Aciklama)
                   .HasMaxLength(500);

            builder.Property(s => s.carId);

            builder.Property(s => s.SeriNo)
                   .HasMaxLength(100);

            // İlişki: stok → malzeme
            builder.HasOne(s => s.Malzeme)
                   .WithMany()
                   .HasForeignKey(s => s.MalzemeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // İlişki: stok → depo
            builder.HasOne(s => s.Depo)
                   .WithMany()
                   .HasForeignKey(s => s.DepoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // İlişki: stok → cari (opsiyonel)
            builder.HasOne(s => s.cari)
                   .WithMany()
                   .HasForeignKey(s => s.carId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
