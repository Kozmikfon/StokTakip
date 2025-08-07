using StokTakip.Entities.Dtos.DepoDtos;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Abstract
{
    public interface IDepoService
    {
        Task<IDataResult<DepoDto>>Get(int id);
        Task<IDataResult<DepoDto>> Create(DepoCreateDto depoCreateDto);
        Task<IDataResult<DepoDto>> Update(DepoUpdateDto depoUpdateDto);
        Task<IResult> Delete(int id);
    }
}
