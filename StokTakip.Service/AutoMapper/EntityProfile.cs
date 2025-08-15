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
    

            CreateMap<IrsaliyeDetay, IrsaliyeDetayDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));





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
                .ForMember(d => d.irsaliyeNo, m => m.MapFrom(s => s.IrsaliyeNo))
                .ForMember(d => d.carId, m => m.MapFrom(s => s.CarId))
                .ForMember(d => d.irsaliyeTarihi, m => m.MapFrom(s => s.IrsaliyeTarihi))
                .ForMember(d => d.irsaliyeTipi, m => m.MapFrom(s => s.IrsaliyeTipi))
                .ForMember(d => d.aciklama, m => m.MapFrom(s => s.Aciklama))
                .ForMember(d => d.depoId, m => m.MapFrom(s => s.DepoId));


            // Irsaliye <-> UpdateDto

            CreateMap<IrsaliyeUpdateDto, Irsaliye>()
                .ForMember(d => d.irsaliyeNo, m => m.MapFrom(s => s.IrsaliyeNo))
                .ForMember(d => d.carId, m => m.MapFrom(s => s.CarId))
                .ForMember(d => d.irsaliyeTarihi, m => m.MapFrom(s => s.IrsaliyeTarihi))
                .ForMember(d => d.irsaliyeTipi, m => m.MapFrom(s => s.IrsaliyeTipi))
                .ForMember(d => d.aciklama, m => m.MapFrom(s => s.Aciklama))
                .ForMember(d => d.depoId, m => m.MapFrom(s => s.DepoId))
                .ForMember(d => d.toplamTutar, m => m.Ignore());


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
            .ForMember(dest => dest.TransferTarihi, opt => opt.MapFrom(src => src.transferTarihi))
             .ForMember(dest => dest.Durum, opt => opt.MapFrom(src => src.durum));

            CreateMap<IrsaliyeDetayCreateDto, IrsaliyeDetay>()
    .ForMember(d => d.araToplam, m => m.MapFrom(s => s.miktar * s.birimFiyat));

            CreateMap<IrsaliyeDetayUpdateDto, IrsaliyeDetay>()
                .ForMember(d => d.araToplam, m => m.MapFrom(s => s.miktar * s.birimFiyat));

       


            CreateMap<IrsaliyeDetay, IrsaliyeDetayDto>()
             .ForMember(d => d.malzemeAd, m => m.MapFrom(s => s.malzeme.malzemeAdi)) 
            .ForMember(d => d.Id, m => m.MapFrom(s => s.Id)); 














        }
    }
}
