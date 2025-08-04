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
    public class KullaniciMap : IEntityTypeConfiguration<Kullanici>
    {
        public void Configure(EntityTypeBuilder<Kullanici> builder)
        {
            builder.ToTable("kullanicilar");

            builder.HasKey(k => k.kullaniciId);
            builder.Property(k => k.kullaniciId)
                   .ValueGeneratedOnAdd();

            builder.Property(k => k.adSoyad)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(k => k.email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(k => k.password)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(k => k.olusturulmaTarihi)
                   .IsRequired();
        }
    }
}
