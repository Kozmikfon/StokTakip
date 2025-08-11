using AutoMapper;
using StokTakip.Data.Abstract;
using StokTakip.Entities.Concrete;
using StokTakip.Entities.Dtos.DepoTransferDtos;
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
    public class DepoTransferService : IDepoTransferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepoTransferService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDataResult<DepoTransferDto>> CreateAsync(DepoTransferCreateDto dto)
        {
            var entity = _mapper.Map<DepoTransfer>(dto);
            await _unitOfWork.DepoTransfer.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            var result = _mapper.Map<DepoTransferDto>(entity);
            return new DataResult<DepoTransferDto>(ResultStatus.Success,"Transfer başarıyla oluşturuldu", result);

        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var depoTransfer = await _unitOfWork.DepoTransfer.GetAsync(x => x.Id == id);
            if (depoTransfer == null)
                return new Result(ResultStatus.Error, "Hata, silinecek malzeme bulunamadı.");

            // HARD DELETE
            await _unitOfWork.DepoTransfer.DeleteAsync(depoTransfer); // <- repo’nuzda varsa bunu kullan
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, "Malzeme veritabanından silindi.");
        }

        public async Task<IDataResult<List<DepoTransferListDto>>> GetAllAsync()
        {
            var transfers = _unitOfWork.DepoTransfer.GetAllAsync(null, x => x.kaynakDepo, x => x.hedefDepo);
            var dtoList = _mapper.Map<List<DepoTransferListDto>>(transfers);
            return new DataResult<List<DepoTransferListDto>>(ResultStatus.Success, "Transfer başarılı",dtoList);
        }

        public async Task<IDataResult<List<DepoTransferListDto>>> GetAllByNonDeleteAndActiveAsync()
        {
            var transfers = await _unitOfWork.DepoTransfer.GetAllAsync(x => !x.IsDelete && x.IsActive, x => x.kaynakDepo, x => x.hedefDepo);
            var dtoList = _mapper.Map<List<DepoTransferListDto>>(transfers);
            return new DataResult<List<DepoTransferListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<List<DepoTransferListDto>>> GetAllByNonDeleteAsync()
        {
            var transfers = await _unitOfWork.DepoTransfer.GetAllAsync(x => !x.IsDelete, x => x.kaynakDepo, x => x.hedefDepo);
            var dtoList = _mapper.Map<List<DepoTransferListDto>>(transfers);
            return new DataResult<List<DepoTransferListDto>>(ResultStatus.Success, dtoList);
        }

        public async Task<IDataResult<DepoTransferDto>> GetAsync(int id)
        {
            var transfer = await _unitOfWork.DepoTransfer.GetAsync(x => x.Id==id,x=>x.kaynakDepo,x=>x.hedefDepo);
            if (transfer == null)
            {
                return new DataResult<DepoTransferDto>(ResultStatus.Error, "Hata, istenilen transfer bulunamadı.", null);
            }
            var dto= _mapper.Map<DepoTransferDto>(transfer);
            return new DataResult<DepoTransferDto>(ResultStatus.Success,"transfer başarılı", dto);


        }

        public async Task<IDataResult<DepoTransferDto>> UpdateAsync(DepoTransferUpdateDto dto)
        {
            var updatedEntity = await _unitOfWork.DepoTransfer.GetAsync(x => x.Id == dto.Id);
            if (updatedEntity == null)
            {
                return new DataResult<DepoTransferDto>(ResultStatus.Error, "Hata, istenilen transfer bulunamadı.", null);
            }
            var updatedTransfer = _mapper.Map(dto, updatedEntity);
            await _unitOfWork.DepoTransfer.UpdateAsync(updatedTransfer);
            await _unitOfWork.SaveAsync();
            var result = _mapper.Map<DepoTransferDto>(updatedTransfer);
            return new DataResult<DepoTransferDto>(ResultStatus.Success, "Transfer başarıyla güncellendi.", result);
        }
    }
}
