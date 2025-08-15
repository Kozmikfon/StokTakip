using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.IrsaliyeDtos
{
    public class IrsaliyeCreateDto
    {
        [Required, DisplayName("İrsaliye Numarası")]
        public string IrsaliyeNo { get; set; } = null!;

        [Required, DisplayName("Cari")]
        public int CarId { get; set; }

        [Required, DisplayName("İrsaliye Tarihi")]
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime IrsaliyeTarihi { get; set; } = DateTime.Now;


        [Required, DisplayName("İrsaliye Tipi")]
        public IrsaliyeTipi IrsaliyeTipi { get; set; }

        [DisplayName("Açıklama")]
        public string? Aciklama { get; set; }

        [Required, DisplayName("Depo")]
        public int DepoId { get; set; }

        // Tek sayfa akışında ilk oluştururken boş bırakılabilir
        public List<IrsaliyeDetayCreateDto>? Detaylar { get; set; }
    }
}
