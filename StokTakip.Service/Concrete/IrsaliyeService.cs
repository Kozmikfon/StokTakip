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

        // ---- Helpers ----
        private static int GetSign(IrsaliyeTipi tip) => tip switch
        {
            IrsaliyeTipi.Giris => +1,
            IrsaliyeTipi.Cikis => -1,
            IrsaliyeTipi.Transfer => throw new InvalidOperationException("Transfer bu servisle işlenmiyor."),
            _ => throw new ArgumentOutOfRangeException(nameof(tip))
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

            var yeniMiktar = stok.Miktar + delta;
            if (yeniMiktar < 0)
                return new Result(ResultStatus.Error, "Yetersiz stok.");

            stok.Miktar = yeniMiktar;
            if (isNew) await _unitOfWork.Stok.AddAsync(stok);
            else await _unitOfWork.Stok.UpdateAsync(stok);

            return new Result(ResultStatus.Success);
        }

        // ---- READ ----
        public async Task<IDataResult<IrsaliyeDto>> Get(int id)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(x => x.Id == id);
            if (irs == null)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "İrsaliye bulunamadı.", null);

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

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAndActionAsync()
        {
            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => !i.IsDelete);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        // ---- CREATE HEADER (Taslak) ----
        // Tek sayfa akışı için ilk başlığı oluşturur; stok oynamaz.
        public async Task<IDataResult<IrsaliyeDto>> CreateHeaderAsync(IrsaliyeCreateDto headerDto)
        {
            if (headerDto == null)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "Geçersiz veri.", null);

            var irs = _mapper.Map<Irsaliye>(headerDto);
            irs.CreatedTime = DateTime.Now;
            irs.durum = IrsaliyeDurumu.Taslak;

            await _unitOfWork.Irsaliye.AddAsync(irs);
            await _unitOfWork.SaveAsync();

            var outDto = _mapper.Map<IrsaliyeDto>(irs);
            outDto.Detaylar = new List<IrsaliyeDetayDto>();
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, outDto);
        }

        // ---- UPSERT (Taslak’ta başlık + detaylar) ----
        // Tek sayfadan gelen veriyi Taslak üzerinde günceller; stok oynamaz.
        public async Task<IDataResult<IrsaliyeDto>> UpsertAsync(IrsaliyeUpdateDto dto)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == dto.Id);
            if (irs == null)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "İrsaliye bulunamadı.", null);

            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "Onaylı irsaliye güncellenemez.", null);

            // Başlık alanlarını güncelle
            _mapper.Map(dto, irs);
            await _unitOfWork.Irsaliye.UpdateAsync(irs);

            // Detay upsert: basit yaklaşım → eski satırları sil, yenilerini ekle (Taslakta serbest)
            var oldDetails = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irs.Id);
            foreach (var od in oldDetails)
                await _unitOfWork.IrsaliyeDetay.DeleteAsync(od);

            if (dto.Detaylar != null && dto.Detaylar.Any())
            {
                foreach (var ndto in dto.Detaylar)
                {
                    var det = _mapper.Map<IrsaliyeDetay>(ndto);
                    det.irsaliyeId = irs.Id;
                    det.araToplam = det.miktar * det.birimFiyat;
                    await _unitOfWork.IrsaliyeDetay.AddAsync(det);
                }
            }

            // Toplam (görsel amaçlı) başlığa yazılabilir
            var newDetails = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irs.Id);
            irs.toplamTutar = newDetails.Sum(x => x.miktar * x.birimFiyat);

            await _unitOfWork.SaveAsync();

            var outDto = _mapper.Map<IrsaliyeDto>(irs);
            outDto.Detaylar = _mapper.Map<List<IrsaliyeDetayDto>>(newDetails);
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, outDto);
        }

        // ---- DELETE (Sadece Taslak) ----
        public async Task<IResult> Delete(int id)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == id);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");

            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı irsaliye silinemez.");

            var details = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == id);
            foreach (var d in details)
                await _unitOfWork.IrsaliyeDetay.DeleteAsync(d);

            await _unitOfWork.Irsaliye.DeleteAsync(irs);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Taslak irsaliye silindi.");
        }

        // ---- TALep/ONAY (Stok burada uygulanır) ----
        public async Task<IResult> TalepOlusturAsync(int irsaliyeId)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == irsaliyeId);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");
            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "İrsaliye zaten onaylı.");

            if (irs.irsaliyeTipi == IrsaliyeTipi.Transfer)
                return new Result(ResultStatus.Error, "Transfer bu akışta onaylanmaz.");

            var details = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irs.Id);
            if (details == null || !details.Any())
                return new Result(ResultStatus.Error, "İrsaliyede satır yok.");

            var sign = GetSign(irs.irsaliyeTipi);

            // 1) Çıkış ise yeterlilik kontrolü
            if (irs.irsaliyeTipi == IrsaliyeTipi.Cikis)
            {
                foreach (var d in details)
                {
                    var stok = await _unitOfWork.Stok.GetAsync(s => s.DepoId == irs.depoId && s.MalzemeId == d.malzemeId);
                    var mevcut = stok?.Miktar ?? 0;
                    if (mevcut < d.miktar)
                        return new Result(ResultStatus.Error, $"Yetersiz stok: MalzemeId={d.malzemeId}, gerekli={d.miktar}, mevcut={mevcut}.");
                }
            }

            // 2) Stok uygula
            foreach (var d in details)
            {
                var delta = sign * d.miktar;
                var res = await AdjustStockAsync(irs.depoId, d.malzemeId, delta);
                if (res.ResultStatus != ResultStatus.Success)
                    return new Result(ResultStatus.Error, res.Info ?? "Stok güncellenemedi.");
            }

            // 3) Onayla + toplamı hesapla + kilitle
            irs.durum = IrsaliyeDurumu.Onayli;
            irs.toplamTutar = details.Sum(x => x.miktar * x.birimFiyat);
            await _unitOfWork.Irsaliye.UpdateAsync(irs);

            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "İrsaliye onaylandı ve stok uygulandı.");
        }

        // ---- Satır Bazlı (Taslak’ta serbest, Onaylı’da yasak) ----
        public async Task<IResult> AddLineAsync(int irsaliyeId, IrsaliyeDetayCreateDto lineDto)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == irsaliyeId);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");
            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı irsaliyeye satır eklenemez.");

            var det = _mapper.Map<IrsaliyeDetay>(lineDto);
            det.irsaliyeId = irsaliyeId;
            det.araToplam = det.miktar * det.birimFiyat;

            await _unitOfWork.IrsaliyeDetay.AddAsync(det);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "Satır eklendi (Taslak).");
        }

        public async Task<IResult> RemoveLineAsync(int irsaliyeId, int detayId)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == irsaliyeId);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");
            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı irsaliyeden satır silinemez.");

            var det = await _unitOfWork.IrsaliyeDetay.GetAsync(d => d.Id == detayId && d.irsaliyeId == irsaliyeId);
            if (det == null) return new Result(ResultStatus.Error, "Detay bulunamadı.");

            await _unitOfWork.IrsaliyeDetay.DeleteAsync(det);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Satır silindi (Taslak).");
        }

        public Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAsync(int malzemeId)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<IrsaliyeListDto>>> GetAllByDepoIdAsync(int depoId)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAndDepoIdAsync(int malzemeId, int depoId)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IrsaliyeDto>> Create(IrsaliyeCreateDto irsaliyeCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IrsaliyeDto>> Update(IrsaliyeUpdateDto irsaliyeUpdateDto)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<IrsaliyeDto>>> GetAllByNonDeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
