using StokTakip.Entities.Dtos.DepoTransferDtos;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IDepoTransferService
    {
        Task<IDataResult<DepoTransferDto>> GetAsync(int id);
        Task<IDataResult<List<DepoTransferListDto>>> GetAllAsync();
        Task<IDataResult<List<DepoTransferListDto>>> GetAllByNonDeleteAsync();
        Task<IDataResult<List<DepoTransferListDto>>> GetAllByNonDeleteAndActiveAsync();

        Task<IDataResult<DepoTransferDto>> CreateAsync(DepoTransferCreateDto dto);
        Task<IDataResult<DepoTransferDto>> UpdateAsync(DepoTransferUpdateDto dto);
        Task<IResult> DeleteAsync(int id);
    }
}
