using StokTakip.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Concrete
{
    public class Depo : EntityBase,IEntity
    {
        [Key]
        public int depoId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [StringLength(100)]
        [DisplayName("Depo İsmi:")]
        public string depoAd { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Raf Bilgisi:")]
        public string rafBilgisi { get; set; }

        [MaxLength(500)]
        [DisplayName("Açıklama:")]
        public string? aciklama { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [DisplayName("Konum Bilgisi:")]
        public string konumBilgisi { get; set; }

    }
}
