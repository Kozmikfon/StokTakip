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
    public class Stok : EntityBase,IEntity
    {
        [Key]
        public int HareketId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Malzeme Numarası:")]
        public int MalzemeId { get; set; }

        [ForeignKey(nameof(MalzemeId))]
        public Malzeme? Malzeme { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Depo Numarası:")]
        public int DepoId { get; set; }

        [ForeignKey(nameof(DepoId))]
        public Depo? Depo { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Hareket Tarihi:")]
        public DateTime HareketTarihi { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public decimal Miktar { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Hareket Tipi:")]
        public StokHareketTipi HareketTipi { get; set; }

        [DisplayName("Evrak Numarası:")]
        public int? ReferansId { get; set; }

        [DisplayName("Açıklama:")]
        public string? Aciklama { get; set; }


        public int? carId { get; set; }

        [ForeignKey(nameof(carId))]
        public Cari? cari { get; set; }


        [DisplayName("Seri Numarası:")]
        public string? SeriNo { get; set; }
    }
}
