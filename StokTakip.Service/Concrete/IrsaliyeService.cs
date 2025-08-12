using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
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
    public class IrsaliyeService : IIrsaliyeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IrsaliyeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // -------------------- Helpers --------------------
        private int GetSign(Irsaliye irs)
        {
            return irs.irsaliyeTipi switch
            {
                IrsaliyeTipi.Giris => +1,
                IrsaliyeTipi.Cikis => -1,
                IrsaliyeTipi.Transfer => throw new InvalidOperationException("Transfer için GetSign kullanılmaz."),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private int GetSignFromDto(IrsaliyeCreateDto dto)
        {
            return dto.IrsaliyeTipi switch
            {
                IrsaliyeTipi.Giris => +1,
                IrsaliyeTipi.Cikis => -1,
                IrsaliyeTipi.Transfer => throw new InvalidOperationException("Transfer için GetSign kullanılmaz."),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private async Task<Result> AdjustStockAsync(int depoId, int malzemeId, decimal delta)
        {
            var stok = await _unitOfWork.Stok.GetAsync(s => s.DepoId == depoId && s.MalzemeId == malzemeId);
            if (stok == null)
            {
                if (delta < 0)
                    return new Result(ResultStatus.Error, "Yetersiz stok. (Stok kaydı yok)");

                stok = new Stok { DepoId = depoId, MalzemeId = malzemeId, Miktar = 0 };
                await _unitOfWork.Stok.AddAsync(stok);
            }

            var yeniMiktar = stok.Miktar + delta;
            if (yeniMiktar < 0)
                return new Result(ResultStatus.Error, "Yetersiz stok. İşlem iptal edildi.");

            stok.Miktar = yeniMiktar;
            await _unitOfWork.Stok.UpdateAsync(stok);
            return new Result(ResultStatus.Success);
        }

        // -------------------- READ --------------------
        public async Task<IDataResult<IrsaliyeDto>> Get(int id)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(x => x.Id == id);
            if (irs == null)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "İrsaliye bulunamadı", null);

            var detaylar = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == id);

            var dto = _mapper.Map<IrsaliyeDto>(irs);
            dto.Detaylar = _mapper.Map<List<IrsaliyeDetayDto>>(detaylar);
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, dto);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllAsync()
        {
            var list = await _unitOfWork.Irsaliye.GetAllAsync();
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByDepoIdAsync(int depoId)
        {
            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => i.depoId == depoId);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAsync(int malzemeId)
        {
            var irsIds = (await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.malzemeId == malzemeId))
                        .Select(d => d.irsaliyeId)
                        .Distinct()
                        .ToList();

            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => irsIds.Contains(i.Id));
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAndDepoIdAsync(int malzemeId, int depoId)
        {
            var irsIds = (await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.malzemeId == malzemeId))
                        .Select(d => d.irsaliyeId)
                        .Distinct()
                        .ToList();

            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => i.depoId == depoId && irsIds.Contains(i.Id));
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAsync()
        {
            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => !i.IsDelete);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAndActionAsync()
        {
            // i.AktifPasif alan adını projendeki gerçeğe göre güncelle
            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => !i.IsDelete);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        // -------------------- CREATE (stok etkili) --------------------s
        public async Task<IDataResult<IrsaliyeDto>> Create(IrsaliyeCreateDto dto)
        {
            if (dto == null || dto.Detaylar == null || !dto.Detaylar.Any())
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "İrsaliye satırı yok.", null);

            var irs = _mapper.Map<Irsaliye>(dto);
            irs.CreatedTime = DateTime.Now;
            await _unitOfWork.Irsaliye.AddAsync(irs);

            var sign = GetSignFromDto(dto);

            foreach (var line in dto.Detaylar)
            {
                var det = _mapper.Map<IrsaliyeDetay>(line);
                det.irsaliye = irs; // navigation ile bağla
                await _unitOfWork.IrsaliyeDetay.AddAsync(det);

                var delta = sign * det.miktar;
                var res = await AdjustStockAsync(dto.DepoId, det.malzemeId, delta);
                if (res.ResultStatus != ResultStatus.Success)
                    return new DataResult<IrsaliyeDto>(ResultStatus.Error, res.Info ?? "Stok güncellenemedi.", null);
            }

            await _unitOfWork.SaveAsync();

            var outDto = _mapper.Map<IrsaliyeDto>(irs);
            var savedDetaylar = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irs.Id);
            outDto.Detaylar = _mapper.Map<List<IrsaliyeDetayDto>>(savedDetaylar);
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, outDto);
        }

        // -------------------- UPDATE (eskiyi geri al, yeniyi uygula) --------------------
        public async Task<IDataResult<IrsaliyeDto>> Update(IrsaliyeUpdateDto dto)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == dto.Id);
            if (irs == null)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "İrsaliye bulunamadı.", null);

            var signOld = GetSign(irs);

            // 1) Eski detayların stok etkisini geri al
            var oldDetails = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irs.Id);
            foreach (var od in oldDetails)
            {
                var reverseDelta = -signOld * od.miktar;
                var res = await AdjustStockAsync(irs.depoId, od.malzemeId, reverseDelta);
                if (res.ResultStatus != ResultStatus.Success)
                    return new DataResult<IrsaliyeDto>(ResultStatus.Error, res.Info ?? "Stok geri alma başarısız.", null);
            }

            // 2) Başlık güncelle
            _mapper.Map(dto, irs);
            await _unitOfWork.Irsaliye.UpdateAsync(irs);

            var signNew = GetSign(irs);

            // 3) Eski satırları sil, yenilerini ekle
            foreach (var od in oldDetails)
                await _unitOfWork.IrsaliyeDetay.DeleteAsync(od);

            if (dto.Detaylar == null || !dto.Detaylar.Any())
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "Güncellemede satır sağlanmadı.", null);

            foreach (var ndto in dto.Detaylar)
            {
                var det = _mapper.Map<IrsaliyeDetay>(ndto);
                det.irsaliyeId = irs.Id;
                await _unitOfWork.IrsaliyeDetay.AddAsync(det);

                var delta = signNew * det.miktar;
                var res = await AdjustStockAsync(irs.depoId, det.malzemeId, delta);
                if (res.ResultStatus != ResultStatus.Success)
                    return new DataResult<IrsaliyeDto>(ResultStatus.Error, res.Info ?? "Stok güncellenemedi.", null);
            }

            await _unitOfWork.SaveAsync();

            var outDto = _mapper.Map<IrsaliyeDto>(irs);
            var newDetails = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irs.Id);
            outDto.Detaylar = _mapper.Map<List<IrsaliyeDetayDto>>(newDetails);
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, outDto);
        }

        // -------------------- DELETE (stok tersle, sonra sil) --------------------
        public async Task<IResult> Delete(int id)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == id);
            if (irs == null)
                return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");

            var sign = GetSign(irs);
            var details = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == id);

            foreach (var d in details)
            {
                var reverse = -sign * d.miktar;
                var res = await AdjustStockAsync(irs.depoId, d.malzemeId, reverse);
                if (res.ResultStatus != ResultStatus.Success)
                    return new Result(ResultStatus.Error, res.Info ?? "Stok geri alınamadı.");
            }

            foreach (var d in details)
                await _unitOfWork.IrsaliyeDetay.DeleteAsync(d);

            await _unitOfWork.Irsaliye.DeleteAsync(irs);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "İrsaliye silindi, stok geri alındı.");
        }

        // -------------------- Satır bazlı --------------------
        public async Task<IResult> AddLineAsync(int irsaliyeId, IrsaliyeDetayCreateDto lineDto)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == irsaliyeId);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");

            var det = _mapper.Map<IrsaliyeDetay>(lineDto);
            det.irsaliyeId = irsaliyeId;
            await _unitOfWork.IrsaliyeDetay.AddAsync(det);

            var sign = GetSign(irs);
            var delta = sign * det.miktar;

            var res = await AdjustStockAsync(irs.depoId, det.malzemeId, delta);
            if (res.ResultStatus != ResultStatus.Success)
                return new Result(ResultStatus.Error, res.Info ?? "Stok güncellenemedi.");

            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "Satır eklendi ve stok güncellendi.");
        }

        public async Task<IResult> RemoveLineAsync(int irsaliyeId, int detayId)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == irsaliyeId);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");

            var det = await _unitOfWork.IrsaliyeDetay.GetAsync(d => d.Id == detayId && d.irsaliyeId == irsaliyeId);
            if (det == null) return new Result(ResultStatus.Error, "Detay bulunamadı.");

            var sign = GetSign(irs);
            var reverse = -sign * det.miktar;

            var res = await AdjustStockAsync(irs.depoId, det.malzemeId, reverse);
            if (res.ResultStatus != ResultStatus.Success)
                return new Result(ResultStatus.Error, res.Info ?? "Stok geri alınamadı.");

            await _unitOfWork.IrsaliyeDetay.DeleteAsync(det);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Satır silindi ve stok geri alındı.");
        }

        // -------------------- Sadece Başlık (detaysız kayıt) --------------------
        public async Task<IDataResult<IrsaliyeDto>> CreateHeaderAsync(IrsaliyeCreateDto headerDto)
        {
            if (headerDto == null)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "Geçersiz veri.", null);

            var irs = _mapper.Map<Irsaliye>(headerDto);
            irs.CreatedTime = DateTime.Now;

            await _unitOfWork.Irsaliye.AddAsync(irs);
            await _unitOfWork.SaveAsync();

            var outDto = _mapper.Map<IrsaliyeDto>(irs);
            outDto.Detaylar = new List<IrsaliyeDetayDto>();
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, outDto);
        }
    }
}
