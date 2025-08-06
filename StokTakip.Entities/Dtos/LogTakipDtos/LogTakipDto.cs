using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.LogTakipDtos
{
    public class LogTakipDto
    {
        [Required(ErrorMessage = "Tablo Adı boş geçilemez")]
        [DisplayName("Tablo Adı:")]
        public string tabloAdi { get; set; } = null!;

        [Required(ErrorMessage = "İşlem Tipi boş geçilemez")]
        [DisplayName("İşlem Tipi:")]
        public string islemTipi { get; set; } = null!;

        [Required(ErrorMessage = "İşlem Tarihi boş geçilemez")]
        [DisplayName("İşlem Tarihi:")]
        public DateTime islemTarihi { get; set; }

        [DisplayName("İşlem Detayı:")]
        [MaxLength(500)]
        public string? detay { get; set; }

        [DisplayName("Kullanıcı ID:")]
        public int AppUserId { get; set; }

        // İlişkili kullanıcının adı (ekranda göstermek için kullanılabilir)
        [DisplayName("Kullanıcı Adı:")]
        public string? kullaniciAdi { get; set; }
    }
}
