using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.IrsaliyeDetayDtos
{
    public class IrsaliyeDetayCreateDto
    {
        [Required(ErrorMessage = "İrsaliye Numarası boş geçilemez")]
        [DisplayName("İrsaliye Numarası:")]
        public int irsaliyeId { get; set; }

        [Required(ErrorMessage = "Malzeme Numarası boş geçilemez")]
        [DisplayName("Malzeme Numarası:")]
        public int malzemeId { get; set; }

        [Required(ErrorMessage = "Miktar boş geçilemez")]
        [DisplayName("Miktar:")]
        public decimal miktar { get; set; }

        [Required(ErrorMessage = "Birim Fiyat boş geçilemez")]
        [DisplayName("Birim Fiyat:")]
        public decimal birimFiyat { get; set; }

        [DisplayName("Seri Numarası:")]
        public string? seriNo { get; set; }
    }
}
