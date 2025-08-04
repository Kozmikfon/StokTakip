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
    public class MalzemeMap : IEntityTypeConfiguration<Malzeme>
    {
        public void Configure(EntityTypeBuilder<Malzeme> builder)
        {
            builder.ToTable("Malzemeler");

            builder.HasKey(m => m.malzemeId);
            builder.Property(m => m.malzemeId)
                   .ValueGeneratedOnAdd();

            builder.Property(m => m.malzemeAdi)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.birim)
                   .HasMaxLength(50);

            builder.Property(m => m.kategori)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.minStokMiktar)
                   .IsRequired();

            builder.Property(m => m.barkodNo)
                   .HasMaxLength(100);

            builder.Property(m => m.aktifPasif)
                   .IsRequired();

            builder.Property(m => m.aciklama)
                   .HasMaxLength(500);
        }
    }
}
