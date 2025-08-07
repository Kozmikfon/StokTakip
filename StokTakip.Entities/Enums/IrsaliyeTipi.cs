using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Enums
{
    public enum IrsaliyeTipi
    {
        [Display(Name = "Giriş İrsaliyesi")]
        Giris = 1,

        [Display(Name = "Çıkış İrsaliyesi")]
        Cikis = 2,

        [Display(Name = "Transfer İrsaliyesi")]
        Transfer = 3
    }
}
