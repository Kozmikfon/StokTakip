using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.IdentityUser
{
    public class AppUser:IdentityUser<int>
    {
        public string AdSoyad { get; set; } = string.Empty;
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;
    }
}
