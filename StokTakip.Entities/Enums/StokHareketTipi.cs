using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Enums
{
    
          public enum StokHareketTipi
        {
            [Display(Name = "Stok Girişi")]
            Giris = 1,
            [Display(Name = "Stok Çıkışı")]
            Cikis = 2,
            [Display(Name = "Transfer Girişi")]
            TransferGiris = 3,
            [Display(Name = "Transfer Çıkışı")]
            TransferCikis = 4
        }

}
