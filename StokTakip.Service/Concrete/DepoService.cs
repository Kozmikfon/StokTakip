using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.DepoDtos;
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
    public class DepoService : IDepoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDataResult<DepoDto>> Create(DepoCreateDto depoCreateDto)
        {
            if (depoCreateDto!=null)
            {
                var entity = _mapper.Map<Depo>(depoCreateDto);
                entity.CreatedTime= DateTime.Now;
                var addes= await _unitOfWork.Depo.AddAsync(entity);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<DepoDto>(addes);
                return new DataResult<DepoDto>(ResultStatus.Success, result);
            }
            return new DataResult<DepoDto>(ResultStatus.Error, "Hata, depo oluşturma işlemi başarısız", null);
        }

        public async Task<IResult> Delete(int id)
        {
            var entity= await _unitOfWork.Depo.GetAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.IsDelete = true;
                await _unitOfWork.Depo.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success);
            }
            return new Result(ResultStatus.Error, "Hata, depo silme işlemi başarısız.");
        }

        public async Task<IDataResult<DepoDto>> Get(int id)
        {
            var entity = await _unitOfWork.Depo.GetAsync(x => x.Id == id);
            if (entity != null)
            {
                var result = _mapper.Map<DepoDto>(entity);
                return new DataResult<DepoDto>(ResultStatus.Success, result);
            }
            return new DataResult<DepoDto>(ResultStatus.Error, "Hata, depo bulunamadı", null);
        }

        public async Task<IDataResult<DepoDto>> Update(DepoUpdateDto depoUpdateDto)
        {
            if (depoUpdateDto!=null)
            {
                var entity = _mapper.Map<Depo>(depoUpdateDto);
                var updated = await _unitOfWork.Depo.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<DepoDto>(updated);
                return new DataResult<DepoDto>(ResultStatus.Success, result);
            }
            return new DataResult<DepoDto>(ResultStatus.Error, "Hata, depo güncelleme işlemi başarısız", null);
        }
    }
}
