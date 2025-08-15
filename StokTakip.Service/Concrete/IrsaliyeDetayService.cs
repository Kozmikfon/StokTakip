using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Enums;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Entities.Abstract;
using StokTakip.Shared.Utilities.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using StokTakip.Shared.Utilities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private static void SoftDeleteEntity(EntityBase e)
        {
            e.IsDelete = true;
            e.IsActive = false;
            e.ModifiedTime = DateTime.Now;
        }

        // ---------------- CREATE (Taslak; stok oynamaz) ----------------
        public async Task<IDataResult<IrsaliyeDetayDto>> CreateAsync(IrsaliyeDetayCreateDto dto)
        {
            if (dto == null)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "Geçersiz veri.", null);

            if (dto.irsaliyeId <= 0)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "İrsaliye Id geçersiz.", null);

            if (dto.miktar <= 0)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "Miktar sıfırdan büyük olmalı.", null);

            if (dto.birimFiyat < 0)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "Birim fiyat negatif olamaz.", null);

            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == dto.irsaliyeId && !i.IsDelete);
            if (irs == null)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "İrsaliye bulunamadı.", null);

            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "Onaylı irsaliyeye satır eklenemez.", null);

            // Malzeme kontrolü (aktif/silinmemiş)
            var mal = await _unitOfWork.Malzeme.GetAsync(m => m.Id == dto.malzemeId && !m.IsDelete);
            if (mal == null)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "Malzeme bulunamadı veya pasif/silinmiş.", null);

            var detay = _mapper.Map<IrsaliyeDetay>(dto);
            detay.araToplam = detay.miktar * detay.birimFiyat;
            detay.IsActive = true;
            detay.IsDelete = false;
            detay.CreatedTime = DateTime.Now;
            detay.ModifiedTime = DateTime.Now;

            await _unitOfWork.IrsaliyeDetay.AddAsync(detay);
            await _unitOfWork.SaveAsync();

            // DTO’ya güvenli malzeme adı da yaz
            var outDto = _mapper.Map<IrsaliyeDetayDto>(detay);
            outDto.malzemeAd = outDto.malzemeAd ?? mal.malzemeAdi;

            return new DataResult<IrsaliyeDetayDto>(ResultStatus.Success, "Satır eklendi (Taslak).", outDto);
        }

        // ---------------- DELETE (Soft; sadece Taslak) ----------------
        public async Task<IResult> DeleteAsync(int detayId)
        {
            var detay = await _unitOfWork.IrsaliyeDetay.GetAsync(d => d.Id == detayId && !d.IsDelete);
            if (detay == null)
                return new Result(ResultStatus.Error, "Detay bulunamadı.");

            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == detay.irsaliyeId && !i.IsDelete);
            if (irs == null)
                return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");

            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı irsaliyeden satır silinemez.");

            SoftDeleteEntity(detay);
            await _unitOfWork.IrsaliyeDetay.UpdateAsync(detay);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Satır silindi (soft).");
        }

        // ---------------- LIST (nav dahil; isimler dolu gelsin) ----------------
        public async Task<IDataResult<List<IrsaliyeDetayDto>>> GetByIrsaliyeIdAsync(int irsaliyeId)
        {
            var list = await _unitOfWork.IrsaliyeDetay.GetAllAsync(
                x => x.irsaliyeId == irsaliyeId && !x.IsDelete,
                x => x.irsaliye,
                x => x.malzeme
            );

            // Görsel tutarlılık için kronolojik sırala (istersen Id’ye göre)
            list = list.OrderBy(x => x.CreatedTime).ToList();

            var dtoList = _mapper.Map<List<IrsaliyeDetayDto>>(list);

            // AutoMapper profilinde malzemeAd yoksa güvence:
            for (int i = 0; i < list.Count; i++)
            {
                dtoList[i].malzemeAd ??= list[i].malzeme?.malzemeAdi;
            }

            return new DataResult<List<IrsaliyeDetayDto>>(ResultStatus.Success, dtoList);
        }
    }
}
