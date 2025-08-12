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

        // ----- Helpers -----
        private static int GetSign(IrsaliyeTipi tip) => tip switch
        {
            IrsaliyeTipi.Giris => +1,
            IrsaliyeTipi.Cikis => -1,
            _ => throw new InvalidOperationException("Transfer burada işlenmiyor.")
        };

        private async Task<Result> AdjustStockAsync(int depoId, int malzemeId, decimal delta)
        {
            var stok = await _unitOfWork.Stok.GetAsync(s => s.DepoId == depoId && s.MalzemeId == malzemeId);
            var isNew = false;

            if (stok == null)
            {
                if (delta < 0)
                    return new Result(ResultStatus.Error, "Yetersiz stok (kayıt yok).");

                stok = new Stok { DepoId = depoId, MalzemeId = malzemeId, Miktar = 0 };
                isNew = true;
            }

            var yeni = stok.Miktar + delta;
            if (yeni < 0)
                return new Result(ResultStatus.Error, "Yetersiz stok. İşlem iptal edildi.");

            stok.Miktar = yeni;

            if (isNew)
                await _unitOfWork.Stok.AddAsync(stok);   // YENİ: sadece Add
            else
                await _unitOfWork.Stok.UpdateAsync(stok); // VAR OLAN: Update

            return new Result(ResultStatus.Success);
        }


        // ----- CREATE -----
        public async Task<IDataResult<IrsaliyeDetayDto>> CreateAsync(IrsaliyeDetayCreateDto dto)
        {
            if (dto == null)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "Geçersiz veri.", null);

            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == dto.irsaliyeId);
            if (irs == null)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "İrsaliye bulunamadı.", null);

            var sign = GetSign(irs.irsaliyeTipi);

            // Stok toplamını güncelle
            var res = await AdjustStockAsync(irs.depoId, dto.malzemeId, sign * dto.miktar);
            if (res.ResultStatus != ResultStatus.Success)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, res.Info ?? "Stok güncellenemedi.", null);

            // Detay satırını ekle
            var detay = _mapper.Map<IrsaliyeDetay>(dto);
            detay.araToplam = dto.miktar * dto.birimFiyat;

            await _unitOfWork.IrsaliyeDetay.AddAsync(detay);
            await _unitOfWork.SaveAsync();

            var outDto = _mapper.Map<IrsaliyeDetayDto>(detay);
            return new DataResult<IrsaliyeDetayDto>(ResultStatus.Success, "İrsaliye detay oluşturuldu ve stok güncellendi.", outDto);
        }

        // ----- DELETE (stok geri al + detay sil) -----
        public async Task<IResult> DeleteAsync(int detayId)
        {
            var detay = await _unitOfWork.IrsaliyeDetay.GetAsync(d => d.Id == detayId);
            if (detay == null)
                return new Result(ResultStatus.Error, "Detay bulunamadı.");

            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == detay.irsaliyeId);
            if (irs == null)
                return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");

            var sign = GetSign(irs.irsaliyeTipi);
            var reverse = -sign * detay.miktar;

            var res = await AdjustStockAsync(irs.depoId, detay.malzemeId, reverse);
            if (res.ResultStatus != ResultStatus.Success)
                return new Result(ResultStatus.Error, res.Info ?? "Stok geri alınamadı.");

            await _unitOfWork.IrsaliyeDetay.DeleteAsync(detay);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Detay silindi ve stok geri alındı.");
        }

        // ----- LIST by header -----
        public async Task<IDataResult<List<IrsaliyeDetayDto>>> GetByIrsaliyeIdAsync(int irsaliyeId)
        {
            var list = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irsaliyeId);
            var dtoList = _mapper.Map<List<IrsaliyeDetayDto>>(list);
            return new DataResult<List<IrsaliyeDetayDto>>(ResultStatus.Success, dtoList);
        }
    }
}
