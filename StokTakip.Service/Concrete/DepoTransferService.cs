using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Entities.Dtos.DepoTransferDtos;
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
    public class DepoTransferService : IDepoTransferService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DepoTransferService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // ---------------- Helpers ----------------
        private static void SoftDelete(EntityBase e)
        {
            e.IsDelete = true;
            e.IsActive = false;
            e.ModifiedTime = DateTime.Now;
        }

        private async Task<decimal> GetKalanMiktarAsync(int depoId, int malzemeId)
        {
            // Vw_StokDurumu veya hareket toplamı üzerinden kalan miktar
            return await _uow.Stok.GetKalanMiktarAsync(depoId, malzemeId);
        }

        private async Task CreateMovementAsync(int depoId, int malzemeId, decimal miktar,
                                               StokHareketTipi tip, string aciklama, int? refNo = null)
        {
            var hareket = new Stok
            {
                DepoId = depoId,
                MalzemeId = malzemeId,
                Miktar = miktar,                 // Giriş: +, Çıkış: -
                HareketTipi = tip,               // TransferGiris / TransferCikis
                Aciklama = aciklama,
                ReferansId = refNo,
                IsActive = true,
                IsDelete = false,
                CreatedTime = DateTime.Now,
                ModifiedTime = DateTime.Now
            };
            await _uow.Stok.AddAsync(hareket);
        }

        // ---------------- Read ----------------
        public async Task<IDataResult<DepoTransferDto>> GetAsync(int id)
        {
            var tr = await _uow.DepoTransfer.GetAsync(
                x => x.Id == id && !x.IsDelete,
                x => x.kaynakDepo,
                x => x.hedefDepo
            );

            if (tr == null)
                return new DataResult<DepoTransferDto>(ResultStatus.Error, "Transfer bulunamadı.", null);

            var detaylar = await _uow.DepoTransferDetay.GetAllAsync(
                d => d.transferId == id && !d.IsDelete,
                d => d.malzeme
            );

            var dto = _mapper.Map<DepoTransferDto>(tr);
            dto.Detaylar = _mapper.Map<List<DepoTransferDetayDto>>(detaylar);
            return new DataResult<DepoTransferDto>(ResultStatus.Success, dto);
        }

        public async Task<IDataResult<List<DepoTransferListDto>>> GetAllAsync()
        {
            var list = await _uow.DepoTransfer.GetAllAsync(
                x => !x.IsDelete,
                x => x.kaynakDepo,
                x => x.hedefDepo
            );
            var dtoList = _mapper.Map<List<DepoTransferListDto>>(list);
            return new DataResult<List<DepoTransferListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<DepoTransferListDto>>> GetAllByNonDeleteAndActiveAsync()
        {
            var list = await _uow.DepoTransfer.GetAllAsync(
                x => !x.IsDelete && x.IsActive,
                x => x.kaynakDepo,
                x => x.hedefDepo
            );
            var dtoList = _mapper.Map<List<DepoTransferListDto>>(list);
            return new DataResult<List<DepoTransferListDto>>(ResultStatus.Success, dtoList);
        }

        // -------------- Header (yalnız Taslak) --------------
        public async Task<IDataResult<DepoTransferDto>> CreateHeaderAsync(DepoTransferCreateDto dto)
        {
            if (dto == null)
                return new DataResult<DepoTransferDto>(ResultStatus.Error, "Geçersiz veri.", null);

            if (dto.KaynakDepoId <= 0 || dto.HedefDepoId <= 0)
                return new DataResult<DepoTransferDto>(ResultStatus.Error, "Kaynak/Hedef depo zorunlu.", null);

            if (dto.KaynakDepoId == dto.HedefDepoId)
                return new DataResult<DepoTransferDto>(ResultStatus.Error, "Kaynak ve hedef depo aynı olamaz.", null);

            if (dto.TransferTarihi == default) dto.TransferTarihi = DateTime.Now;

            var entity = _mapper.Map<DepoTransfer>(dto);
            entity.IsActive = true;
            entity.IsDelete = false;
            entity.CreatedTime = DateTime.Now;
            entity.ModifiedTime = DateTime.Now;
            entity.durum = TransferDurumu.Taslak;

            await _uow.DepoTransfer.AddAsync(entity);
            await _uow.SaveAsync();

            var outDto = _mapper.Map<DepoTransferDto>(entity);
            outDto.Detaylar = new List<DepoTransferDetayDto>();
            return new DataResult<DepoTransferDto>(ResultStatus.Success, "Taslak oluşturuldu.", outDto);
        }

        public async Task<IDataResult<DepoTransferDto>> UpsertAsync(DepoTransferUpdateDto dto)
        {
            var tr = await _uow.DepoTransfer.GetAsync(x => x.Id == dto.Id && !x.IsDelete);
            if (tr == null)
                return new DataResult<DepoTransferDto>(ResultStatus.Error, "Transfer bulunamadı.", null);

            if (tr.durum != TransferDurumu.Taslak)
                return new DataResult<DepoTransferDto>(ResultStatus.Error, "Onaylı transfer güncellenemez.", null);

            if (dto.KaynakDepoId == dto.HedefDepoId)
                return new DataResult<DepoTransferDto>(ResultStatus.Error, "Kaynak ve hedef depo aynı olamaz.", null);

            if (dto.TransferTarihi == default) dto.TransferTarihi = DateTime.Now;

            _mapper.Map(dto, tr);
            tr.ModifiedTime = DateTime.Now;

            await _uow.DepoTransfer.UpdateAsync(tr);
            await _uow.SaveAsync();

            var detaylar = await _uow.DepoTransferDetay.GetAllAsync(d => d.transferId == tr.Id && !d.IsDelete);
            var outDto = _mapper.Map<DepoTransferDto>(tr);
            outDto.Detaylar = _mapper.Map<List<DepoTransferDetayDto>>(detaylar);
            return new DataResult<DepoTransferDto>(ResultStatus.Success, "Taslak güncellendi.", outDto);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var tr = await _uow.DepoTransfer.GetAsync(x => x.Id == id && !x.IsDelete);
            if (tr == null) return new Result(ResultStatus.Error, "Transfer bulunamadı.");

            if (tr.durum != TransferDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı transfer silinemez.");

            // Detayları soft sil
            var detaylar = await _uow.DepoTransferDetay.GetAllAsync(d => d.transferId == id && !d.IsDelete);
            foreach (var d in detaylar)
            {
                SoftDelete(d);
                await _uow.DepoTransferDetay.UpdateAsync(d);
            }

            SoftDelete(tr);
            await _uow.DepoTransfer.UpdateAsync(tr);
            await _uow.SaveAsync();

            return new Result(ResultStatus.Success, "Transfer silindi (soft).");
        }

        // -------------- Detay (yalnız Taslak) --------------
        public async Task<IResult> AddLineAsync(int transferId, DepoTransferDetayCreateDto dto)
        {
            var tr = await _uow.DepoTransfer.GetAsync(x => x.Id == transferId && !x.IsDelete);
            if (tr == null) return new Result(ResultStatus.Error, "Transfer bulunamadı.");
            if (tr.durum != TransferDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı transfere satır eklenemez.");

            var det = _mapper.Map<DepoTransferDetay>(dto);
            det.transferId = transferId;
            det.IsActive = true;
            det.IsDelete = false;
            det.CreatedTime = DateTime.Now;
            det.ModifiedTime = DateTime.Now;

            await _uow.DepoTransferDetay.AddAsync(det);
            await _uow.SaveAsync();

            return new Result(ResultStatus.Success, "Satır eklendi.");
        }

        public async Task<IResult> RemoveLineAsync(int transferId, int detayId)
        {
            var tr = await _uow.DepoTransfer.GetAsync(x => x.Id == transferId && !x.IsDelete);
            if (tr == null) return new Result(ResultStatus.Error, "Transfer bulunamadı.");
            if (tr.durum != TransferDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı transferden satır silinemez.");

            var det = await _uow.DepoTransferDetay.GetAsync(
                d => d.Id == detayId && d.transferId == transferId && !d.IsDelete
            );
            if (det == null) return new Result(ResultStatus.Error, "Detay bulunamadı.");

            SoftDelete(det);
            await _uow.DepoTransferDetay.UpdateAsync(det);
            await _uow.SaveAsync();

            return new Result(ResultStatus.Success, "Satır silindi (soft).");
        }

        public async Task<IDataResult<List<DepoTransferDetayDto>>> GetLinesAsync(int transferId)
        {
            var list = await _uow.DepoTransferDetay.GetAllAsync(
                d => d.transferId == transferId && !d.IsDelete,
                d => d.malzeme
            );
            var dto = _mapper.Map<List<DepoTransferDetayDto>>(list);
            return new DataResult<List<DepoTransferDetayDto>>(ResultStatus.Success, dto);
        }

        // -------------- Onay (tek adım: stok hareketleri + Durum=Onaylı) --------------
        // Onay butonu ile çağrılır: Taslak -> (kontrol, hareket yaz) -> Onaylı
        public async Task<IResult> TalepOlusturAsync(int transferId, string onaylayanKullanici)
        {
            var tr = await _uow.DepoTransfer.GetAsync(
                x => x.Id == transferId && !x.IsDelete,
                x => x.depoTransferDetaylari
            );
            if (tr == null) return new Result(ResultStatus.Error, "Transfer bulunamadı.");
            if (tr.durum == TransferDurumu.Onayli) return new Result(ResultStatus.Success, "Transfer zaten onaylı.");
            if (tr.durum != TransferDurumu.Taslak) return new Result(ResultStatus.Error, "Bu transfer onaylanamaz.");
            if (tr.kaynakDepoId == tr.hedefDepoId) return new Result(ResultStatus.Error, "Kaynak ve hedef depo aynı olamaz.");
            if (tr.depoTransferDetaylari == null || !tr.depoTransferDetaylari.Any()) return new Result(ResultStatus.Error, "Detay olmadan onaylanamaz.");

            using var tx = await _uow.BeginTransactionAsync();
            try
            {
                // 1) TOPLU yeterlilik
                var malzemeIds = tr.depoTransferDetaylari.Select(d => d.malzemeId).Distinct().ToList();
                var kalanlar = await _uow.Stok.GetKalanMiktarlarAsync(tr.kaynakDepoId, malzemeIds);
                foreach (var d in tr.depoTransferDetaylari)
                {
                    kalanlar.TryGetValue(d.malzemeId, out var kalan);
                    if (kalan < d.miktar)
                        throw new InvalidOperationException(
                            $"Yetersiz stok (Kaynak Depo {tr.kaynakDepoId}, Malzeme {d.malzemeId}): {kalan} < {d.miktar}");
                }

                // 2) Hareketler: Kaynak (-), Hedef (+)
                foreach (var d in tr.depoTransferDetaylari)
                {
                    await _uow.Stok.AddAsync(new Stok
                    {
                        DepoId = tr.kaynakDepoId,
                        MalzemeId = d.malzemeId,
                        Miktar = -d.miktar,
                        HareketTipi = StokHareketTipi.TransferCikis,
                        Aciklama = $"TransferNo {tr.transferNo} - Kaynak",
                        ReferansId = tr.transferNo,
                        IsActive = true,
                        IsDelete = false,
                        CreatedTime = DateTime.Now,
                        ModifiedTime = DateTime.Now
                    });
                    await _uow.Stok.AddAsync(new Stok
                    {
                        DepoId = tr.hedefDepoId,
                        MalzemeId = d.malzemeId,
                        Miktar = +d.miktar,
                        HareketTipi = StokHareketTipi.TransferGiris,
                        Aciklama = $"TransferNo {tr.transferNo} - Hedef",
                        ReferansId = tr.transferNo,
                        IsActive = true,
                        IsDelete = false,
                        CreatedTime = DateTime.Now,
                        ModifiedTime = DateTime.Now
                    });
                }

                // 3) Durum
                tr.durum = TransferDurumu.Onayli;
                //tr.OnaylayanKullanici = onaylayanKullanici;
                tr.transferTarihi = DateTime.Now;
                tr.ModifiedTime = DateTime.Now;
                await _uow.DepoTransfer.UpdateAsync(tr);
                await _uow.SaveAsync();

                await tx.CommitAsync();
                return new Result(ResultStatus.Success, "Transfer onaylandı ve stok hareketleri işlendi.");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return new Result(ResultStatus.Error, $"Onay sırasında hata: {ex.Message}");
            }
        }

    }
}
