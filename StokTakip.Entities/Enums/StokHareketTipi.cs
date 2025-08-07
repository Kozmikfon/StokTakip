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
        [Display(Name = "İrsaliye Girişi")]
        IrsaliyeGiris = 1,

        [Display(Name = "İrsaliye Çıkışı")]
        IrsaliyeCikis = 2,

        [Display(Name = "Transfer Girişi")]
        TransferGiris = 3,

        [Display(Name = "Transfer Çıkışı")]
        TransferCikis = 4
    }

}
