using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.DepoTransferDtos
{
    public class DepoTransferCreateDto
    {
        [Required(ErrorMessage = "Transfer numarası boş bırakılamaz")]
        [DisplayName("Transfer Numarası")]
        public int TransferNo { get; set; }

        [Required(ErrorMessage = "Kaynak depo seçilmelidir")]
        [DisplayName("Kaynak Depo")]
        public int KaynakDepoId { get; set; }

        [Required(ErrorMessage = "Hedef depo seçilmelidir")]
        [DisplayName("Hedef Depo")]
        public int HedefDepoId { get; set; }

        [Required(ErrorMessage = "Transfer tarihi boş bırakılamaz")]
        [DisplayName("Transfer Tarihi")]
        public DateTime TransferTarihi { get; set; }

        [MaxLength(500)]
        [DisplayName("Açıklama")]
        public string? Aciklama { get; set; }

        [MaxLength(100)]
        [DisplayName("Seri Numarası")]
        public string? SeriNo { get; set; }
    }
}
