using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.CariDtos
{
    public class CariUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ünvan boş bırakılamaz")]
        [MaxLength(100)]
        [DisplayName("Ünvan")]
        public string Unvan { get; set; } = null!;

        [Required(ErrorMessage = "Telefon boş bırakılamaz")]
        [MaxLength(20)]
        [DisplayName("Telefon")]
        public string Telefon { get; set; } = null!;

        [Required(ErrorMessage = "Mail boş bırakılamaz")]
        [MaxLength(100)]
        [DisplayName("Mail Adresi")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Adres boş bırakılamaz")]
        [MaxLength(200)]
        [DisplayName("Adres")]
        public string Adres { get; set; } = null!;

        [MaxLength(50)]
        [DisplayName("Vergi No")]
        public string VergiNo { get; set; } = null!;

        [DisplayName("Vergi Dairesi")]
        public string? VergiDairesi { get; set; }
    }
}
