using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.DepoTransferDtos
{
    public class DepoTransferListDto
    {
        public int Id { get; set; }

        [DisplayName("Transfer No")]
        public int TransferNo { get; set; }

        [DisplayName("Kaynak Depo")]
        public string? KaynakDepoAdi { get; set; }

        [DisplayName("Hedef Depo")]
        public string? HedefDepoAdi { get; set; }

        [DisplayName("Transfer Tarihi")]
        public DateTime TransferTarihi { get; set; }
    }
}
