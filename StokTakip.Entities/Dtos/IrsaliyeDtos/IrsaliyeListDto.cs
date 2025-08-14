using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.IrsaliyeDtos
{
    public class IrsaliyeListDto
    {
        public int Id { get; set; }
        public string irsaliyeNo { get; set; }
        public DateTime irsaliyeTarihi { get; set; }
        public string? cariAd { get; set; }
        public string? depoAd { get; set; }
        public Enums.IrsaliyeTipi irsaliyeTipi { get; set; }
        public IrsaliyeDurumu durum { get; set; }
        public decimal? toplamTutar { get; set; }
    }
}
