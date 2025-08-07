using AutoMapper;
using StokTakip.Data.Concrete;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.IrsaliyeDtos;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using StokTakip.Shared.Utilities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Service.Concrete
{
    public class IrsaliyeService : IIrsaliyeService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IrsaliyeService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }   

        public async Task<IDataResult<IrsaliyeDto>> Create(IrsaliyeCreateDto irsaliyeCreateDto)
        {
            
                var entity = _mapper.Map<Irsaliye>(irsaliyeCreateDto);
                var added=await _unitOfWork.Irsaliye.AddAsync(entity);
                await _unitOfWork.SaveAsync();

                var result = _mapper.Map<IrsaliyeDto>(added);
                return new DataResult<IrsaliyeDto>(ResultStatus.Success, result);
            
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
