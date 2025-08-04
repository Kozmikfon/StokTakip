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
    public class CariMap : IEntityTypeConfiguration<Cari>
    {
        public void Configure(EntityTypeBuilder<Cari> builder)
        {
            builder.ToTable("Cariler");

            builder.HasKey(c => c.carId);
            builder.Property(c => c.carId).ValueGeneratedOnAdd();

            builder.Property(c => c.unvan)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.telefon)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(c => c.email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.adres)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.vergiNo)
                   .HasMaxLength(50);

            builder.Property(c => c.vergiDairesi)
                   .HasMaxLength(100);
        }
    }
}
