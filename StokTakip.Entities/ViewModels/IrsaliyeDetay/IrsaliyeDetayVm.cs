using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.ViewModels.IrsaliyeDetay
{
    public class IrsaliyeDetayVm
    {
        public IrsaliyeDto Irsaliye { get; set; }
        public List<IrsaliyeDetayDto> Detaylar { get; set; }
    }
}
