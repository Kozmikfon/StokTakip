using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class IrsaliyeDetay : EntityBase,IEntity
    {
        [Key]
        public int detayId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("İrsaliye Numarası:")]
        public int irsaliyeId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Malzeme Numarası:")]
        public int malzemeId { get; set; }


        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Miktar:")]
        public decimal miktar { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Birim Fiyat:")]
        public decimal birimFiyat { get; set; }


        [DisplayName("Ara Toplam:")]
        public decimal araToplam { get; set; }

        [DisplayName("Seri Numarası:")]
        public string? seriNo { get; set; }

        [BindNever]
        [ForeignKey(nameof(irsaliyeId))]
        public Irsaliye? irsaliye { get; set; } = null!;

        [BindNever]
        [ForeignKey(nameof(malzemeId))]
        public Malzeme? malzeme { get; set; }
    }
}
