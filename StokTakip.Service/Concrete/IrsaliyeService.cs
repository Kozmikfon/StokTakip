using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
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
            var stok = await _unitOfWork.Stok.GetAsync(s => s.DepoId == depoId && s.MalzemeId == malzemeId && !s.IsDelete);
            var isNew = false;

            if (stok == null)
            {
                if (delta < 0)
                    return new Result(ResultStatus.Error, "Yetersiz stok (kayıt yok).");

                stok = new Stok
                {
                    DepoId = depoId,
                    MalzemeId = malzemeId,
                    Miktar = 0,
                    IsActive = true,
                    IsDelete = false,
                    CreatedTime = DateTime.Now,
                    ModifiedTime = DateTime.Now
                };
                isNew = true;
            }

            var yeniMiktar = stok.Miktar + delta;
            if (yeniMiktar < 0)
                return new Result(ResultStatus.Error, "Yetersiz stok.");

            stok.Miktar = yeniMiktar;
            stok.ModifiedTime = DateTime.Now;

            if (isNew) await _unitOfWork.Stok.AddAsync(stok);
            else await _unitOfWork.Stok.UpdateAsync(stok);

            return new Result(ResultStatus.Success);
        }

        private static void SoftDeleteEntity(EntityBase e)
        {
            e.IsDelete = true;
            e.IsActive = false;
            e.ModifiedTime = DateTime.Now;
        }

        // ---- READ ----
        public async Task<IDataResult<IrsaliyeDto>> Get(int id)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(x => x.Id == id && !x.IsDelete);
            if (irs == null)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "İrsaliye bulunamadı.", null);

            var detaylar = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == id && !d.IsDelete);
            var dto = _mapper.Map<IrsaliyeDto>(irs);
            dto.Detaylar = _mapper.Map<List<IrsaliyeDetayDto>>(detaylar);
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, dto);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllAsync()
        {
            var list = await _unitOfWork.Irsaliye.GetAllAsync(x => !x.IsDelete);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAsync()
        {
            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => !i.IsDelete);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByDepoIdAsync(int depoId)
        {
            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => i.depoId == depoId && !i.IsDelete);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAsync(int malzemeId)
        {
            var irsIds = (await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.malzemeId == malzemeId && !d.IsDelete))
                         .Select(d => d.irsaliyeId)
                         .Distinct()
                         .ToList();

            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => irsIds.Contains(i.Id) && !i.IsDelete);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAndDepoIdAsync(int malzemeId, int depoId)
        {
            var irsIds = (await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.malzemeId == malzemeId && !d.IsDelete))
                         .Select(d => d.irsaliyeId)
                         .Distinct()
                         .ToList();

            var list = await _unitOfWork.Irsaliye.GetAllAsync(i => i.depoId == depoId && irsIds.Contains(i.Id) && !i.IsDelete);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        // ---- CREATE HEADER (Taslak) ----
        public async Task<IDataResult<IrsaliyeDto>> CreateHeaderAsync(IrsaliyeCreateDto headerDto)
        {
            if (headerDto == null)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "Geçersiz veri.", null);

            var irs = _mapper.Map<Irsaliye>(headerDto);
            irs.CreatedTime = DateTime.Now;
            irs.ModifiedTime = DateTime.Now;
            irs.IsActive = true;
            irs.IsDelete = false;
            irs.durum = IrsaliyeDurumu.Taslak; // Taslak olarak başlat
            irs.toplamTutar = 0m; // taslakta isteğe bağlı hesaplanabilir

            await _unitOfWork.Irsaliye.AddAsync(irs);
            await _unitOfWork.SaveAsync();

            var outDto = _mapper.Map<IrsaliyeDto>(irs);
            outDto.Detaylar = new List<IrsaliyeDetayDto>();
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, outDto);
        }

        // ---- UPSERT (Taslak) ----
        public async Task<IDataResult<IrsaliyeDto>> UpsertAsync(IrsaliyeUpdateDto dto)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == dto.Id && !i.IsDelete);
            if (irs == null)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "İrsaliye bulunamadı.", null);

            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "Onaylı irsaliye güncellenemez.", null);

            // Başlık güncelle
            _mapper.Map(dto, irs);
            irs.ModifiedTime = DateTime.Now;
            await _unitOfWork.Irsaliye.UpdateAsync(irs);

            // Taslakta basit yaklaşım: eski aktif detayları soft delete et, yenilerini ekle
            var oldDetails = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irs.Id && !d.IsDelete);
            foreach (var od in oldDetails)
            {
                SoftDeleteEntity(od);
                await _unitOfWork.IrsaliyeDetay.UpdateAsync(od);
            }

            if (dto.Detaylar != null && dto.Detaylar.Any())
            {
                foreach (var ndto in dto.Detaylar)
                {
                    var det = _mapper.Map<IrsaliyeDetay>(ndto);
                    det.irsaliyeId = irs.Id;
                    det.araToplam = det.miktar * det.birimFiyat;
                    det.IsActive = true;
                    det.IsDelete = false;
                    det.CreatedTime = DateTime.Now;
                    det.ModifiedTime = DateTime.Now;
                    await _unitOfWork.IrsaliyeDetay.AddAsync(det);
                }
            }

            // Görsel amaçlı toplam (taslakta)
            var newDetails = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irs.Id && !d.IsDelete);
            irs.toplamTutar = newDetails.Sum(x => x.miktar * x.birimFiyat);
            irs.ModifiedTime = DateTime.Now;

            await _unitOfWork.SaveAsync();

            var outDto = _mapper.Map<IrsaliyeDto>(irs);
            outDto.Detaylar = _mapper.Map<List<IrsaliyeDetayDto>>(newDetails);
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, outDto);
        }

        // ---- DELETE (Soft, sadece Taslak) ----
        public async Task<IResult> Delete(int id)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == id && !i.IsDelete);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");

            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı irsaliye silinemez.");

            // Detayları soft delete
            var details = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == id && !d.IsDelete);
            foreach (var d in details)
            {
                SoftDeleteEntity(d);
                await _unitOfWork.IrsaliyeDetay.UpdateAsync(d);
            }

            // Başlığı soft delete
            SoftDeleteEntity(irs);
            await _unitOfWork.Irsaliye.UpdateAsync(irs);

            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "Taslak irsaliye silindi (soft).");
        }

        // ---- Talep/Onay (Stok burada uygulanır) ----
        public async Task<IResult> TalepOlusturAsync(int irsaliyeId)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == irsaliyeId && !i.IsDelete);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");
            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "İrsaliye zaten onaylı.");
            if (irs.irsaliyeTipi == IrsaliyeTipi.Transfer)
                return new Result(ResultStatus.Error, "Transfer bu akışta onaylanmaz.");

            var details = await _unitOfWork.IrsaliyeDetay.GetAllAsync(d => d.irsaliyeId == irs.Id && !d.IsDelete);
            if (details == null || !details.Any())
                return new Result(ResultStatus.Error, "İrsaliyede satır yok.");

            var sign = GetSign(irs.irsaliyeTipi);

            // 1) Çıkış ise yeterlilik kontrolü
            if (irs.irsaliyeTipi == IrsaliyeTipi.Cikis)
            {
                foreach (var d in details)
                {
                    var stok = await _unitOfWork.Stok.GetAsync(s => s.DepoId == irs.depoId && s.MalzemeId == d.malzemeId && !s.IsDelete);
                    var mevcut = stok?.Miktar ?? 0;
                    if (mevcut < d.miktar)
                        return new Result(ResultStatus.Error, $"Yetersiz stok: MalzemeId={d.malzemeId}, gerekli={d.miktar}, mevcut={mevcut}.");
                }
            }

            // 2) Stok uygula (transaction kullanıyorsan buraya al)
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
            irs.ModifiedTime = DateTime.Now;
            await _unitOfWork.Irsaliye.UpdateAsync(irs);

            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "İrsaliye onaylandı ve stok uygulandı.");
        }

        // ---- Satır Bazlı (Taslak; soft delete) ----
        public async Task<IResult> AddLineAsync(int irsaliyeId, IrsaliyeDetayCreateDto lineDto)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == irsaliyeId && !i.IsDelete);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");
            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı irsaliyeye satır eklenemez.");

            var det = _mapper.Map<IrsaliyeDetay>(lineDto);
            det.irsaliyeId = irsaliyeId;
            det.araToplam = det.miktar * det.birimFiyat;
            det.IsActive = true;
            det.IsDelete = false;
            det.CreatedTime = DateTime.Now;
            det.ModifiedTime = DateTime.Now;

            await _unitOfWork.IrsaliyeDetay.AddAsync(det);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "Satır eklendi (Taslak).");
        }

        public async Task<IResult> RemoveLineAsync(int irsaliyeId, int detayId)
        {
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == irsaliyeId && !i.IsDelete);
            if (irs == null) return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");
            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı irsaliyeden satır silinemez.");

            var det = await _unitOfWork.IrsaliyeDetay.GetAsync(d => d.Id == detayId && d.irsaliyeId == irsaliyeId && !d.IsDelete);
            if (det == null) return new Result(ResultStatus.Error, "Detay bulunamadı.");

            SoftDeleteEntity(det);
            await _unitOfWork.IrsaliyeDetay.UpdateAsync(det);

            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "Satır silindi (soft).");
        }
    }
}
