using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.ViewModels
{

    public class IrsaliyePageVm
    {
        public IrsaliyeDto Irsaliye { get; set; } = new IrsaliyeDto { Detaylar = new List<IrsaliyeDetayDto>() };
        public IrsaliyeUpdateDto PostModel { get; set; } = new IrsaliyeUpdateDto();

        public SelectList Cariler { get; set; } = new SelectList(Enumerable.Empty<object>());
        public SelectList Depolar { get; set; } = new SelectList(Enumerable.Empty<object>());
        public SelectList Malzemeler { get; set; } = new SelectList(Enumerable.Empty<object>());

        public bool IsOnayli => Irsaliye?.durum == IrsaliyeDurumu.Onayli;
    }

}
