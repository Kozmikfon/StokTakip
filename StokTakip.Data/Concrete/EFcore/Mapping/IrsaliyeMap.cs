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
    public class IrsaliyeMap : IEntityTypeConfiguration<Irsaliye>
    {
        public void Configure(EntityTypeBuilder<Irsaliye> builder)
        {
            builder.ToTable("irsaliyeler");

            builder.HasKey(i => i.irsaliyeId);
            builder.Property(i => i.irsaliyeId)
                   .ValueGeneratedOnAdd();

            builder.Property(i => i.irsaliyeNo)
                   .IsRequired()
                   .HasMaxLength(100); // opsiyonel ama önerilir

            builder.Property(i => i.carId)
                   .IsRequired();

            builder.Property(i => i.depoId)
                   .IsRequired();

            builder.Property(i => i.irsaliyeTarihi)
                   .IsRequired();

            builder.Property(i => i.toplamTutar)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(i => i.irsaliyeTipi)
                   .IsRequired()
                   .HasConversion<int>(); // Enum

            builder.Property(i => i.aciklama)
                   .HasMaxLength(500);

            builder.Property(i => i.durum)
                   .IsRequired();

            // Cari ilişkisi
            builder.HasOne(i => i.cari)
                   .WithMany()
                   .HasForeignKey(i => i.carId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Depo ilişkisi
            builder.HasOne(i => i.depo)
                   .WithMany()
                   .HasForeignKey(i => i.depoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Detay ilişkisi
            builder.HasMany(i => i.irsaliyeDetaylari)
                   .WithOne(d => d.irsaliye)
                   .HasForeignKey(d => d.irsaliyeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
