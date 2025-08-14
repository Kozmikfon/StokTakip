
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Shared.Utilities.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    
    public interface IIrsaliyeService
    {
        // ----- READ -----
        Task<IDataResult<IrsaliyeDto>> Get(int id);
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllAsync();
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAsync();
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByDepoIdAsync(int depoId);
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAsync(int malzemeId);
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAndDepoIdAsync(int malzemeId, int depoId);


        Task<IDataResult<IrsaliyeDto>> CreateHeaderAsync(IrsaliyeCreateDto dto);

        Task<IDataResult<IrsaliyeDto>> UpsertAsync(IrsaliyeUpdateDto dto);


        Task<IResult> Delete(int id);


        Task<IResult> AddLineAsync(int irsaliyeId, IrsaliyeDetayCreateDto lineDto);
        Task<IResult> RemoveLineAsync(int irsaliyeId, int detayId);

        Task<IResult> TalepOlusturAsync(int irsaliyeId);
    }
}
