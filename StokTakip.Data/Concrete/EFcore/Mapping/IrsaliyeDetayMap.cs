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
    public class IrsaliyeDetayMap : IEntityTypeConfiguration<IrsaliyeDetay>
    {
        public void Configure(EntityTypeBuilder<IrsaliyeDetay> builder)
        {
            builder.ToTable("irsaliyeDetaylari");

            builder.HasKey(d => d.detayId);
            builder.Property(d => d.detayId)
                   .ValueGeneratedOnAdd();

            builder.Property(d => d.irsaliyeId)
                   .IsRequired();

            builder.Property(d => d.malzemeId)
                   .IsRequired();

            builder.Property(d => d.miktar)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(d => d.birimFiyat)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(d => d.araToplam)
                   .HasColumnType("decimal(18,2)");

            builder.Property(d => d.seriNo)
                   .HasMaxLength(100);

            // İlişki: Detay → İrsaliye
            builder.HasOne(d => d.irsaliye)
                   .WithMany(i => i.irsaliyeDetaylari)
                   .HasForeignKey(d => d.irsaliyeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // İlişki: Detay → Malzeme
            builder.HasOne(d => d.malzeme)
                   .WithMany()
                   .HasForeignKey(d => d.malzemeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
