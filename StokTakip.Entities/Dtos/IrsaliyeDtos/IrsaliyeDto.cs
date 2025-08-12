using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.IrsaliyeDtos
{
    public class IrsaliyeDto
    {

        public Irsaliye Irsaliye { get; set; } = null!;
        public List<IrsaliyeDetayDto> Detaylar { get; set; } = new();
    }
}
