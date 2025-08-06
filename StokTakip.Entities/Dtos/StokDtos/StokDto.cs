using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.StokDtos
{
    public class StokDto
    {
        [Required(ErrorMessage = "Malzeme ID boş geçilemez")]
        [DisplayName("Malzeme Numarası:")]
        public int MalzemeId { get; set; }

        [DisplayName("Malzeme Adı:")]
        public string? MalzemeAdi { get; set; } // JOIN ile listelemede gösterim için

        [Required(ErrorMessage = "Depo ID boş geçilemez")]
        [DisplayName("Depo Numarası:")]
        public int DepoId { get; set; }

        [DisplayName("Depo Adı:")]
        public string? DepoAdi { get; set; }

        [Required(ErrorMessage = "Hareket Tarihi boş geçilemez")]
        [DisplayName("Hareket Tarihi:")]
        public DateTime HareketTarihi { get; set; }

        [Required(ErrorMessage = "Miktar boş geçilemez")]
        [DisplayName("Miktar:")]
        public decimal Miktar { get; set; }

        [Required(ErrorMessage = "Hareket Tipi boş geçilemez")]
        [DisplayName("Hareket Tipi:")]
        public StokHareketTipi HareketTipi { get; set; }

        [DisplayName("Evrak Numarası:")]
        public int? ReferansId { get; set; }

        [DisplayName("Açıklama:")]
        public string? Aciklama { get; set; }

        [DisplayName("Cari ID:")]
        public int? carId { get; set; }

        [DisplayName("Cari Ünvan:")]
        public string? CariUnvan { get; set; }

        [DisplayName("Seri Numarası:")]
        public string? SeriNo { get; set; }
    }
}
