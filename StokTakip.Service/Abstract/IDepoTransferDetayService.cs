using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Shared.Utilities.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IDepoTransferDetayService
    {
        Task<IDataResult<DepoTransferDetayDto>> CreateAsync(DepoTransferDetayCreateDto dto);
        Task<IResult> DeleteAsync(int detayId);                                              
        Task<IDataResult<List<DepoTransferDetayDto>>> GetByTransferIdAsync(int transferId);
    }
}
