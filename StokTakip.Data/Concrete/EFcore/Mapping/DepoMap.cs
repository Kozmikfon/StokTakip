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
    public class DepoMap : IEntityTypeConfiguration<Depo>
    {
        public void Configure(EntityTypeBuilder<Depo> builder)
        {
            // Tablo adı
            builder.ToTable("Depolar");

            // Primary key
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id)
                   .ValueGeneratedOnAdd();

            // depoAd
            builder.Property(d => d.depoAd)
                   .IsRequired()
                   .HasMaxLength(100);

            // rafBilgisi
            builder.Property(d => d.rafBilgisi)
                   .IsRequired();

            // açıklama
            builder.Property(d => d.aciklama)
                   .HasMaxLength(500);

            // konumBilgisi
            builder.Property(d => d.konumBilgisi)
                   .IsRequired();

        }
    }
}
