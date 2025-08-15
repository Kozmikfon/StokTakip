using StokTakip.Entities.Enums;
using StokTakip.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Concrete
{
    public class DepoTransfer : EntityBase , IEntity
    {
        //[Key]
        //public int transferId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]


        [DisplayName("Transfer Numarası:")]

        public int transferNo { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Kaynak Depo Numarası::")]
        public int kaynakDepoId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Hedef Depo Numarası:")]
        public int hedefDepoId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Transfer Tarihi:")]
        public DateTime transferTarihi { get; set; }

        //durum
        public TransferDurumu durum { get; set; }

        [MaxLength(500)]
        [DisplayName("Açıklama:")]
        public string? aciklama { get; set; }

        [MaxLength(100)]
        [DisplayName("Seri Numarası:")]
        public string? seriNo { get; set; }

        [ForeignKey(nameof(kaynakDepoId))]
        public Depo? kaynakDepo { get; set; }

        [ForeignKey(nameof(hedefDepoId))]
        public Depo? hedefDepo { get; set; }
        public ICollection<DepoTransferDetay> depoTransferDetaylari { get; set; } = new List<DepoTransferDetay>();
    }
}
