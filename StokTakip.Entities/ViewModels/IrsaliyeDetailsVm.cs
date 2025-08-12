using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.ViewModels
{
    public class IrsaliyeDetailsVm
    {
        public IrsaliyeDto Header { get; set; } = new();
        public IrsaliyeDetayCreateDto NewLine { get; set; } = new();

        // Kısa yol (Header.Detaylar null gelirse boş liste döner)
        public List<IrsaliyeDetayDto> Lines => Header?.Detaylar ?? new List<IrsaliyeDetayDto>();
    }
}
