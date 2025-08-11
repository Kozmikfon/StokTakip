using StokTakip.Entities.Dtos.CariDtos;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface ICariService
    {
        Task<IDataResult<CariDto>> CreateAsync(CariCreateDto cariCreateDto);
        Task<IDataResult<CariDto>> GetAsync(int id);
        Task<IDataResult<List<CariListDto>>> GetAllAsync();

        Task<IDataResult<CariDto>> UpdateAsync(CariUpdateDto cariUpdateDto);

        Task<IResult> DeleteAsync(int id);
    }
}
