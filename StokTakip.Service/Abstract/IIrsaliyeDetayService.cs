using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IIrsaliyeDetayService
    {
        Task<IDataResult<IrsaliyeDetayDto>> CreateAsync(IrsaliyeDetayCreateDto irsaliyeDetayCreateDto);
        Task<IResult> DeleteAsync(int detayId);
        Task<IDataResult<List<IrsaliyeDetayDto>>> GetByIrsaliyeIdAsync(int irsaliyeId);
    }
}
