using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.MalzemeDtos;
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
    public class MalzemeService : IMalzemeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MalzemeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDataResult<MalzemeDto>> CreateAsync(MalzemeCreateDto malzemeCreateDto)
        {
            if (malzemeCreateDto!=null)
            {
                var entity = _mapper.Map<Malzeme>(malzemeCreateDto);
                entity.CreatedTime= DateTime.Now;
                var addes = await _unitOfWork.Malzeme.AddAsync(entity);
                await _unitOfWork.SaveAsync();
                var result = _mapper.Map<MalzemeDto>(entity);
                return new DataResult<MalzemeDto>(ResultStatus.Success, result);

            }
            return new DataResult<MalzemeDto>(ResultStatus.Error, "Hata, malzeme oluşturma işlemi başarısız", null);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var malzeme = await _unitOfWork.Malzeme.GetAsync(x => x.Id == id);
            if (malzeme == null)
                return new Result(ResultStatus.Error, "Hata, silinecek malzeme bulunamadı.");

            if (malzeme.IsDelete)
                return new Result(ResultStatus.Warning, "Malzeme zaten silinmiş.");

            malzeme.IsDelete = true;
            await _unitOfWork.Malzeme.UpdateAsync(malzeme);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Malzeme başarıyla silindi.");
        }

        public async Task<IDataResult<List<MalzemeListDto>>> GetAllAsync()
        {
            var malzemeList = await _unitOfWork.Malzeme.GetAllAsync();
            var dtoList = _mapper.Map<List<MalzemeListDto>>(malzemeList);

            return new DataResult<List<MalzemeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<MalzemeDto>> GetAsync(int id)
        {
            var entity= await _unitOfWork.Malzeme.GetAsync(x => x.Id == id);
            if (entity != null)
            {
                var dto = _mapper.Map<MalzemeDto>(entity);
                return new DataResult<MalzemeDto>(ResultStatus.Success, dto);
            }
            return new DataResult<MalzemeDto>(ResultStatus.Error, "Hata, kayıt bulunamadı", null);

        }
    }
}
