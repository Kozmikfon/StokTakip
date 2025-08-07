using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Enums;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using StokTakip.Shared.Utilities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Concrete
{
    public class IrsaliyeDetayService : IIrsaliyeDetayService
    {
        private readonly IUnitOfWork _unitOfWork;   
        private readonly IMapper _mapper;
        public IrsaliyeDetayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDataResult<IrsaliyeDetayDto>> CreateAsync(IrsaliyeDetayCreateDto dto)
        {
            // 1. Ara toplam hesapla
            var araToplam = dto.miktar * dto.birimFiyat;

            // 2. İrsaliye kontrolü
            var irsaliye = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == dto.irsaliyeId);
            if (irsaliye == null)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "Hata, irsaliye bulunamadı.", null);


            // 3. IrsaliyeDetay oluştur
            var detay = _mapper.Map<IrsaliyeDetay>(dto);
            detay.araToplam = araToplam;

            await _unitOfWork.IrsaliyeDetay.AddAsync(detay);

            // 4. Stok kaydı oluştur
            var stok = new Stok
            {
                MalzemeId = dto.malzemeId,
                DepoId = irsaliye.depoId,
                HareketTarihi = DateTime.Now,
                Miktar = irsaliye.irsaliyeTipi == IrsaliyeTipi.Giris ? dto.miktar : -dto.miktar,
                HareketTipi = StokHareketTipi.IrsaliyeGiris,
                ReferansId = dto.irsaliyeId,
                Aciklama = "İrsaliye üzerinden otomatik stok kaydı",
                carId = irsaliye.carId,
                SeriNo = dto.seriNo
            };

            await _unitOfWork.Stok.AddAsync(stok);

            // 5. Save işlemi
            await _unitOfWork.SaveAsync();
            return new DataResult<IrsaliyeDetayDto>(ResultStatus.Success, "İrsaliye detay başarıyla oluşturuldu.", _mapper.Map<IrsaliyeDetayDto>(detay));

        }
    }
}
