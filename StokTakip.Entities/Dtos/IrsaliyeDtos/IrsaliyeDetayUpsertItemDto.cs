using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.IrsaliyeDtos
{
    public class IrsaliyeDetayUpsertItemDto
    {
        public int? Id { get; set; }          // null → yeni satır
        [Required] public int malzemeId { get; set; }
        [Required][Range(0.0001, double.MaxValue)] public decimal miktar { get; set; }
        [Required][Range(0, double.MaxValue)] public decimal birimFiyat { get; set; }
        public string? seriNo { get; set; }
    }
}
