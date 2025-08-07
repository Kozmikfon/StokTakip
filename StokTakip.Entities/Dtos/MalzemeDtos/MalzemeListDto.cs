using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.MalzemeDtos
{
    public class MalzemeListDto
    {
        public int Id { get; set; }

        [DisplayName("Malzeme İsmi")]
        public string malzemeAdi { get; set; } = null!;

        [DisplayName("Birim")]
        public string? birim { get; set; }

        [DisplayName("Kategori")]
        public string kategori { get; set; } = null!;

        [DisplayName("Min. Stok Miktarı")]
        public int minStokMiktar { get; set; }

        [DisplayName("Barkod No")]
        public string? barkodNo { get; set; }

        [DisplayName("Kullanım Durumu")]
        public bool aktifPasif { get; set; }
    }
}
