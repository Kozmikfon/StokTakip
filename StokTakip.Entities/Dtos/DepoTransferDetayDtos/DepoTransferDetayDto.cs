using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.DepoTransferDetayDtos
{
    public class DepoTransferDetayDto
    {
        public int Id { get; set; } // EntityBase'den geliyor

        public int TransferId { get; set; }
        public int MalzemeId { get; set; }
        public string? MalzemeAdi { get; set; }

        public string? KaynakDepoAdi { get; set; }
        public string? HedefDepoAdi { get; set; }
        public decimal Miktar { get; set; }
        public string? Aciklama { get; set; }
        public string? SeriNo { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
