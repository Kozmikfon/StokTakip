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
    public class DepoTransferDetayMap : IEntityTypeConfiguration<DepoTransferDetay>
    {
        public void Configure(EntityTypeBuilder<DepoTransferDetay> builder)
        {
            builder.ToTable("depoTransferDetaylari");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(d => d.transferId)
                   .IsRequired();

            builder.Property(d => d.malzemeId)
                   .IsRequired();

            builder.Property(d => d.miktar)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // depoTransfer ilişkisi (N detay → 1 transfer)
            builder.HasOne(d => d.depoTransfer)
                   .WithMany(dt => dt.depoTransferDetaylari)
                   .HasForeignKey(d => d.transferId)
                   .OnDelete(DeleteBehavior.Restrict);

            // malzeme ilişkisi
            builder.HasOne(d => d.malzeme)
                   .WithMany()
                   .HasForeignKey(d => d.malzemeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
