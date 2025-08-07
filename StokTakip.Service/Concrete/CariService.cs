using StokTakip.Entities.Dtos.CariDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Concrete
{
    public class CariService : ICariService
    {
        public Task<IDataResult<CariDto>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<CariDto>> UpdateAsync(CariUpdateDto cariUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
