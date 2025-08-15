using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Entities.Enums;
using System.Collections.Generic;
using System.Linq;

namespace StokTakip.Entities.ViewModels
{
    public class IrsaliyePageVm
    {
        public IrsaliyeDto Irsaliye { get; set; } = new IrsaliyeDto { Detaylar = new List<IrsaliyeDetayDto>() };

        // Detaylar AJAX ile yönetildiği için POST'ta bu alanın validasyonunu istemiyoruz.
        [ValidateNever]
        public IrsaliyeUpdateDto PostModel { get; set; } = new IrsaliyeUpdateDto();

        // Dropdown’lar hiçbir zaman null olmasın
        [ValidateNever] public SelectList Cariler { get; set; } = new SelectList(Enumerable.Empty<object>());
        [ValidateNever] public SelectList Depolar { get; set; } = new SelectList(Enumerable.Empty<object>());
        [ValidateNever] public SelectList Malzemeler { get; set; } = new SelectList(Enumerable.Empty<object>());

        public bool IsOnayli => Irsaliye?.durum == IrsaliyeDurumu.Onayli;
    }
}
