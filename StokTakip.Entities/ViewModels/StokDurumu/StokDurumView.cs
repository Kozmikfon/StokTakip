using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.ViewModels.StokDurumu
{
    public class StokDurumView
    {
        public int DepoId { get; set; }
        public int MalzemeId { get; set; }
        public decimal KalanMiktar { get; set; }
    }
}
