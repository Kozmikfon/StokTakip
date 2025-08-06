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
    public class Cari : EntityBase, IEntity
    {
        //[Key]
        //public int carId { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [MaxLength(100)]
        [DisplayName("Ünvan:")]
        public string unvan { get; set; } = null!;

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [MaxLength(20)]
        [DisplayName("Telefon:")]
        public string telefon { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [MaxLength(100)]
        [DisplayName("Mail Adres:")]
        public string email { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        [MaxLength(200)]
        [DisplayName("Adres Bilgileri:")]
        public string adres { get; set; }

        [MaxLength(50)]
        [DisplayName("Vergi Numarası:")]
        public string vergiNo { get; set; }

        [DisplayName("Vergi Dairesi:")]
        public string? vergiDairesi { get; set; }
    }
}
