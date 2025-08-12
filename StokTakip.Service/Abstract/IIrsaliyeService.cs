using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos; // <- satır ekleme/çıkarma için
using StokTakip.Shared.Utilities.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IIrsaliyeService
    {
        
       
        Task<IDataResult<IrsaliyeDto>> Get(int id);                
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllAsync();
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAsync(int malzemeId);
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByDepoIdAsync(int depoId);
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAndDepoIdAsync(int malzemeId, int depoId);

        
        // Create: irsaliyeyi ve satırlarını kaydederken stok anında +/- güncellenir (transaction içinde).
        Task<IDataResult<IrsaliyeDto>> Create(IrsaliyeCreateDto irsaliyeCreateDto);

        Task<IDataResult<IrsaliyeDto>> Update(IrsaliyeUpdateDto irsaliyeUpdateDto);

        Task<IDataResult<IrsaliyeDto>> CreateHeaderAsync(IrsaliyeCreateDto headerDto);

        Task<IResult> Delete(int id);

        Task<IDataResult<List<IrsaliyeDto>>> GetAllByNonDeleteAsync();


        // Satır bazlı ekleme/çıkarma; her çağrıda stok anında +/- güncellenir.
        Task<IResult> AddLineAsync(int irsaliyeId, IrsaliyeDetayCreateDto lineDto);
        Task<IResult> RemoveLineAsync(int irsaliyeId, int detayId);
    }
}
