using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.IrsaliyeDtos
{
    public class IrsaliyeUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public int DepoId { get; set; }

        public int? HedefDepoId { get; set; }

        [Required]
        public IrsaliyeTipi IrsaliyeTipi { get; set; }

        public List<IrsaliyeDetayCreateDto>? Detaylar { get; set; }
    }
}
