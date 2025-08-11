using AutoMapper;
using StokTakip.Data.Abstract;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IrsaliyeService(IUnitOfWork unitOfWork, IMapper mapper)
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
                return new DataResult<IrsaliyeDto>(ResultStatus.Success,"İrsaliye başarılı şekilde oluşturuldu", result);
            
        }

        public async Task<IResult> Delete(int id)
        {
            var irsaliye = await _unitOfWork.Irsaliye.GetAsync(x => x.Id == id);
            if (irsaliye == null)
                return new Result(ResultStatus.Error, "Hata, silinecek malzeme bulunamadı.");

            // HARD DELETE
            await _unitOfWork.Irsaliye.DeleteAsync(irsaliye); // <- repo’nuzda varsa bunu kullan
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Malzeme veritabanından silindi.");
        }

        public async Task<IDataResult<IrsaliyeDto>> Get(int id)
        {
            var entity=await _unitOfWork.Irsaliye.GetAsync(x => x.Id == id);
            if (entity==null)
            {
                return new DataResult<IrsaliyeDto>(ResultStatus.Error, "Hata, istenilen irsaliye bulunamadı.", null);
            }
            var dto=_mapper.Map<IrsaliyeDto>(entity);
            return new DataResult<IrsaliyeDto>(ResultStatus.Success, dto);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllAsync()
        {
           var list=await _unitOfWork.Irsaliye.GetAllAsync();
            if (list == null || !list.Any())
            {
                return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Error, "Hata, irsaliye listesi bulunamadı.", null);
            }
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByDepoIdAsync(int depoId)
        {
            var list = _unitOfWork.Irsaliye.GetAllAsync(x => x.depoId == depoId);
            var dtolist=_mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtolist);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAndDepoIdAsync(int malzemeId, int depoId)
        {
            var details=await _unitOfWork.IrsaliyeDetay.GetAllAsync(x => x.malzemeId == malzemeId);
            var irsaliyeIds=details.Select(d => d.irsaliyeId).ToList();
            var irsaliyes = await _unitOfWork.Irsaliye.GetAllAsync(x => irsaliyeIds.Contains(x.Id) && x.depoId == depoId);
            
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(irsaliyes);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByMalzemeIdAsync(int malzemeId)
        {
            var details = await _unitOfWork.IrsaliyeDetay.GetAllAsync(x => x.malzemeId == malzemeId);
            var ırsaliyeIds=details.Select(x => x.irsaliyeId).ToList();
            var irsaliyes = await _unitOfWork.Irsaliye.GetAllAsync(x => ırsaliyeIds.Contains(x.Id));

            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(irsaliyes);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);

        }


        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAndActionAsync()
        {
            var list = await _unitOfWork.Irsaliye.GetAllAsync(x => !x.IsDelete && x.IsActive);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<IrsaliyeListDto>>> GetAllByNonDeleteAsync()
        {
            var list=await _unitOfWork.Irsaliye.GetAllAsync(x => !x.IsDelete);
            var dtoList = _mapper.Map<List<IrsaliyeListDto>>(list);
            return new DataResult<List<IrsaliyeListDto>>(ResultStatus.Success, dtoList);
        }

        public Task<IDataResult<IrsaliyeDto>> Update(IrsaliyeUpdateDto irsaliyeUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
