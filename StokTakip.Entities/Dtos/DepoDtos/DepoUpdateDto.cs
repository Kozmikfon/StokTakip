using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.DepoDtos
{
    public class DepoUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Depo adı boş geçilemez")]
        [StringLength(100)]
        [DisplayName("Depo İsmi:")]
        public string DepoAd { get; set; } = null!;

        [Required(ErrorMessage = "Raf bilgisi boş geçilemez")]
        [DisplayName("Raf Bilgisi:")]
        public string RafBilgisi { get; set; } = null!;

        [MaxLength(500)]
        [DisplayName("Açıklama:")]
        public string? Aciklama { get; set; }

        [Required(ErrorMessage = "Konum bilgisi boş geçilemez")]
        [DisplayName("Konum Bilgisi:")]
        public string KonumBilgisi { get; set; } = null!;
    }
}
