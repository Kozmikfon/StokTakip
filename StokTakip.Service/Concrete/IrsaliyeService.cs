using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Concrete
{
    public class IrsaliyeService : IIrsaliyeService
    {
        public Task<IDataResult<IrsaliyeDto>> Create(IrsaliyeCreateDto irsaliyeCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IrsaliyeDto>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<IrsaliyeListDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<IrsaliyeListDto>>> GetAllByDepoIdAsync(int depoId)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAndDepoIdAsync(int malzemeId, int depoId)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAsync(int malzemeId)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAndActionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IrsaliyeDto>> Update(IrsaliyeUpdateDto irsaliyeUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
