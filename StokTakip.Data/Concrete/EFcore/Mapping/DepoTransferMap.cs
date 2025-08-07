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
    public class DepoTransferMap : IEntityTypeConfiguration<DepoTransfer>
    {
        public void Configure(EntityTypeBuilder<DepoTransfer> builder)
        {
            builder.ToTable("depoTransferleri");

            builder.HasKey(dt => dt.Id);
            builder.Property(dt => dt.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(dt => dt.transferNo)
                   .IsRequired();

            builder.Property(dt => dt.kaynakDepoId)
                   .IsRequired();

            builder.Property(dt => dt.hedefDepoId)
                   .IsRequired();

            builder.Property(dt => dt.transferTarihi)
                   .IsRequired();

            builder.Property(dt => dt.aciklama)
                   .HasMaxLength(500);

            builder.Property(dt => dt.seriNo)
                   .HasMaxLength(100);

            // Kaynak depo ilişkisi
            builder.HasOne(dt => dt.kaynakDepo)
                   .WithMany()
                   .HasForeignKey(dt => dt.kaynakDepoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Hedef depo ilişkisi
            builder.HasOne(dt => dt.hedefDepo)
                   .WithMany()
                   .HasForeignKey(dt => dt.hedefDepoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Detay ilişkisi (1 transfer → N detay)
            builder.HasMany(dt => dt.depoTransferDetaylari)
                   .WithOne(d => d.depoTransfer)
                   .HasForeignKey(d => d.transferId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
