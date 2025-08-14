using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StokTakip.Entities.Dtos.IrsaliyeDtos
{
    public class IrsaliyeUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string IrsaliyeNo { get; set; } = null!;

        [Required]
        public int CarId { get; set; }

        [Required]
        public DateTime IrsaliyeTarihi { get; set; }

        [Required]
        public int DepoId { get; set; }

        // Bu akışta Transfer işlenmiyor; istersen şimdilik null bırak
        public int? HedefDepoId { get; set; }

        [Required]
        public IrsaliyeTipi IrsaliyeTipi { get; set; }

        public string? Aciklama { get; set; }

        // Tek sayfadan gelen satırlar: upsert mantığı için CreateDto yeterli
        public List<IrsaliyeDetayCreateDto>? Detaylar { get; set; }
    }
}
