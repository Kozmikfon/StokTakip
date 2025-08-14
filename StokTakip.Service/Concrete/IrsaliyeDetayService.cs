using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.IrsaliyeDetayDtos;
using StokTakip.Entities.Enums;
using StokTakip.Service.Abstract;
using StokTakip.Shared.Utilities.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using StokTakip.Shared.Utilities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StokTakip.Service.Concrete
{
    public class IrsaliyeDetayService : IIrsaliyeDetayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IrsaliyeDetayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ----- CREATE (Taslakta; stok oynamaz) -----
        public async Task<IDataResult<IrsaliyeDetayDto>> CreateAsync(IrsaliyeDetayCreateDto dto)
        {
            if (dto == null)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "Geçersiz veri.", null);

            // BUG FIX: dto.Id değil, dto.irsaliyeId
            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == dto.irsaliyeId);
            if (irs == null)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "İrsaliye bulunamadı.", null);

            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new DataResult<IrsaliyeDetayDto>(ResultStatus.Error, "Onaylı irsaliyeye satır eklenemez.", null);

            var detay = _mapper.Map<IrsaliyeDetay>(dto);
            detay.araToplam = detay.miktar * detay.birimFiyat;

            await _unitOfWork.IrsaliyeDetay.AddAsync(detay);
            await _unitOfWork.SaveAsync();

            // İsteğe bağlı: navigation yükleme (gerekirse)
            // await _unitOfWork.Context.Entry(detay).Reference(d => d.irsaliye).LoadAsync();
            // await _unitOfWork.Context.Entry(detay).Reference(d => d.malzeme).LoadAsync();

            var outDto = _mapper.Map<IrsaliyeDetayDto>(detay);
            return new DataResult<IrsaliyeDetayDto>(ResultStatus.Success, "Satır eklendi (Taslak).", outDto);
        }

        // ----- DELETE (Taslakta; stok oynamaz) -----
        public async Task<IResult> DeleteAsync(int detayId)
        {
            var detay = await _unitOfWork.IrsaliyeDetay.GetAsync(d => d.Id == detayId);
            if (detay == null)
                return new Result(ResultStatus.Error, "Detay bulunamadı.");

            var irs = await _unitOfWork.Irsaliye.GetAsync(i => i.Id == detay.irsaliyeId);
            if (irs == null)
                return new Result(ResultStatus.Error, "İrsaliye bulunamadı.");

            if (irs.durum != IrsaliyeDurumu.Taslak)
                return new Result(ResultStatus.Error, "Onaylı irsaliyeden satır silinemez.");

            await _unitOfWork.IrsaliyeDetay.DeleteAsync(detay);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Satır silindi (Taslak).");
        }

        // ----- LIST by header -----
        public async Task<IDataResult<List<IrsaliyeDetayDto>>> GetByIrsaliyeIdAsync(int irsaliyeId)
        {
            var list = await _unitOfWork.IrsaliyeDetay.GetAllAsync(
                x => x.irsaliyeId == irsaliyeId,
                x => x.irsaliye,
                x => x.malzeme
            );

            var dtoList = _mapper.Map<List<IrsaliyeDetayDto>>(list);
            return new DataResult<List<IrsaliyeDetayDto>>(ResultStatus.Success, dtoList);
        }
    }
}
