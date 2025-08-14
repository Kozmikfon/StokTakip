using AutoMapper;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.CariDtos;
using StokTakip.Entities.Dtos.DepoDtos;
using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Entities.Dtos.DepoTransferDtos;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Entities.Dtos.LogTakipDtos;
using StokTakip.Entities.Dtos.MalzemeDtos;
using StokTakip.Entities.Dtos.StokDtos;
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
            CreateMap<Cari, CariListDto>().ReverseMap();

            //depo
            CreateMap<Depo,DepoDto>().ReverseMap();
            CreateMap<Depo, DepoUpdateDto>().ReverseMap();
            CreateMap<Depo, DepoCreateDto>().ReverseMap();
            CreateMap<Depo, DepoListDto>().ReverseMap();

            //malzeme
            CreateMap<Malzeme,MalzemeDto>().ReverseMap();
            CreateMap<Malzeme, MalzemeCreateDto>().ReverseMap();
            CreateMap<Malzeme, MalzemeListDto>().ReverseMap();

            //irsaliye
            CreateMap<Irsaliye, IrsaliyeDto>().ReverseMap();
            CreateMap<Irsaliye, IrsaliyeCreateDto>().ReverseMap();
            CreateMap<Irsaliye, IrsaliyeListDto>().ReverseMap();
            CreateMap<Irsaliye, IrsaliyeDto>()
                .ForMember(d => d.Irsaliye, o => o.MapFrom(s => s))
                .ForMember(d => d.Detaylar, o => o.Ignore());

            CreateMap<IrsaliyeDetay, IrsaliyeDetayDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));

            CreateMap<IrsaliyeDetay, IrsaliyeDetayDto>()
                 .ForMember(dest => dest.malzemeAdi, opt => opt.MapFrom(src => src.malzeme.malzemeAdi))
                 .ForMember(dest => dest.irsaliyeNo, opt => opt.MapFrom(src => src.irsaliye.irsaliyeNo));



            //irsaliyeDetay
            CreateMap<IrsaliyeDetay, IrsaliyeDetayDto>().ReverseMap();
            CreateMap<IrsaliyeDetay, IrsaliyeDetayCreateDto>().ReverseMap();
            CreateMap<IrsaliyeDetayCreateDto, IrsaliyeDetay>()
                 .ForMember(d => d.araToplam, o => o.MapFrom(s => s.miktar * s.birimFiyat));

            //Stok

            CreateMap<Stok, StokDto>()
           .ForMember(dest => dest.MalzemeAdi, opt => opt.MapFrom(src => src.Malzeme != null ? src.Malzeme.malzemeAdi : null))
           .ForMember(dest => dest.DepoAdi, opt => opt.MapFrom(src => src.Depo != null ? src.Depo.depoAd : null))
           .ForMember(dest => dest.CariUnvan, opt => opt.MapFrom(src => src.cari != null ? src.cari.unvan : null));


            //logtakip
            CreateMap<LogTakip,LogTakipDto>().ReverseMap();

            CreateMap<DepoTransfer,DepoTransferDto>().ReverseMap();
            CreateMap<DepoTransfer, DepoTransferCreateDto>().ReverseMap();
            CreateMap<DepoTransfer, DepoTransferListDto>()
             .ForMember(dest => dest.KaynakDepoAdi, opt => opt.MapFrom(src => src.kaynakDepo.depoAd))
             .ForMember(dest => dest.HedefDepoAdi, opt => opt.MapFrom(src => src.hedefDepo.depoAd));

            CreateMap<DepoTransfer, DepoTransferUpdateDto>().ReverseMap();

            CreateMap<DepoTransferDetay, DepoTransferDetayDto>().ReverseMap();
            CreateMap<DepoTransferDetay, DepoTransferDetayCreateDto>().ReverseMap();
            
            CreateMap<DepoTransferDetay, DepoTransferDetayUpdateDto>().ReverseMap();


            // Irsaliye <-> CreateDto alan adı düzeltmeleri
            CreateMap<IrsaliyeCreateDto, Irsaliye>()
                .ForMember(d => d.irsaliyeNo, o => o.MapFrom(s => s.IrsaliyeNo))
                .ForMember(d => d.carId, o => o.MapFrom(s => s.CarId))
                .ForMember(d => d.irsaliyeTarihi, o => o.MapFrom(s => s.IrsaliyeTarihi))
                .ForMember(d => d.toplamTutar, o => o.MapFrom(s => s.ToplamTutar))
                .ForMember(d => d.irsaliyeTipi, o => o.MapFrom(s => s.IrsaliyeTipi))
                .ForMember(d => d.aciklama, o => o.MapFrom(s => s.Aciklama))
                .ForMember(d => d.durum, o => o.MapFrom(s => s.Durum))
                .ForMember(d => d.depoId, o => o.MapFrom(s => s.DepoId));


            // Irsaliye <-> UpdateDto
            CreateMap<IrsaliyeUpdateDto, Irsaliye>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.irsaliyeTarihi, o => o.MapFrom(s => s.Tarih))
                .ForMember(d => d.depoId, o => o.MapFrom(s => s.DepoId))
                .ForMember(d => d.irsaliyeTipi, o => o.MapFrom(s => s.IrsaliyeTipi));


            CreateMap<DepoTransferDetay, DepoTransferDetayDto>()
               .ForMember(dest => dest.MalzemeAdi, opt => opt.MapFrom(src => src.malzeme.malzemeAdi))
               .ForMember(dest => dest.SeriNo, opt => opt.MapFrom(src => src.depoTransfer.seriNo))
              .ForMember(dest => dest.Aciklama, opt => opt.MapFrom(src => src.depoTransfer.aciklama))
                .ForMember(dest => dest.KaynakDepoAdi, opt => opt.MapFrom(src => src.depoTransfer.kaynakDepo.depoAd))
            .ForMember(dest => dest.HedefDepoAdi, opt => opt.MapFrom(src => src.depoTransfer.hedefDepo.depoAd));

            CreateMap<DepoTransfer, DepoTransferDto>()
             .ForMember(dest => dest.KaynakDepoAdi, opt => opt.MapFrom(src => src.kaynakDepo.depoAd))
             .ForMember(dest => dest.HedefDepoAdi, opt => opt.MapFrom(src => src.hedefDepo.depoAd))
             .ForMember(dest => dest.SeriNo, opt => opt.MapFrom(src => src.seriNo)) // BU SATIR
            .ForMember(dest => dest.TransferTarihi, opt => opt.MapFrom(src => src.transferTarihi));










        }
    }
}
