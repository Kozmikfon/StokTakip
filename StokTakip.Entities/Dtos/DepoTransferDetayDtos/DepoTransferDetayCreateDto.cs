using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.DepoTransferDetayDtos
{
    public class DepoTransferDetayCreateDto
    {
        [Required(ErrorMessage = "Transfer numarası boş geçilemez.")]
        [DisplayName("Transfer Numarası")]
        public int TransferId { get; set; }

        [Required(ErrorMessage = "Malzeme seçilmelidir.")]
        [DisplayName("Malzeme")]
        public int MalzemeId { get; set; }

        [Required(ErrorMessage = "Miktar boş geçilemez.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Miktar sıfırdan büyük olmalıdır.")]
        [DisplayName("Miktar")]
        public decimal Miktar { get; set; }
    }
}
