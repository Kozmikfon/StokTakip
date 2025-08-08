using AutoMapper;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.CariDtos;
using StokTakip.Entities.Dtos.DepoDtos;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Entities.Dtos.MalzemeDtos;
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

            //malzeme
            CreateMap<Malzeme,MalzemeDto>().ReverseMap();
            CreateMap<Malzeme, MalzemeCreateDto>().ReverseMap();
            CreateMap<Malzeme, MalzemeListDto>().ReverseMap();

            //irsaliye
            CreateMap<Irsaliye, IrsaliyeDto>().ReverseMap();
            CreateMap<Irsaliye, IrsaliyeCreateDto>().ReverseMap();
            CreateMap<Irsaliye, IrsaliyeListDto>().ReverseMap();

            //irsaliyeDetay
            CreateMap<IrsaliyeDetay, IrsaliyeDetayDto>().ReverseMap();
            CreateMap<IrsaliyeDetay, IrsaliyeDetayCreateDto>().ReverseMap();

            


        }
    }
}
