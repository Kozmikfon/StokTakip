using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.CariDtos;
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
    public class CariService : ICariService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CariService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var cari = await _unitOfWork.Cari.GetAsync(x => x.Id == id);
            if (cari == null)
                return new Result(ResultStatus.Error, "Hata, silinecek malzeme bulunamadı.");

            // HARD DELETE
            await _unitOfWork.Cari.DeleteAsync(cari); // <- repo’nuzda varsa bunu kullan
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Malzeme veritabanından silindi.");
        }

        public async Task<IDataResult<CariDto>> GetAsync(int id)
        {
            var cari = await _unitOfWork.Cari.GetAsync(x=> x.Id==id);
            if (cari != null)
            {
                var dto=_mapper.Map<CariDto>(cari);
                return new DataResult<CariDto>(ResultStatus.Success,dto);
            }
            return new DataResult<CariDto>(ResultStatus.Error, "Hata, kayıt bulunamadı", null);
        }

        public async Task<IDataResult<CariDto>> UpdateAsync(CariUpdateDto cariUpdateDto)
        {
            if (cariUpdateDto!=null)
            {
                var entity = _mapper.Map<Cari>(cariUpdateDto);
                var updated = await _unitOfWork.Cari.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();
                var dto = _mapper.Map<CariDto>(updated);
                return new DataResult<CariDto>(ResultStatus.Success, dto);
            }
            return new DataResult<CariDto>(ResultStatus.Error, "Hata, güncelleme işlemi başarısız", null);

        }
    }
}
