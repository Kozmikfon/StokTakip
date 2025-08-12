using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.ViewModels
{
    public class IrsaliyeCreateVm
    {
        public IrsaliyeCreateDto Header { get; set; } = new IrsaliyeCreateDto
        {
            IrsaliyeTarihi = DateTime.Now,
            Durum = true,
            ToplamTutar = 0
        };

        public IEnumerable<SelectListItem> Cariler { get; set; } = [];
        public IEnumerable<SelectListItem> Depolar { get; set; } = [];

        // Enum için dropdown
        public IEnumerable<SelectListItem> IrsaliyeTipleri { get; set; } =
            new List<SelectListItem>
            {
            new("Giriş", ((int)IrsaliyeTipi.Giris).ToString()),
            new("Çıkış", ((int)IrsaliyeTipi.Cikis).ToString())
                // Transfer bu akışta yok; ayrı ekrandı
            };
    }
}
