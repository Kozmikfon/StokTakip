using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.IrsaliyeDtos
{
    public class IrsaliyeDto
    {
        public int Id { get; set; }
        public string irsaliyeNo { get; set; }
        public int carId { get; set; }
        public DateTime irsaliyeTarihi { get; set; }
        public Enums.IrsaliyeTipi irsaliyeTipi { get; set; }
        public string? aciklama { get; set; }
        public int depoId { get; set; }

        public IrsaliyeDurumu durum { get; set; }
        public decimal? toplamTutar { get; set; }

        // Ekranda gösterim kolaylığı için isim alanları (opsiyonel)
        public string? cariAd { get; set; }
        public string? depoAd { get; set; }
        public DateTime CreatedTime { get; set; }
        public string ModifiedTime { get; set; }
        public List<IrsaliyeDetayDto> Detaylar { get; set; } = new();
    }
}
