using StokTakip.Entities.Dtos.DepoDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Concrete
{
    public class DepoService : IDepoService
    {
        public Task<IDataResult<DepoDto>> Create(DepoCreateDto depoCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<DepoDto>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<DepoDto>> Update(DepoUpdateDto depoUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
