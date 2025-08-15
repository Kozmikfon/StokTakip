using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.DepoTransferDtos
{
    public class DepoTransferDto
    {
        public int Id { get; set; }

        [DisplayName("Transfer Numarası")]
        public int TransferNo { get; set; }

        [DisplayName("Kaynak Depo")]
        public int KaynakDepoId { get; set; }

        [DisplayName("Hedef Depo")]
        public int HedefDepoId { get; set; }

        [DisplayName("Transfer Tarihi")]
        public DateTime TransferTarihi { get; set; }

        [DisplayName("Açıklama")]
        public string? Aciklama { get; set; }

        [DisplayName("Seri Numarası")]
        public string? SeriNo { get; set; }
        public TransferDurumu Durum { get; set; }

        //  ilişkili depo adlarını göstermek için
        public string? KaynakDepoAdi { get; set; }
        public string? HedefDepoAdi { get; set; }
        public List<DepoTransferDetayDto>? Detaylar { get; set; }
    }
}
