using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.DepoDtos
{
    public class DepoListDto
    {
        public int Id { get; set; }
        public string DepoAd { get; set; } = null!;
        public string RafBilgisi { get; set; } = null!;
        public string? Aciklama { get; set; }
        public string KonumBilgisi { get; set; } = null!;

    }
}
