using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Entities.Dtos.DepoTransferDtos;
using StokTakip.Shared.Utilities.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IDepoTransferService
    {
        // ----- READ -----
        Task<IDataResult<DepoTransferDto>> GetAsync(int id);
        Task<IDataResult<List<DepoTransferListDto>>> GetAllAsync();
        Task<IDataResult<List<DepoTransferListDto>>> GetAllByNonDeleteAndActiveAsync();

        // ----- HEADER (yalnız Taslak) -----
        Task<IDataResult<DepoTransferDto>> CreateHeaderAsync(DepoTransferCreateDto dto);
        Task<IDataResult<DepoTransferDto>> UpsertAsync(DepoTransferUpdateDto dto); // sadece Taslak
        Task<IResult> DeleteAsync(int id);                                         // sadece Taslak (soft)

        // ----- DETAY (yalnız Taslak) -----
        Task<IResult> AddLineAsync(int transferId, DepoTransferDetayCreateDto dto);
        Task<IResult> RemoveLineAsync(int transferId, int detayId);
        Task<IDataResult<List<DepoTransferDetayDto>>> GetLinesAsync(int transferId);

        // ----- ONAY (tek adım: stok hareketleri + Durum=Onaylı) -----
        // İrsaliyedeki Onay/“TalepOlustur” butonu ile aynı davranış:
        Task<IResult> TalepOlusturAsync(int transferId, string onaylayanKullanici);
    }
}
