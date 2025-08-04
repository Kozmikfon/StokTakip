using StokTakip.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Concrete
{
    public class DepoTransferDetay : EntityBase, IEntity
    {
        [Key]
        public int detayId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Transfer Numarası:")]
        public int transferId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Malzeme Numarası:")]
        public int malzemeId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Miktar:")]
        public decimal miktar { get; set; }

        [ForeignKey(nameof(transferId))]
        public DepoTransfer? depoTransfer { get; set; }


        [ForeignKey(nameof(malzemeId))]
        public Malzeme? malzeme { get; set; }
    }
}
