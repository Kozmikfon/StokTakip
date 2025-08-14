
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Shared.Utilities.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{

    public interface IIrsaliyeDetayService
    {
        
        Task<IDataResult<IrsaliyeDetayDto>> CreateAsync(IrsaliyeDetayCreateDto dto);

        
        Task<IResult> DeleteAsync(int detayId);

      
        Task<IDataResult<List<IrsaliyeDetayDto>>> GetByIrsaliyeIdAsync(int irsaliyeId);
    }
}
