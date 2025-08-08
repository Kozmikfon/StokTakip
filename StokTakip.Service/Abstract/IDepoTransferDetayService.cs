using StokTakip.Entities.Dtos.DepoTransferDetayDtos;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IDepoTransferDetayService
    {
        Task<IDataResult<DepoTransferDetayDto>> Get(int id);
        Task<IDataResult<DepoTransferDetayDto>> Create(DepoTransferDetayCreateDto dto);
        Task<IResult> Delete(int id);

        Task<IDataResult<List<DepoTransferDetayDto>>> GetAllByTransferIdAsync(int transferId);
        Task<IDataResult<List<DepoTransferDetayDto>>> GetAllByNonDeleteAsync();
    }
}
