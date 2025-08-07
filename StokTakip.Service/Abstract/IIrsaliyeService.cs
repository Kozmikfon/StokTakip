using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IIrsaliyeService
    {
        Task<IDataResult<IrsaliyeDto>> Get(int id);
        Task<IDataResult<IrsaliyeDto>> Create(IrsaliyeCreateDto irsaliyeCreateDto);
        Task<IDataResult<IrsaliyeDto>> Update(IrsaliyeUpdateDto irsaliyeUpdateDto);
        Task<IResult> Delete(int id);
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllAsync();//tüm kayutlar
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAsync(int malzemeId);
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByDepoIdAsync(int depoId);
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAndDepoIdAsync(int malzemeId, int depoId);

        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAsync(); //silinmemiş kayıtlat
        Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAndActionAsync();    // hem silinmemiş hemde aktif olan kayıtlar

    }
}
