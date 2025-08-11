using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Entities.Dtos.CariDtos
{
    public class CariListDto
    {
        public int Id { get; set; }
        public string Unvan { get; set; } = null!;
        public string Telefon { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string VergiNo { get; set; } = null!;
    }
}
