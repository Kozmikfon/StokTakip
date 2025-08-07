using StokTakip.Entities.Dtos.MalzemeDtos;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IMalzemeService
    {
        Task<IDataResult<MalzemeDto>> GetAsync(int id);
        Task<IDataResult<MalzemeDto>> CreateAsync(MalzemeCreateDto malzemeCreateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<List<MalzemeListDto>>> GetAllAsync();


    }
}
