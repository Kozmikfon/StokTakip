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
    public class LogTakipMap : IEntityTypeConfiguration<LogTakip>
    {
       
        public void Configure(EntityTypeBuilder<LogTakip> builder)
        {
            builder.ToTable("logTakipler");

            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(l => l.tabloAdi)
                       .IsRequired()
                       .HasMaxLength(100);

            builder.Property(l => l.islemTipi)
                       .IsRequired()
                       .HasMaxLength(50);

            builder.Property(l => l.islemTarihi)
                       .IsRequired();

            builder.Property(l => l.detay)
                       .HasMaxLength(500);

            builder.Property(l => l.AppUserId)
                       .IsRequired();

            builder.HasOne(l => l.AppUser)
                       .WithMany()
                       .HasForeignKey(l => l.AppUserId)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
