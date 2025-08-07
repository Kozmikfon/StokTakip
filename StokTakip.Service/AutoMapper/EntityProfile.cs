using AutoMapper;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.CariDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.AutoMapper
{
    public class EntityProfile :Profile
    {
        public EntityProfile()
        {
            CreateMap<Cari,CariUpdateDto>().ReverseMap();
            CreateMap<Cari,CariDto>().ReverseMap();
            CreateMap<Cari,CariCreateDto>().ReverseMap();
        }
    }
}
