using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.ViewModels
{
    public class IrsaliyeLinesVm
    {
        // Header özeti
        public int IrsaliyeId { get; set; }
        public string IrsaliyeNo { get; set; } = "";
        public DateTime IrsaliyeTarihi { get; set; }
        public IrsaliyeTipi IrsaliyeTipi { get; set; }
        public string? CariUnvan { get; set; }
        public string? DepoAd { get; set; }
        public string? Aciklama { get; set; }

        // Liste + yeni satır
        public List<IrsaliyeDetayDto> Satirlar { get; set; } = new();
        public IrsaliyeDetayCreateDto YeniSatir { get; set; } = new();

        // Dropdown
        public IEnumerable<SelectListItem> Malzemeler { get; set; } = [];
    }
}
