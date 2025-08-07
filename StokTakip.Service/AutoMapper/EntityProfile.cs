using AutoMapper;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.CariDtos;
using StokTakip.Entities.Dtos.DepoDtos;
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
            //cari
            CreateMap<Cari,CariUpdateDto>().ReverseMap();
            CreateMap<Cari,CariDto>().ReverseMap();
            CreateMap<Cari,CariCreateDto>().ReverseMap();
            
            //depo
            CreateMap<Depo,DepoDto>().ReverseMap();
            CreateMap<Depo, DepoUpdateDto>().ReverseMap();
            CreateMap<Depo, DepoCreateDto>().ReverseMap();

        }
    }
}
